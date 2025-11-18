using Microsoft.AspNetCore.Mvc;
using ItchIoApi.Models;
using ItchIoApi.Services;

namespace ItchIoApi.Controllers;

/// <summary>
/// Controller for managing game purchases
/// </summary>
[ApiController]
[Route("api/games/{gameId}/purchases")]
public class PurchasesController : ControllerBase
{
    private readonly IItchApiService _itchApiService;
    private readonly ILogger<PurchasesController> _logger;

    public PurchasesController(IItchApiService itchApiService, ILogger<PurchasesController> logger)
    {
        _itchApiService = itchApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get purchases for a specific game
    /// </summary>
    /// <param name="gameId">Game ID</param>
    /// <param name="userId">User ID (optional)</param>
    /// <param name="email">Email address (optional)</param>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>List of purchases</returns>
    /// <remarks>
    /// Optionally filter by userId or email to get specific purchases
    /// </remarks>
    /// <response code="200">Returns the list of purchases</response>
    /// <response code="400">If the API key is missing or invalid</response>
    /// <response code="404">If the game is not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Purchase>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases(
        int gameId,
        [FromQuery] int? userId = null,
        [FromQuery] string? email = null,
        [FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        _logger.LogInformation("Getting purchases for game {GameId}", gameId);

        var response = await _itchApiService.GetGamePurchasesAsync(gameId, userId, email, apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get purchases for game {GameId}: {Errors}", gameId, string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }
}
