using ItchIoApi.Controllers;
using ItchIoApi.Models;
using ItchIoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ItchIoApi.Tests.Controllers;

public class PurchasesControllerTests
{
    private readonly Mock<IItchApiService> _mockApiService;
    private readonly Mock<ILogger<PurchasesController>> _mockLogger;
    private readonly PurchasesController _controller;

    public PurchasesControllerTests()
    {
        _mockApiService = new Mock<IItchApiService>();
        _mockLogger = new Mock<ILogger<PurchasesController>>();
        _controller = new PurchasesController(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPurchases_WithValidGameId_ReturnsOk()
    {
        // Arrange
        var expectedPurchases = new List<Purchase>
        {
            new Purchase { Id = 1, GameId = 123, Price = 999 },
            new Purchase { Id = 2, GameId = 123, Price = 1999 }
        };

        _mockApiService
            .Setup(s => s.GetGamePurchasesAsync(123, null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Purchase>>.Success(expectedPurchases));

        // Act
        var result = await _controller.GetPurchases(123, null, null, "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var purchases = Assert.IsAssignableFrom<IEnumerable<Purchase>>(okResult.Value);
        Assert.Equal(2, purchases.Count());
    }

    [Fact]
    public async Task GetPurchases_WithUserId_ReturnsOk()
    {
        // Arrange
        var expectedPurchases = new List<Purchase>
        {
            new Purchase { Id = 1, GameId = 123, UserId = 456, Price = 999 }
        };

        _mockApiService
            .Setup(s => s.GetGamePurchasesAsync(123, 456, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Purchase>>.Success(expectedPurchases));

        // Act
        var result = await _controller.GetPurchases(123, 456, null, "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var purchases = Assert.IsAssignableFrom<IEnumerable<Purchase>>(okResult.Value);
        Assert.Single(purchases);
        Assert.Equal(456, purchases.First().UserId);
    }

    [Fact]
    public async Task GetPurchases_WithEmail_ReturnsOk()
    {
        // Arrange
        var expectedPurchases = new List<Purchase>
        {
            new Purchase { Id = 1, GameId = 123, Email = "test@example.com", Price = 999 }
        };

        _mockApiService
            .Setup(s => s.GetGamePurchasesAsync(123, null, "test@example.com", It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Purchase>>.Success(expectedPurchases));

        // Act
        var result = await _controller.GetPurchases(123, null, "test@example.com", "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var purchases = Assert.IsAssignableFrom<IEnumerable<Purchase>>(okResult.Value);
        Assert.Single(purchases);
        Assert.Equal("test@example.com", purchases.First().Email);
    }

    [Fact]
    public async Task GetPurchases_WithInvalidApiKey_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetGamePurchasesAsync(123, null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Purchase>>.Error("API key is required"));

        // Act
        var result = await _controller.GetPurchases(123, null, null, null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPurchases_WithEmptyResult_ReturnsEmptyList()
    {
        // Arrange
        var expectedPurchases = new List<Purchase>();

        _mockApiService
            .Setup(s => s.GetGamePurchasesAsync(123, null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Purchase>>.Success(expectedPurchases));

        // Act
        var result = await _controller.GetPurchases(123, null, null, "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var purchases = Assert.IsAssignableFrom<IEnumerable<Purchase>>(okResult.Value);
        Assert.Empty(purchases);
    }
}
