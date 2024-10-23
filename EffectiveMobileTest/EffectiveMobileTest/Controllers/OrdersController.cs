using EffectiveMobileTest.Contracts;
using EffectiveMobileTest.Repository;
using EffectiveMobileTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobileTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order data received");
                return BadRequest(ModelState);
            }

            try
            {
                var orderId = await _orderService.CreateOrderAsync(orderRequest);
                return Ok(orderId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while create orders.");
                return StatusCode(500, "An internal server error occurred.");
            }

        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<OrderResponse>>> FilterOrders(string district, DateTime firstDeliveryDateTime)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                _logger.LogWarning("District cannot be null or empty.");
                return BadRequest("District cannot be null or empty.");
            }

            if (firstDeliveryDateTime == default)
            {
                _logger.LogWarning("Invalid delivery date time received.");
                return BadRequest("Invalid delivery date time.");
            }

            try
            {
                var filteredOrders = await _orderService.FilterOrdersAsync(district, firstDeliveryDateTime);
                if (filteredOrders == null || !filteredOrders.Any())
                {
                    _logger.LogWarning("No orders found for the district: {district}", district);
                    return NotFound("No orders found.");
                }

                return Ok(filteredOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering orders.");
                return StatusCode(500, "An internal server error occurred.");
            };
        }



    }
}
