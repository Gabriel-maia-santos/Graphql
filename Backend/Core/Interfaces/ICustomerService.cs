using Core.Entities;
using Core.Models;

namespace Core.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetCustomersAndOrders();

    Task<Customer> AddOrUpdateCustomerAsync(CustomerModel customerModel);

    Task<bool> DeleteCustomerAsync(int customerId);
}