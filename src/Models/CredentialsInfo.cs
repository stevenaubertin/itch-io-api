using System.Text.Json.Serialization;

namespace ItchIoApi.Models;

/// <summary>
/// Represents API credentials information
/// </summary>
public class CredentialsInfo
{
    /// <summary>
    /// List of scopes this credential has access to
    /// </summary>
    [JsonPropertyName("scopes")]
    public List<string> Scopes { get; set; } = new();

    /// <summary>
    /// Expiration timestamp (for JWT tokens)
    /// </summary>
    [JsonPropertyName("expires_at")]
    public string? ExpiresAt { get; set; }
}
