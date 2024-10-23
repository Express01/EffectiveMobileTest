using EffectiveMobileTest.Contracts;
using EffectiveMobileTest.Models;
using EffectiveMobileTest.Repository;

namespace EffectiveMobileTest.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> CreateOrderAsync(OrderRequest orderRequest)
        {

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                Weight = orderRequest.Weight,
                District = orderRequest.District,
                DeliveryTime = orderRequest.DeliveryTime,

            };

            return await _orderRepository.CreateOrder(order);
        }

        public async Task<List<OrderResponse>> FilterOrdersAsync(string district, DateTime firstDeliveryDateTime)
        {
            // Рассчитываем временной интервал: 30 минут после первого заказа
            var halfHourLater = firstDeliveryDateTime.AddMinutes(30);
            var orders =
                await _orderRepository.GetOrdersByDistrictAndTimeAsync(district, firstDeliveryDateTime, halfHourLater);
            var orderResponses = orders.Select(order => new OrderResponse
            {
                OrderId = order.OrderId,
                Weight = order.Weight,
                District = order.District,
                DeliveryTime = order.DeliveryTime

            }).ToList();
            return orderResponses;
        }
    }
}
