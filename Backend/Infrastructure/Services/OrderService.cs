using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IDbContextFactory<OMAContext> _contextFactory;
    private readonly IMapper _mapper;

    public OrderService(IDbContextFactory<OMAContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    #region Get

    public async Task<List<Order>> GetOrders()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Customer)
            .ToListAsync();
    }

    #endregion Get

    #region Post and Put

    public async Task<Order> AddOrUpdateOrderAsync(OrderModel orderModel)
    {
        using var context = _contextFactory.CreateDbContext();

        var customer = await context.Customers.FindAsync(orderModel.CustomerId);
        if (customer == null)
        {
            throw new Exception($"Cliente com ID {orderModel.CustomerId} não foi encontrado!");
        }

        var order = await context.Orders.FindAsync(orderModel.Id);

        if (order == null)
        {
            order = _mapper.Map<Order>(orderModel);
            await context.Orders.AddAsync(order);
        }
        else
        {
            _mapper.Map(orderModel, order);
            context.Orders.Update(order);
        }

        await context.SaveChangesAsync();
        return order;
    }

    #endregion Post and Put

    #region Delete

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        using var context = _contextFactory.CreateDbContext();

        var order = await context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new Exception($"Pedido com ID {orderId} não foi encontrado!");
        }

        order.IsDeleted = true;
        context.Orders.Update(order);

        return await context.SaveChangesAsync() > 0;
    }

    #endregion Delete
}