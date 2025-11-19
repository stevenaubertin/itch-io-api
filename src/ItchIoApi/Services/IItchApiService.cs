using ItchIoApi.Models;

namespace ItchIoApi.Services;

/// <summary>
/// Interface for itch.io API service
/// </summary>
public interface IItchApiService
{
    /// <summary>
    /// Get credentials information
    /// </summary>
    Task<ApiResponse<CredentialsInfo>> GetCredentialsInfoAsync(string? apiKey = null);

    /// <summary>
    /// Get current user profile
    /// </summary>
    Task<ApiResponse<ItchUser>> GetMyProfileAsync(string? apiKey = null);

    /// <summary>
    /// Get current user's games
    /// </summary>
    Task<ApiResponse<List<ItchGame>>> GetMyGamesAsync(string? apiKey = null);

    /// <summary>
    /// Get uploads for a specific game
    /// </summary>
    Task<ApiResponse<List<Upload>>> GetGameUploadsAsync(int gameId, string? apiKey = null);

    /// <summary>
    /// Get download key information
    /// </summary>
    Task<ApiResponse<DownloadKey>> GetDownloadKeyAsync(int gameId, string? downloadKey = null, int? userId = null, string? email = null, string? apiKey = null);

    /// <summary>
    /// Get purchases for a game
    /// </summary>
    Task<ApiResponse<List<Purchase>>> GetGamePurchasesAsync(int gameId, int? userId = null, string? email = null, string? apiKey = null);

    /// <summary>
    /// Search for games
    /// </summary>
    Task<ApiResponse<SearchResponse>> SearchGamesAsync(string query, int page = 1, string? apiKey = null);

    /// <summary>
    /// Get game data by URL slug
    /// </summary>
    Task<ApiResponse<ItchGame>> GetGameDataAsync(string creator, string gameName);
}
