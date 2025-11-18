using ItchIoApi.Controllers;
using ItchIoApi.Models;
using ItchIoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ItchIoApi.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IItchApiService> _mockApiService;
    private readonly Mock<ILogger<UsersController>> _mockLogger;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockApiService = new Mock<IItchApiService>();
        _mockLogger = new Mock<ILogger<UsersController>>();
        _controller = new UsersController(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCredentialsInfo_WithValidApiKey_ReturnsOk()
    {
        // Arrange
        var expectedCredentials = new CredentialsInfo
        {
            Scopes = new List<string> { "profile:me" }
        };

        _mockApiService
            .Setup(s => s.GetCredentialsInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<CredentialsInfo>.Success(expectedCredentials));

        // Act
        var result = await _controller.GetCredentialsInfo("test-api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var credentials = Assert.IsType<CredentialsInfo>(okResult.Value);
        Assert.Single(credentials.Scopes);
    }

    [Fact]
    public async Task GetCredentialsInfo_WithInvalidApiKey_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetCredentialsInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<CredentialsInfo>.Error("Invalid API key"));

        // Act
        var result = await _controller.GetCredentialsInfo("invalid-key");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetMyProfile_WithValidApiKey_ReturnsOk()
    {
        // Arrange
        var expectedUser = new ItchUser
        {
            Id = 123,
            Username = "testuser",
            DisplayName = "Test User"
        };

        _mockApiService
            .Setup(s => s.GetMyProfileAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<ItchUser>.Success(expectedUser));

        // Act
        var result = await _controller.GetMyProfile("test-api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<ItchUser>(okResult.Value);
        Assert.Equal("testuser", user.Username);
    }

    [Fact]
    public async Task GetMyProfile_WithInvalidApiKey_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetMyProfileAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<ItchUser>.Error("Unauthorized"));

        // Act
        var result = await _controller.GetMyProfile("invalid-key");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetMyProfile_CallsServiceWithCorrectApiKey()
    {
        // Arrange
        var apiKey = "test-api-key-123";
        var expectedUser = new ItchUser
        {
            Id = 1,
            Username = "test"
        };

        _mockApiService
            .Setup(s => s.GetMyProfileAsync(apiKey))
            .ReturnsAsync(ApiResponse<ItchUser>.Success(expectedUser));

        // Act
        await _controller.GetMyProfile(apiKey);

        // Assert
        _mockApiService.Verify(s => s.GetMyProfileAsync(apiKey), Times.Once);
    }
}
