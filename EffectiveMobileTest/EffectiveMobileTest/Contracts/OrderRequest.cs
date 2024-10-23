using System.ComponentModel.DataAnnotations;

namespace EffectiveMobileTest.Contracts;

public record OrderRequest()
{
    [Required]
    public decimal Weight { get; set; }
    [Required]
    public required string District { get; set; }
    [Required]
    public DateTime DeliveryTime { get; set; }
}