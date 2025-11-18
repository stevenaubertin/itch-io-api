using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents a download key for a game
/// </summary>
public class DownloadKey
{
    /// <summary>
    /// Unique download key ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The actual download key string
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Game ID this key is for
    /// </summary>
    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// User ID who owns this key (if claimed)
    /// </summary>
    [JsonPropertyName("owner_id")]
    public int? OwnerId { get; set; }

    /// <summary>
    /// Email address if key was sent via email
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Unix timestamp of creation
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// Unix timestamp when key was claimed/used
    /// </summary>
    [JsonPropertyName("downloaded_at")]
    public string? DownloadedAt { get; set; }

    /// <summary>
    /// Game information
    /// </summary>
    [JsonPropertyName("game")]
    public ItchGame? Game { get; set; }

    /// <summary>
    /// Owner information
    /// </summary>
    [JsonPropertyName("owner")]
    public ItchUser? Owner { get; set; }
}
