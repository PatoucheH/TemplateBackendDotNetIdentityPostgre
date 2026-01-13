using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.DTOs.Common;
using MyTemplate.Application.Interfaces;
using System.Security.Claims;

namespace MyTemplate.Api.Controllers;

/// <summary>
/// Authentication controller.
/// Handles registration, login and token management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDto">Registration data</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResponseDto.FailureResponse("Invalid data"));
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        _logger.LogInformation("New user registered: {Email}", registerDto.Email);
        return Ok(result);
    }

    /// <summary>
    /// Logs in an existing user
    /// </summary>
    /// <param name="loginDto">Login data</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResponseDto.FailureResponse("Invalid data"));
        }

        var result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Refreshes the access token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>New JWT token</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Logs out the user (revokes the token)
    /// </summary>
    /// <returns>Logout result</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(ApiResponse.Failure("User not identified"));
        }

        await _authService.RevokeTokenAsync(userId);

        return Ok(ApiResponse.SuccessResponse("Logout successful"));
    }

    /// <summary>
    /// Gets the current user's information
    /// </summary>
    /// <returns>User information</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<ApiResponse<UserDto>> GetCurrentUser()
    {
        // Extract information from JWT token or cookie
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var firstName = User.FindFirstValue("firstName");
        var lastName = User.FindFirstValue("lastName");
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var userDto = new UserDto
        {
            Id = userId,
            Email = email ?? string.Empty,
            UserName = userName ?? string.Empty,
            FirstName = firstName,
            LastName = lastName,
            Roles = roles
        };

        return Ok(ApiResponse<UserDto>.SuccessResult(userDto));
    }

    // ============================================================
    // [COOKIES] - COOKIE ENDPOINTS (for Blazor)
    // Delete this section if you only use JWT
    // ============================================================

    /// <summary>
    /// Logs in a user via cookie (for Blazor)
    /// </summary>
    /// <param name="loginDto">Login data</param>
    /// <returns>User information</returns>
    [HttpPost("cookie/login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> LoginWithCookie([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResponseDto.FailureResponse("Invalid data"));
        }

        var result = await _authService.LoginWithCookieAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        _logger.LogInformation("Cookie login successful: {Email}", loginDto.EmailOrUserName);
        return Ok(result);
    }

    /// <summary>
    /// Logs out the user by removing the cookie (for Blazor)
    /// </summary>
    /// <returns>Logout result</returns>
    [HttpPost("cookie/logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> LogoutWithCookie()
    {
        await _authService.LogoutWithCookieAsync();
        return Ok(ApiResponse.SuccessResponse("Logout successful"));
    }
}
