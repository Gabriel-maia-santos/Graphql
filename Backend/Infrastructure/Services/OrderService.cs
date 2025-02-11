using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IDbContextFactory<OMAContext> _contextFactory;

    public OrderService(IDbContextFactory<OMAContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region Get

    public IQueryable<Order> GetOrders()
    {
        var context = _contextFactory.CreateDbContext();
        context.Database.EnsureCreated();
        return context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.Customer);
        ;
    }

    #endregion Get

    #region Post and Put

    public async Task<Order> AddOrUpdateOrderAsync(OrderModel orderModel)
    {
        var context = _contextFactory.CreateDbContext();
        Order order;

        var customer = await context.Customers
            .Where(c => c.Id == orderModel.CustomerId)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            throw new Exception($"Customer o id {orderModel.CustomerId} não foi encontrado!");
        }

        if (orderModel.Id == null)
        {
            order = new Order
            {
                Id = orderModel.CustomerId,
                OrderDate = orderModel.OrderDate,
                Description = orderModel.Description,
                TotalAmount = orderModel.TotalAmount,
                DepositAmount = orderModel.DepositAmount,
                IsDelivery = orderModel.IsDelivery,
                Status = orderModel.Status,
                OtherNotes = orderModel.OtherNotes
            };

            await context.Orders.AddAsync(order);
        }
        else
        {
            order = await context.Orders
                    .Where(o => o.Id == orderModel.CustomerId)
                    .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new Exception($"Customer o id {order.CustomerId} não foi encontrado!");
            }

            order.OrderDate = orderModel.OrderDate;
            order.Description = orderModel.Description;
            order.TotalAmount = orderModel.TotalAmount;
            order.DepositAmount = orderModel.DepositAmount;
            order.Status = orderModel.Status;
            order.OtherNotes = orderModel.OtherNotes;

            context.Orders.Update(order);
        }

        await context.SaveChangesAsync();

        return order;
    }

    #endregion Post and Put

    #region Delete

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var context = _contextFactory.CreateDbContext();

        var order = await context.Orders
                    .Where(o => o.Id == orderId)
                    .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new Exception($"Order with id {orderId} was not found!");
        }

        order.IsDelivery = true;

        context.Orders.Update(order);
        return await context.SaveChangesAsync() > 0;
    }

    #endregion Delete
}