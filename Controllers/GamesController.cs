using Microsoft.AspNetCore.Mvc;

namespace ItchIoApi.Controllers;

/// <summary>
/// Controller for managing itch.io games
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;

    public GamesController(ILogger<GamesController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get all games
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 20)</param>
    /// <returns>List of games</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Game>> GetGames([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting games - Page: {Page}, PageSize: {PageSize}", page, pageSize);
        
        // TODO: Implement actual itch.io API integration
        return Ok(new[] 
        { 
            new Game 
            { 
                Id = 1, 
                Title = "Sample Game", 
                Developer = "Sample Developer",
                Price = 9.99m,
                Description = "A sample game from itch.io",
                Tags = new[] { "indie", "adventure" }
            } 
        });
    }

    /// <summary>
    /// Get a specific game by ID
    /// </summary>
    /// <param name="id">Game ID</param>
    /// <returns>Game details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Game> GetGame(int id)
    {
        _logger.LogInformation("Getting game with ID: {Id}", id);
        
        // TODO: Implement actual itch.io API integration
        var game = new Game 
        { 
            Id = id, 
            Title = "Sample Game", 
            Developer = "Sample Developer",
            Price = 9.99m,
            Description = "A sample game from itch.io",
            Tags = new[] { "indie", "adventure" }
        };

        return Ok(game);
    }

    /// <summary>
    /// Search games by title or tags
    /// </summary>
    /// <param name="query">Search query</param>
    /// <returns>List of matching games</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Game>> SearchGames([FromQuery] string query)
    {
        _logger.LogInformation("Searching games with query: {Query}", query);
        
        // TODO: Implement actual itch.io API integration
        return Ok(new[] 
        { 
            new Game 
            { 
                Id = 1, 
                Title = $"Game matching '{query}'", 
                Developer = "Sample Developer",
                Price = 9.99m,
                Description = "A sample game from itch.io",
                Tags = new[] { "indie", query.ToLower() }
            } 
        });
    }
}

/// <summary>
/// Represents a game on itch.io
/// </summary>
public class Game
{
    /// <summary>
    /// Unique game identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Game title
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Game developer/creator
    /// </summary>
    public required string Developer { get; set; }

    /// <summary>
    /// Game description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Game price (null for free games)
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Game tags/categories
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Release date
    /// </summary>
    public DateTime? ReleaseDate { get; set; }
}
