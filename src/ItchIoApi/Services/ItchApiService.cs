using System.Text.Json;
using ItchIoApi.Models;
using Microsoft.Extensions.Options;

namespace ItchIoApi.Services;

/// <summary>
/// Service for interacting with the itch.io API
/// </summary>
public class ItchApiService : IItchApiService
{
    private readonly HttpClient _httpClient;
    private readonly ItchApiSettings _settings;
    private readonly ILogger<ItchApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ItchApiService(
        HttpClient httpClient,
        IOptions<ItchApiSettings> settings,
        ILogger<ItchApiService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
    }

    public async Task<ApiResponse<CredentialsInfo>> GetCredentialsInfoAsync(string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<CredentialsInfo>.Error("API key is required");
        }

        var endpoint = BuildEndpoint(key, "credentials/info");
        return await GetAsync<CredentialsInfo>(endpoint, key);
    }

    public async Task<ApiResponse<ItchUser>> GetMyProfileAsync(string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<ItchUser>.Error("API key is required");
        }

        var endpoint = BuildEndpoint(key, "me");
        var response = await GetAsync<UserResponse>(endpoint, key);

        if (!response.IsSuccess || response.Data?.User == null)
        {
            return ApiResponse<ItchUser>.Error(response.Errors?.ToArray() ?? new[] { "Failed to retrieve user profile" });
        }

        return ApiResponse<ItchUser>.Success(response.Data.User);
    }

    public async Task<ApiResponse<List<ItchGame>>> GetMyGamesAsync(string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<List<ItchGame>>.Error("API key is required");
        }

        var endpoint = BuildEndpoint(key, "my-games");
        var response = await GetAsync<MyGamesResponse>(endpoint, key);

        if (!response.IsSuccess || response.Data == null)
        {
            return ApiResponse<List<ItchGame>>.Error(response.Errors?.ToArray() ?? new[] { "Failed to retrieve games" });
        }

        return ApiResponse<List<ItchGame>>.Success(response.Data.Games);
    }

    public async Task<ApiResponse<List<Upload>>> GetGameUploadsAsync(int gameId, string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<List<Upload>>.Error("API key is required");
        }

        var endpoint = BuildEndpoint(key, $"game/{gameId}/uploads");
        var response = await GetAsync<UploadsResponse>(endpoint, key);

        if (!response.IsSuccess || response.Data == null)
        {
            return ApiResponse<List<Upload>>.Error(response.Errors?.ToArray() ?? new[] { "Failed to retrieve uploads" });
        }

        return ApiResponse<List<Upload>>.Success(response.Data.Uploads);
    }

    public async Task<ApiResponse<DownloadKey>> GetDownloadKeyAsync(
        int gameId,
        string? downloadKey = null,
        int? userId = null,
        string? email = null,
        string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<DownloadKey>.Error("API key is required");
        }

        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(downloadKey))
            queryParams.Add($"download_key={Uri.EscapeDataString(downloadKey)}");
        if (userId.HasValue)
            queryParams.Add($"user_id={userId.Value}");
        if (!string.IsNullOrEmpty(email))
            queryParams.Add($"email={Uri.EscapeDataString(email)}");

        var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
        var endpoint = BuildEndpoint(key, $"game/{gameId}/download_keys{query}");

        var response = await GetAsync<DownloadKeysResponse>(endpoint, key);

        if (!response.IsSuccess || response.Data?.DownloadKey == null)
        {
            return ApiResponse<DownloadKey>.Error(response.Errors?.ToArray() ?? new[] { "Download key not found" });
        }

        return ApiResponse<DownloadKey>.Success(response.Data.DownloadKey);
    }

    public async Task<ApiResponse<List<Purchase>>> GetGamePurchasesAsync(
        int gameId,
        int? userId = null,
        string? email = null,
        string? apiKey = null)
    {
        var key = apiKey ?? _settings.ApiKey;
        if (string.IsNullOrEmpty(key))
        {
            return ApiResponse<List<Purchase>>.Error("API key is required");
        }

        var queryParams = new List<string>();
        if (userId.HasValue)
            queryParams.Add($"user_id={userId.Value}");
        if (!string.IsNullOrEmpty(email))
            queryParams.Add($"email={Uri.EscapeDataString(email)}");

        var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
        var endpoint = BuildEndpoint(key, $"game/{gameId}/purchases{query}");

        var response = await GetAsync<PurchasesResponse>(endpoint, key);

        if (!response.IsSuccess || response.Data == null)
        {
            return ApiResponse<List<Purchase>>.Error(response.Errors?.ToArray() ?? new[] { "Failed to retrieve purchases" });
        }

        return ApiResponse<List<Purchase>>.Success(response.Data.Purchases);
    }

    public async Task<ApiResponse<SearchResponse>> SearchGamesAsync(string query, int page = 1, string? apiKey = null)
    {
        var endpoint = $"/api/1/search/games?q={Uri.EscapeDataString(query)}&page={page}";

        // Search endpoint may work without authentication
        var response = await GetAsync<SearchResponse>(endpoint, apiKey);

        if (!response.IsSuccess || response.Data == null)
        {
            return ApiResponse<SearchResponse>.Error(response.Errors?.ToArray() ?? new[] { "Search failed" });
        }

        return ApiResponse<SearchResponse>.Success(response.Data);
    }

    public async Task<ApiResponse<ItchGame>> GetGameDataAsync(string creator, string gameName)
    {
        try
        {
            var endpoint = $"/{creator}/{gameName}/data.json";
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get game data for {Creator}/{GameName}: {StatusCode}",
                    creator, gameName, response.StatusCode);
                return ApiResponse<ItchGame>.Error($"Failed to retrieve game data: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var game = JsonSerializer.Deserialize<ItchGame>(content, _jsonOptions);

            if (game == null)
            {
                return ApiResponse<ItchGame>.Error("Failed to parse game data");
            }

            return ApiResponse<ItchGame>.Success(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting game data for {Creator}/{GameName}", creator, gameName);
            return ApiResponse<ItchGame>.Error($"Error: {ex.Message}");
        }
    }

    private string BuildEndpoint(string apiKey, string path)
    {
        if (_settings.UseKeyInPath)
        {
            return $"/api/1/{apiKey}/{path}";
        }
        else
        {
            return $"/api/1/key/{path}";
        }
    }

    private async Task<ApiResponse<T>> GetAsync<T>(string endpoint, string? apiKey = null) where T : class
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            // Add Authorization header if not using key in path
            if (!_settings.UseKeyInPath && !string.IsNullOrEmpty(apiKey))
            {
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
            }

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API request failed: {StatusCode} - {Content}", response.StatusCode, content);

                // Try to parse error response
                try
                {
                    var errorDoc = JsonDocument.Parse(content);
                    if (errorDoc.RootElement.TryGetProperty("errors", out var errorsElement))
                    {
                        var errors = errorsElement.EnumerateArray()
                            .Select(e => e.GetString() ?? "Unknown error")
                            .ToArray();
                        return ApiResponse<T>.Error(errors);
                    }
                }
                catch
                {
                    // If parsing fails, return generic error
                }

                return ApiResponse<T>.Error($"API request failed: {response.StatusCode}");
            }

            var data = JsonSerializer.Deserialize<T>(content, _jsonOptions);

            if (data == null)
            {
                return ApiResponse<T>.Error("Failed to parse response");
            }

            return ApiResponse<T>.Success(data);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error for endpoint: {Endpoint}", endpoint);
            return ApiResponse<T>.Error($"Network error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for endpoint: {Endpoint}", endpoint);
            return ApiResponse<T>.Error($"Failed to parse response: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error for endpoint: {Endpoint}", endpoint);
            return ApiResponse<T>.Error($"Unexpected error: {ex.Message}");
        }
    }
}
