namespace EffectiveMobileTest.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public decimal Weight { get; set; }
    public required string District { get; set; }
    public DateTime DeliveryTime { get; set; }
}
    