using EffectiveMobileTest.Models;

namespace EffectiveMobileTest.Repository;

public interface IOrderRepository
{
    Task<Guid> CreateOrder(Order order);
    Task<List<Order>> GetOrdersByDistrictAndTimeAsync(string district, DateTime firstDeliveryDateTime, DateTime halfHourLater);
}