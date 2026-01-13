using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.DTOs.Common;
using MyTemplate.Application.Interfaces;
using System.Security.Claims;

namespace MyTemplate.Api.Controllers;

/// <summary>
/// Controller pour l'authentification.
/// Gère l'inscription, la connexion et la gestion des tokens.
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
    /// Inscrit un nouvel utilisateur
    /// </summary>
    /// <param name="registerDto">Données d'inscription</param>
    /// <returns>Token JWT et informations utilisateur</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResponseDto.FailureResponse("Données invalides"));
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        _logger.LogInformation("Nouvel utilisateur inscrit: {Email}", registerDto.Email);
        return Ok(result);
    }

    /// <summary>
    /// Connecte un utilisateur existant
    /// </summary>
    /// <param name="loginDto">Données de connexion</param>
    /// <returns>Token JWT et informations utilisateur</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResponseDto.FailureResponse("Données invalides"));
        }

        var result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Rafraîchit le token d'accès
    /// </summary>
    /// <param name="refreshToken">Token de rafraîchissement</param>
    /// <returns>Nouveau token JWT</returns>
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
    /// Déconnecte l'utilisateur (révoque le token)
    /// </summary>
    /// <returns>Résultat de la déconnexion</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(ApiResponse.Failure("Utilisateur non identifié"));
        }

        await _authService.RevokeTokenAsync(userId);

        return Ok(ApiResponse.SuccessResponse("Déconnexion réussie"));
    }

    /// <summary>
    /// Récupère les informations de l'utilisateur connecté
    /// </summary>
    /// <returns>Informations de l'utilisateur</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<ApiResponse<UserDto>> GetCurrentUser()
    {
        // Extraire les informations du token JWT
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
}
