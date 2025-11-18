using ItchIoApi.Controllers;
using ItchIoApi.Models;
using ItchIoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ItchIoApi.Tests.Controllers;

public class DownloadKeysControllerTests
{
    private readonly Mock<IItchApiService> _mockApiService;
    private readonly Mock<ILogger<DownloadKeysController>> _mockLogger;
    private readonly DownloadKeysController _controller;

    public DownloadKeysControllerTests()
    {
        _mockApiService = new Mock<IItchApiService>();
        _mockLogger = new Mock<ILogger<DownloadKeysController>>();
        _controller = new DownloadKeysController(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetDownloadKey_WithValidKey_ReturnsOk()
    {
        // Arrange
        var expectedKey = new DownloadKey
        {
            Id = 1,
            Key = "test-key",
            GameId = 123
        };

        _mockApiService
            .Setup(s => s.GetDownloadKeyAsync(123, "test-key", null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<DownloadKey>.Success(expectedKey));

        // Act
        var result = await _controller.GetDownloadKey(123, "test-key", null, null, "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var key = Assert.IsType<DownloadKey>(okResult.Value);
        Assert.Equal("test-key", key.Key);
    }

    [Fact]
    public async Task GetDownloadKey_WithUserId_ReturnsOk()
    {
        // Arrange
        var expectedKey = new DownloadKey
        {
            Id = 1,
            GameId = 123,
            OwnerId = 456
        };

        _mockApiService
            .Setup(s => s.GetDownloadKeyAsync(123, null, 456, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<DownloadKey>.Success(expectedKey));

        // Act
        var result = await _controller.GetDownloadKey(123, null, 456, null, "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var key = Assert.IsType<DownloadKey>(okResult.Value);
        Assert.Equal(456, key.OwnerId);
    }

    [Fact]
    public async Task GetDownloadKey_WithEmail_ReturnsOk()
    {
        // Arrange
        var expectedKey = new DownloadKey
        {
            Id = 1,
            GameId = 123,
            Email = "test@example.com"
        };

        _mockApiService
            .Setup(s => s.GetDownloadKeyAsync(123, null, null, "test@example.com", It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<DownloadKey>.Success(expectedKey));

        // Act
        var result = await _controller.GetDownloadKey(123, null, null, "test@example.com", "api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var key = Assert.IsType<DownloadKey>(okResult.Value);
        Assert.Equal("test@example.com", key.Email);
    }

    [Fact]
    public async Task GetDownloadKey_WithNoParameters_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetDownloadKey(123, null, null, null, "api-key");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDownloadKey_WithInvalidKey_ReturnsNotFound()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetDownloadKeyAsync(123, "invalid", null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<DownloadKey>.Error("Download key not found"));

        // Act
        var result = await _controller.GetDownloadKey(123, "invalid", null, null, "api-key");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDownloadKey_WithApiError_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetDownloadKeyAsync(123, "test-key", null, null, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<DownloadKey>.Error("API error"));

        // Act
        var result = await _controller.GetDownloadKey(123, "test-key", null, null, "api-key");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
