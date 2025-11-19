using Microsoft.AspNetCore.Mvc;
using ItchIoApi.Models;
using ItchIoApi.Services;

namespace ItchIoApi.Controllers;

/// <summary>
/// Controller for managing download keys
/// </summary>
[ApiController]
[Route("api/games/{gameId}/download-keys")]
public class DownloadKeysController : ControllerBase
{
    private readonly IItchApiService _itchApiService;
    private readonly ILogger<DownloadKeysController> _logger;

    public DownloadKeysController(IItchApiService itchApiService, ILogger<DownloadKeysController> logger)
    {
        _itchApiService = itchApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get download key information for a game
    /// </summary>
    /// <param name="gameId">Game ID</param>
    /// <param name="downloadKey">Download key string (optional)</param>
    /// <param name="userId">User ID (optional)</param>
    /// <param name="email">Email address (optional)</param>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>Download key information</returns>
    /// <remarks>
    /// You must provide at least one of: downloadKey, userId, or email
    /// </remarks>
    /// <response code="200">Returns the download key information</response>
    /// <response code="400">If the API key is missing or parameters are invalid</response>
    /// <response code="404">If the download key is not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(DownloadKey), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DownloadKey>> GetDownloadKey(
        int gameId,
        [FromQuery] string? downloadKey = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? email = null,
        [FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        if (string.IsNullOrWhiteSpace(downloadKey) && !userId.HasValue && string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { errors = new[] { "Must provide at least one of: downloadKey, userId, or email" } });
        }

        _logger.LogInformation("Getting download key for game {GameId}", gameId);

        var response = await _itchApiService.GetDownloadKeyAsync(gameId, downloadKey, userId, email, apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get download key for game {GameId}: {Errors}", gameId, string.Join(", ", response.Errors ?? new List<string>()));

            if (response.Errors?.Any(e => e.Contains("not found")) == true)
            {
                return NotFound(new { errors = response.Errors });
            }

            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }
}
