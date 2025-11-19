using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents a game purchase
/// </summary>
public class Purchase
{
    /// <summary>
    /// Unique purchase ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Game ID that was purchased
    /// </summary>
    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// User ID who made the purchase (if logged in)
    /// </summary>
    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    /// <summary>
    /// Email address used for purchase
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Price paid in cents
    /// </summary>
    [JsonPropertyName("price")]
    public int Price { get; set; }

    /// <summary>
    /// Currency code (e.g., USD)
    /// </summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>
    /// Formatted price string
    /// </summary>
    [JsonPropertyName("formatted_price")]
    public string? FormattedPrice { get; set; }

    /// <summary>
    /// Whether this was a gift purchase
    /// </summary>
    [JsonPropertyName("gift")]
    public bool IsGift { get; set; }

    /// <summary>
    /// Whether this purchase was refunded
    /// </summary>
    [JsonPropertyName("refunded")]
    public bool Refunded { get; set; }

    /// <summary>
    /// Unix timestamp of purchase
    /// </summary>
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    /// <summary>
    /// Source of the purchase (paypal, stripe, etc.)
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// Download key associated with this purchase
    /// </summary>
    [JsonPropertyName("download_key_id")]
    public int? DownloadKeyId { get; set; }

    /// <summary>
    /// Sale rate at time of purchase (percentage)
    /// </summary>
    [JsonPropertyName("sale_rate")]
    public int? SaleRate { get; set; }

    /// <summary>
    /// Game information
    /// </summary>
    [JsonPropertyName("game")]
    public ItchGame? Game { get; set; }
}
