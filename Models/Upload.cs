using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents a file upload for a game
/// </summary>
public class Upload
{
    /// <summary>
    /// Unique upload ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Filename
    /// </summary>
    [JsonPropertyName("filename")]
    public required string Filename { get; set; }

    /// <summary>
    /// Display name (optional override)
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>
    /// Upload type (default, flash, unity, java, html, soundtrack, book, video, documentation, mod, audio_assets, graphical_assets, sourcecode)
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Unix timestamp of creation
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// Unix timestamp of last update
    /// </summary>
    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    /// <summary>
    /// Platform support traits
    /// </summary>
    [JsonPropertyName("traits")]
    public UploadTraits? Traits { get; set; }

    /// <summary>
    /// Whether this is a demo file
    /// </summary>
    [JsonPropertyName("demo")]
    public bool IsDemo { get; set; }

    /// <summary>
    /// Position in upload list
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }
}

/// <summary>
/// Platform-specific traits for an upload
/// </summary>
public class UploadTraits
{
    /// <summary>
    /// Windows compatibility
    /// </summary>
    [JsonPropertyName("p_windows")]
    public bool Windows { get; set; }

    /// <summary>
    /// Linux compatibility
    /// </summary>
    [JsonPropertyName("p_linux")]
    public bool Linux { get; set; }

    /// <summary>
    /// macOS compatibility
    /// </summary>
    [JsonPropertyName("p_osx")]
    public bool MacOS { get; set; }

    /// <summary>
    /// Android compatibility
    /// </summary>
    [JsonPropertyName("p_android")]
    public bool Android { get; set; }
}
