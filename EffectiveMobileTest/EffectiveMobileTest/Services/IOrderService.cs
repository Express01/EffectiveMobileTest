using EffectiveMobileTest.Contracts;

namespace EffectiveMobileTest.Services;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(OrderRequest orderRequest);
    Task <List<OrderResponse>> FilterOrdersAsync(string district, DateTime firstDeliveryDateTime);
}