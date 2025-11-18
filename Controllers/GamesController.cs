using Microsoft.AspNetCore.Mvc;
using ItchIoApi.Models;
using ItchIoApi.Services;

namespace ItchIoApi.Controllers;

/// <summary>
/// Controller for managing itch.io games
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    private readonly IItchApiService _itchApiService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(IItchApiService itchApiService, ILogger<GamesController> logger)
    {
        _itchApiService = itchApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all games owned by the authenticated user
    /// </summary>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>List of user's games</returns>
    /// <response code="200">Returns the list of games</response>
    /// <response code="400">If the API key is missing or invalid</response>
    /// <response code="401">If authentication fails</response>
    [HttpGet("my-games")]
    [ProducesResponseType(typeof(IEnumerable<ItchGame>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ItchGame>>> GetMyGames([FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        _logger.LogInformation("Getting user's games");

        var response = await _itchApiService.GetMyGamesAsync(apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get games: {Errors}", string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }

    /// <summary>
    /// Get uploads for a specific game
    /// </summary>
    /// <param name="gameId">Game ID</param>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>List of game uploads</returns>
    /// <response code="200">Returns the list of uploads</response>
    /// <response code="400">If the API key is missing or invalid</response>
    /// <response code="404">If the game is not found</response>
    [HttpGet("{gameId}/uploads")]
    [ProducesResponseType(typeof(IEnumerable<Upload>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Upload>>> GetGameUploads(int gameId, [FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        _logger.LogInformation("Getting uploads for game {GameId}", gameId);

        var response = await _itchApiService.GetGameUploadsAsync(gameId, apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get uploads for game {GameId}: {Errors}", gameId, string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }

    /// <summary>
    /// Get game data by creator and game name
    /// </summary>
    /// <param name="creator">Creator username</param>
    /// <param name="gameName">Game name/slug</param>
    /// <returns>Game details</returns>
    /// <response code="200">Returns the game details</response>
    /// <response code="404">If the game is not found</response>
    [HttpGet("{creator}/{gameName}")]
    [ProducesResponseType(typeof(ItchGame), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItchGame>> GetGameBySlug(string creator, string gameName)
    {
        _logger.LogInformation("Getting game data for {Creator}/{GameName}", creator, gameName);

        var response = await _itchApiService.GetGameDataAsync(creator, gameName);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get game {Creator}/{GameName}: {Errors}", creator, gameName, string.Join(", ", response.Errors ?? new List<string>()));
            return NotFound(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }

    /// <summary>
    /// Search games by query
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="apiKey">itch.io API key (optional)</param>
    /// <returns>Search results</returns>
    /// <response code="200">Returns the search results</response>
    /// <response code="400">If the query is invalid</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SearchResponse>> SearchGames(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { errors = new[] { "Query parameter is required" } });
        }

        _logger.LogInformation("Searching games with query: {Query}, page: {Page}", query, page);

        var response = await _itchApiService.SearchGamesAsync(query, page, apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Search failed for query '{Query}': {Errors}", query, string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }
}
