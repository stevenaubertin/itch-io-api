using Microsoft.AspNetCore.Mvc;
using ItchIoApi.Models;
using ItchIoApi.Services;

namespace ItchIoApi.Controllers;

/// <summary>
/// Controller for user-related endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IItchApiService _itchApiService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IItchApiService itchApiService, ILogger<UsersController> logger)
    {
        _itchApiService = itchApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get credentials information for the provided API key
    /// </summary>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>Credentials information including scopes and expiration</returns>
    /// <response code="200">Returns the credentials information</response>
    /// <response code="400">If the API key is missing or invalid</response>
    /// <response code="401">If authentication fails</response>
    [HttpGet("credentials")]
    [ProducesResponseType(typeof(CredentialsInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CredentialsInfo>> GetCredentialsInfo([FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        _logger.LogInformation("Getting credentials information");

        var response = await _itchApiService.GetCredentialsInfoAsync(apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get credentials: {Errors}", string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }

    /// <summary>
    /// Get the current user's profile information
    /// </summary>
    /// <param name="apiKey">itch.io API key (optional if configured in settings)</param>
    /// <returns>User profile including username, display name, and URLs</returns>
    /// <response code="200">Returns the user profile</response>
    /// <response code="400">If the API key is missing or invalid</response>
    /// <response code="401">If authentication fails</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ItchUser), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ItchUser>> GetMyProfile([FromHeader(Name = "X-API-Key")] string? apiKey = null)
    {
        _logger.LogInformation("Getting user profile");

        var response = await _itchApiService.GetMyProfileAsync(apiKey);

        if (!response.IsSuccess)
        {
            _logger.LogWarning("Failed to get user profile: {Errors}", string.Join(", ", response.Errors ?? new List<string>()));
            return BadRequest(new { errors = response.Errors });
        }

        return Ok(response.Data);
    }
}
