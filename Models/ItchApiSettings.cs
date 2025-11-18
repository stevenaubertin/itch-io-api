namespace ItchIoApi.Models;

/// <summary>
/// Configuration settings for itch.io API integration
/// </summary>
public class ItchApiSettings
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "ItchApi";

    /// <summary>
    /// Base URL for itch.io API (default: https://itch.io)
    /// </summary>
    public string BaseUrl { get; set; } = "https://itch.io";

    /// <summary>
    /// API key for authentication (optional - can be provided per-request)
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Whether to use API key in URL path vs Authorization header
    /// </summary>
    public bool UseKeyInPath { get; set; } = false;
}
