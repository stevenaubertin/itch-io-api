using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents an itch.io game
/// </summary>
public class ItchGame
{
    /// <summary>
    /// Unique game ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Game title
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    /// <summary>
    /// URL-friendly slug
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Cover image URL
    /// </summary>
    [JsonPropertyName("cover_url")]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// Short description/tagline
    /// </summary>
    [JsonPropertyName("short_text")]
    public string? ShortText { get; set; }

    /// <summary>
    /// Game type (default, flash, unity, java, html)
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Classification (game, tool, assets, game_mod, physical_game, soundtrack, other, comic, book)
    /// </summary>
    [JsonPropertyName("classification")]
    public string? Classification { get; set; }

    /// <summary>
    /// Whether the game can be bought
    /// </summary>
    [JsonPropertyName("can_be_bought")]
    public bool CanBeBought { get; set; }

    /// <summary>
    /// Minimum price in cents (USD)
    /// </summary>
    [JsonPropertyName("min_price")]
    public int? MinPrice { get; set; }

    /// <summary>
    /// Whether the game has a demo
    /// </summary>
    [JsonPropertyName("has_demo")]
    public bool HasDemo { get; set; }

    /// <summary>
    /// Unix timestamp of creation
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// Unix timestamp of publication
    /// </summary>
    [JsonPropertyName("published_at")]
    public string? PublishedAt { get; set; }

    /// <summary>
    /// Number of downloads
    /// </summary>
    [JsonPropertyName("downloads_count")]
    public int DownloadsCount { get; set; }

    /// <summary>
    /// Number of purchases
    /// </summary>
    [JsonPropertyName("purchases_count")]
    public int PurchasesCount { get; set; }

    /// <summary>
    /// Number of views
    /// </summary>
    [JsonPropertyName("views_count")]
    public int ViewsCount { get; set; }

    /// <summary>
    /// Earnings information
    /// </summary>
    [JsonPropertyName("earnings")]
    public Earnings? Earnings { get; set; }

    /// <summary>
    /// Platform support (Windows, Linux, macOS, Android)
    /// </summary>
    [JsonPropertyName("traits")]
    public GameTraits? Traits { get; set; }

    /// <summary>
    /// Whether the game is published
    /// </summary>
    [JsonPropertyName("published")]
    public bool Published { get; set; }
}

/// <summary>
/// Represents game earnings
/// </summary>
public class Earnings
{
    /// <summary>
    /// Currency code (e.g., USD)
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Amount in cents
    /// </summary>
    [JsonPropertyName("amount_formatted")]
    public string? AmountFormatted { get; set; }
}

/// <summary>
/// Represents platform support for a game
/// </summary>
public class GameTraits
{
    /// <summary>
    /// Windows support
    /// </summary>
    [JsonPropertyName("p_windows")]
    public bool Windows { get; set; }

    /// <summary>
    /// Linux support
    /// </summary>
    [JsonPropertyName("p_linux")]
    public bool Linux { get; set; }

    /// <summary>
    /// macOS support
    /// </summary>
    [JsonPropertyName("p_osx")]
    public bool MacOS { get; set; }

    /// <summary>
    /// Android support
    /// </summary>
    [JsonPropertyName("p_android")]
    public bool Android { get; set; }
}
