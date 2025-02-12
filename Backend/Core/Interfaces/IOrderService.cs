using Core.Entities;
using Core.Models;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetOrders();

    Task<Order> AddOrUpdateOrderAsync(OrderModel orderModel);

    Task<bool> DeleteOrderAsync(int orderId);
}