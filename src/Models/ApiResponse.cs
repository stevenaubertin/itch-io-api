using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Generic wrapper for itch.io API responses
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// The response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// List of errors if any occurred
    /// </summary>
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Whether the request was successful
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Errors == null || Errors.Count == 0;

    /// <summary>
    /// Creates a successful response
    /// </summary>
    public static ApiResponse<T> Success(T data) => new() { Data = data };

    /// <summary>
    /// Creates an error response
    /// </summary>
    public static ApiResponse<T> Error(params string[] errors) => new() { Errors = errors.ToList() };
}

/// <summary>
/// Response wrapper for user profile endpoint
/// </summary>
public class UserResponse
{
    /// <summary>
    /// User profile data
    /// </summary>
    [JsonPropertyName("user")]
    public ItchUser? User { get; set; }
}

/// <summary>
/// Response wrapper for my-games endpoint
/// </summary>
public class MyGamesResponse
{
    /// <summary>
    /// List of games owned by the user
    /// </summary>
    [JsonPropertyName("games")]
    public List<ItchGame> Games { get; set; } = new();
}

/// <summary>
/// Response wrapper for game uploads endpoint
/// </summary>
public class UploadsResponse
{
    /// <summary>
    /// List of uploads for a game
    /// </summary>
    [JsonPropertyName("uploads")]
    public List<Upload> Uploads { get; set; } = new();
}

/// <summary>
/// Response wrapper for download keys endpoint
/// </summary>
public class DownloadKeysResponse
{
    /// <summary>
    /// Download key information
    /// </summary>
    [JsonPropertyName("download_key")]
    public DownloadKey? DownloadKey { get; set; }

    /// <summary>
    /// List of download keys (when multiple are returned)
    /// </summary>
    [JsonPropertyName("download_keys")]
    public List<DownloadKey>? DownloadKeys { get; set; }
}

/// <summary>
/// Response wrapper for purchases endpoint
/// </summary>
public class PurchasesResponse
{
    /// <summary>
    /// List of purchases
    /// </summary>
    [JsonPropertyName("purchases")]
    public List<Purchase> Purchases { get; set; } = new();
}

/// <summary>
/// Response wrapper for search results
/// </summary>
public class SearchResponse
{
    /// <summary>
    /// List of games from search
    /// </summary>
    [JsonPropertyName("games")]
    public List<ItchGame>? Games { get; set; }

    /// <summary>
    /// List of users from search
    /// </summary>
    [JsonPropertyName("users")]
    public List<ItchUser>? Users { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Total number of results
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; set; }
}
