using Core.Entities;
using Core.Interfaces;

namespace API.GraphQL;

public class Query
{
    [UseFiltering]
    public Task<List<Customer>> GetCustomers([Service] ICustomerService customerService)
    {
        return customerService.GetCustomersAndOrders();
    }

    [UseFiltering]
    public Task<List<Order>> GetOrders([Service] IOrderService orderService)
    {
        return orderService.GetOrders();
    }
}