using System.Net;
using System.Text.Json;
using ItchIoApi.Models;
using ItchIoApi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ItchIoApi.Tests.Services;

public class ItchApiServiceTests
{
    private readonly Mock<ILogger<ItchApiService>> _mockLogger;
    private readonly ItchApiSettings _settings;
    private readonly IOptions<ItchApiSettings> _options;

    public ItchApiServiceTests()
    {
        _mockLogger = new Mock<ILogger<ItchApiService>>();
        _settings = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = "test-api-key",
            TimeoutSeconds = 30,
            UseKeyInPath = false
        };
        _options = Options.Create(_settings);
    }

    [Fact]
    public async Task GetCredentialsInfoAsync_WithValidApiKey_ReturnsSuccess()
    {
        // Arrange
        var expectedResponse = new CredentialsInfo
        {
            Scopes = new List<string> { "profile:me", "profile:games" }
        };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetCredentialsInfoAsync("test-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Scopes.Count);
    }

    [Fact]
    public async Task GetCredentialsInfoAsync_WithoutApiKey_ReturnsError()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "{}");
        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var settingsWithoutKey = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = null
        };
        var options = Options.Create(settingsWithoutKey);

        var service = new ItchApiService(httpClient, options, _mockLogger.Object);

        // Act
        var result = await service.GetCredentialsInfoAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("API key is required", result.Errors ?? new List<string>());
    }

    [Fact]
    public async Task GetMyProfileAsync_WithValidApiKey_ReturnsUser()
    {
        // Arrange
        var expectedUser = new ItchUser
        {
            Id = 123,
            Username = "testuser",
            DisplayName = "Test User",
            Url = "https://testuser.itch.io"
        };

        var userResponse = new UserResponse { User = expectedUser };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(userResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetMyProfileAsync("test-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("testuser", result.Data.Username);
        Assert.Equal(123, result.Data.Id);
    }

    [Fact]
    public async Task GetMyGamesAsync_WithValidApiKey_ReturnsGames()
    {
        // Arrange
        var expectedGames = new List<ItchGame>
        {
            new ItchGame { Id = 1, Title = "Game 1" },
            new ItchGame { Id = 2, Title = "Game 2" }
        };

        var gamesResponse = new MyGamesResponse { Games = expectedGames };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(gamesResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetMyGamesAsync("test-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Game 1", result.Data[0].Title);
    }

    [Fact]
    public async Task GetGameUploadsAsync_WithValidGameId_ReturnsUploads()
    {
        // Arrange
        var expectedUploads = new List<Upload>
        {
            new Upload { Id = 1, Filename = "game.zip", Size = 1024 },
            new Upload { Id = 2, Filename = "demo.zip", Size = 512 }
        };

        var uploadsResponse = new UploadsResponse { Uploads = expectedUploads };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(uploadsResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetGameUploadsAsync(123, "test-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("game.zip", result.Data[0].Filename);
    }

    [Fact]
    public async Task GetDownloadKeyAsync_WithValidParameters_ReturnsDownloadKey()
    {
        // Arrange
        var expectedKey = new DownloadKey
        {
            Id = 1,
            Key = "test-download-key",
            GameId = 123
        };

        var keyResponse = new DownloadKeysResponse { DownloadKey = expectedKey };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(keyResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetDownloadKeyAsync(123, downloadKey: "test-key", apiKey: "test-api-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("test-download-key", result.Data.Key);
    }

    [Fact]
    public async Task GetGamePurchasesAsync_WithValidGameId_ReturnsPurchases()
    {
        // Arrange
        var expectedPurchases = new List<Purchase>
        {
            new Purchase { Id = 1, GameId = 123, Price = 999 },
            new Purchase { Id = 2, GameId = 123, Price = 1999 }
        };

        var purchasesResponse = new PurchasesResponse { Purchases = expectedPurchases };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(purchasesResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetGamePurchasesAsync(123, apiKey: "test-key");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task SearchGamesAsync_WithValidQuery_ReturnsResults()
    {
        // Arrange
        var expectedResponse = new SearchResponse
        {
            Games = new List<ItchGame>
            {
                new ItchGame { Id = 1, Title = "Pixel Art Game" }
            },
            Page = 1
        };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedResponse));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.SearchGamesAsync("pixel", page: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Games);
        Assert.Single(result.Data.Games);
    }

    [Fact]
    public async Task GetGameDataAsync_WithValidSlug_ReturnsGame()
    {
        // Arrange
        var expectedGame = new ItchGame
        {
            Id = 1,
            Title = "Test Game"
        };

        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedGame));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetGameDataAsync("creator", "game-name");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Test Game", result.Data.Title);
    }

    [Fact]
    public async Task GetMyProfileAsync_WithHttpError_ReturnsErrorResponse()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.Unauthorized,
            JsonSerializer.Serialize(new { errors = new[] { "Unauthorized" } }));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetMyProfileAsync("invalid-key");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task GetMyGamesAsync_WithoutApiKey_ReturnsError()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "{}");
        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var settingsWithoutKey = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = null
        };
        var options = Options.Create(settingsWithoutKey);

        var service = new ItchApiService(httpClient, options, _mockLogger.Object);

        // Act
        var result = await service.GetMyGamesAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("API key is required", result.Errors ?? new List<string>());
    }

    [Fact]
    public async Task GetGameUploadsAsync_WithoutApiKey_ReturnsError()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "{}");
        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var settingsWithoutKey = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = null
        };
        var options = Options.Create(settingsWithoutKey);

        var service = new ItchApiService(httpClient, options, _mockLogger.Object);

        // Act
        var result = await service.GetGameUploadsAsync(123);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("API key is required", result.Errors ?? new List<string>());
    }

    [Fact]
    public async Task GetDownloadKeyAsync_WithoutApiKey_ReturnsError()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "{}");
        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var settingsWithoutKey = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = null
        };
        var options = Options.Create(settingsWithoutKey);

        var service = new ItchApiService(httpClient, options, _mockLogger.Object);

        // Act
        var result = await service.GetDownloadKeyAsync(123, downloadKey: "test-key");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("API key is required", result.Errors ?? new List<string>());
    }

    [Fact]
    public async Task GetGamePurchasesAsync_WithoutApiKey_ReturnsError()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "{}");
        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var settingsWithoutKey = new ItchApiSettings
        {
            BaseUrl = "https://itch.io",
            ApiKey = null
        };
        var options = Options.Create(settingsWithoutKey);

        var service = new ItchApiService(httpClient, options, _mockLogger.Object);

        // Act
        var result = await service.GetGamePurchasesAsync(123);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("API key is required", result.Errors ?? new List<string>());
    }

    [Fact]
    public async Task GetGameDataAsync_WithHttpError_ReturnsErrorResponse()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.NotFound,
            JsonSerializer.Serialize(new { errors = new[] { "Game not found" } }));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.GetGameDataAsync("creator", "nonexistent-game");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task SearchGamesAsync_WithHttpError_ReturnsErrorResponse()
    {
        // Arrange
        var httpMessageHandler = CreateMockHttpMessageHandler(
            HttpStatusCode.BadRequest,
            JsonSerializer.Serialize(new { errors = new[] { "Invalid search query" } }));

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };

        var service = new ItchApiService(httpClient, _options, _mockLogger.Object);

        // Act
        var result = await service.SearchGamesAsync("", page: 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
    }

    private Mock<HttpMessageHandler> CreateMockHttpMessageHandler(
        HttpStatusCode statusCode,
        string responseContent)
    {
        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseContent)
            });

        return mockHandler;
    }
}
