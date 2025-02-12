using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly IDbContextFactory<OMAContext> _contextFactory;
    private readonly IMapper _mapper;

    public CustomerService(IDbContextFactory<OMAContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    #region Get

    public async Task<List<Customer>> GetCustomersAndOrders()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Customers
            .Where(c => !c.IsDeleted)
            .Include(c => c.Orders)
            .Include(c => c.Address)
            .ToListAsync();
    }

    #endregion Get

    #region Post and Put

    public async Task<Customer> AddOrUpdateCustomerAsync(CustomerModel customerModel)
    {
        using var context = _contextFactory.CreateDbContext();

        var customer = await context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == customerModel.Id);

        if (customer == null)
        {
            customer = _mapper.Map<Customer>(customerModel);
            await context.Customers.AddAsync(customer);
        }
        else
        {
            _mapper.Map(customerModel, customer);
            context.Customers.Update(customer);
        }

        await context.SaveChangesAsync();
        return customer;
    }

    #endregion Post and Put

    #region Delete

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        using var context = _contextFactory.CreateDbContext();

        var customer = await context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
            throw new Exception($"Customer {customerId} not found");

        customer.IsDeleted = true;

        await context.Orders
            .Where(o => o.CustomerId == customerId)
            .ForEachAsync(o => o.IsDeleted = true);

        return await context.SaveChangesAsync() > 0;
    }

    #endregion Delete
}