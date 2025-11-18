using ItchIoApi.Controllers;
using ItchIoApi.Models;
using ItchIoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ItchIoApi.Tests.Controllers;

public class GamesControllerTests
{
    private readonly Mock<IItchApiService> _mockApiService;
    private readonly Mock<ILogger<GamesController>> _mockLogger;
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _mockApiService = new Mock<IItchApiService>();
        _mockLogger = new Mock<ILogger<GamesController>>();
        _controller = new GamesController(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetMyGames_WithValidApiKey_ReturnsOk()
    {
        // Arrange
        var expectedGames = new List<ItchGame>
        {
            new ItchGame { Id = 1, Title = "Game 1" },
            new ItchGame { Id = 2, Title = "Game 2" }
        };

        _mockApiService
            .Setup(s => s.GetMyGamesAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<ItchGame>>.Success(expectedGames));

        // Act
        var result = await _controller.GetMyGames("test-api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var games = Assert.IsAssignableFrom<IEnumerable<ItchGame>>(okResult.Value);
        Assert.Equal(2, games.Count());
    }

    [Fact]
    public async Task GetMyGames_WithInvalidApiKey_ReturnsBadRequest()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetMyGamesAsync(It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<ItchGame>>.Error("API key is required"));

        // Act
        var result = await _controller.GetMyGames(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetGameUploads_WithValidGameId_ReturnsOk()
    {
        // Arrange
        var expectedUploads = new List<Upload>
        {
            new Upload { Id = 1, Filename = "game.zip", Size = 1024 }
        };

        _mockApiService
            .Setup(s => s.GetGameUploadsAsync(123, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<List<Upload>>.Success(expectedUploads));

        // Act
        var result = await _controller.GetGameUploads(123, "test-api-key");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var uploads = Assert.IsAssignableFrom<IEnumerable<Upload>>(okResult.Value);
        Assert.Single(uploads);
    }

    [Fact]
    public async Task GetGameBySlug_WithValidSlug_ReturnsOk()
    {
        // Arrange
        var expectedGame = new ItchGame
        {
            Id = 1,
            Title = "Test Game"
        };

        _mockApiService
            .Setup(s => s.GetGameDataAsync("creator", "game-name"))
            .ReturnsAsync(ApiResponse<ItchGame>.Success(expectedGame));

        // Act
        var result = await _controller.GetGameBySlug("creator", "game-name");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var game = Assert.IsType<ItchGame>(okResult.Value);
        Assert.Equal("Test Game", game.Title);
    }

    [Fact]
    public async Task GetGameBySlug_WithInvalidSlug_ReturnsNotFound()
    {
        // Arrange
        _mockApiService
            .Setup(s => s.GetGameDataAsync("creator", "invalid-game"))
            .ReturnsAsync(ApiResponse<ItchGame>.Error("Game not found"));

        // Act
        var result = await _controller.GetGameBySlug("creator", "invalid-game");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task SearchGames_WithValidQuery_ReturnsOk()
    {
        // Arrange
        var expectedResponse = new SearchResponse
        {
            Games = new List<ItchGame>
            {
                new ItchGame { Id = 1, Title = "Pixel Game" }
            },
            Page = 1
        };

        _mockApiService
            .Setup(s => s.SearchGamesAsync("pixel", 1, It.IsAny<string>()))
            .ReturnsAsync(ApiResponse<SearchResponse>.Success(expectedResponse));

        // Act
        var result = await _controller.SearchGames("pixel", 1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var searchResponse = Assert.IsType<SearchResponse>(okResult.Value);
        Assert.NotNull(searchResponse.Games);
        Assert.Single(searchResponse.Games);
    }

    [Fact]
    public async Task SearchGames_WithEmptyQuery_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.SearchGames("", 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task SearchGames_WithNullQuery_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.SearchGames(null!, 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
