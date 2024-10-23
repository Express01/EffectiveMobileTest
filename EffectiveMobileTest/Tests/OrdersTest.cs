using EffectiveMobileTest.Contracts;
using EffectiveMobileTest.Controllers;
using EffectiveMobileTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class OrdersTest
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly Mock<ILogger<OrdersController>> _mockLogger;
    private readonly OrdersController _controller;

    public OrdersTest()
    {
        _mockOrderService = new Mock<IOrderService>();
        _mockLogger = new Mock<ILogger<OrdersController>>();
        _controller = new OrdersController(_mockOrderService.Object, _mockLogger.Object);
    }
    [Fact]
    public async Task FilterOrders_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var district = "Downtown";
        var firstDeliveryDateTime = new DateTime(2024, 10, 22, 14, 0, 0);
        var expectedOrders = new List<OrderResponse>
        {
            new OrderResponse { OrderId = Guid.NewGuid(), Weight = 5.0m, District = district, DeliveryTime = firstDeliveryDateTime }
        };

        _mockOrderService.Setup(s => s.FilterOrdersAsync(district, firstDeliveryDateTime))
                         .ReturnsAsync(expectedOrders);

        // Act
        var result = await _controller.FilterOrders(district, firstDeliveryDateTime);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<OrderResponse>>(okResult.Value);
        Assert.Equal(expectedOrders.Count, returnValue.Count);
    }

    [Fact]
    public async Task FilterOrders_EmptyDistrict_ReturnsBadRequest()
    {
        // Arrange
        string district = "";
        var firstDeliveryDateTime = new DateTime(2024, 10, 22, 14, 0, 0);

        // Act
        var result = await _controller.FilterOrders(district, firstDeliveryDateTime);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("District cannot be null or empty.", badRequestResult.Value);
    }

    [Fact]
    public async Task FilterOrders_DefaultDeliveryTime_ReturnsBadRequest()
    {
        // Arrange
        var district = "Downtown";
        var firstDeliveryDateTime = default(DateTime);

        // Act
        var result = await _controller.FilterOrders(district, firstDeliveryDateTime);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid delivery date time.", badRequestResult.Value);
    }

    [Fact]
    public async Task FilterOrders_NoOrdersFound_ReturnsNotFound()
    {
        // Arrange
        var district = "Downtown";
        var firstDeliveryDateTime = new DateTime(2024, 10, 22, 14, 0, 0);

        _mockOrderService.Setup(s => s.FilterOrdersAsync(district, firstDeliveryDateTime))
            .ReturnsAsync(new List<OrderResponse>()); // Пустой список заказов

        // Act
        var result = await _controller.FilterOrders(district, firstDeliveryDateTime);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("No orders found.", notFoundResult.Value);
    }

    [Fact]
    public async Task FilterOrders_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var district = "Downtown";
        var firstDeliveryDateTime = new DateTime(2024, 10, 22, 14, 0, 0);

        _mockOrderService.Setup(s => s.FilterOrdersAsync(district, firstDeliveryDateTime))
                         .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _controller.FilterOrders(district, firstDeliveryDateTime);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, internalServerErrorResult.StatusCode);
        Assert.Equal("An internal server error occurred.", internalServerErrorResult.Value);
    }
}