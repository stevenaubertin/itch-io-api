using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents an itch.io user profile
/// </summary>
public class ItchUser
{
    /// <summary>
    /// Unique user ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Username (URL-friendly)
    /// </summary>
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// URL to user's profile
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Cover image URL
    /// </summary>
    [JsonPropertyName("cover_url")]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// Whether the user is a developer/seller
    /// </summary>
    [JsonPropertyName("developer")]
    public bool IsDeveloper { get; set; }

    /// <summary>
    /// Whether the user account is from itch.io
    /// </summary>
    [JsonPropertyName("gamer")]
    public bool IsGamer { get; set; }

    /// <summary>
    /// Whether the user is a press account
    /// </summary>
    [JsonPropertyName("press_user")]
    public bool IsPressUser { get; set; }
}
