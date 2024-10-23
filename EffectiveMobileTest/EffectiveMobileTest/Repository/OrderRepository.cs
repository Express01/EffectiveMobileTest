using EffectiveMobileTest.Context;
using EffectiveMobileTest.Models;
using Microsoft.EntityFrameworkCore;

namespace EffectiveMobileTest.Repository;

public class OrderRepository:IOrderRepository
{
    private readonly DeliveryDbContext _context;
    private readonly ILogger<OrderRepository> _logger;
    public OrderRepository(DeliveryDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Guid> CreateOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        _context.SaveChangesAsync();
        _logger.LogInformation("Order created successfully");
        return order.OrderId;
    }

    public async Task<List<Order>> GetOrdersByDistrictAndTimeAsync(string district, DateTime startTime, DateTime endTime)
    {
        return await _context.Orders
            .Where(o => o.District == district && o.DeliveryTime >= startTime && o.DeliveryTime <= endTime)
            .ToListAsync();
    }
}

