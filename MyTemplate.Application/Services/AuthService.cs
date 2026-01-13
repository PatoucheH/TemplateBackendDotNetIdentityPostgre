using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.Interfaces;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Application.Services;

/// <summary>
/// Service d'authentification.
/// Gère l'inscription, la connexion et la gestion des tokens.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        // Vérifier si l'email existe déjà
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return AuthResponseDto.FailureResponse("Un utilisateur avec cet email existe déjà");
        }

        // Créer le nouvel utilisateur
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName ?? registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Échec de création d'utilisateur pour {Email}: {Errors}", registerDto.Email, errors);
            return AuthResponseDto.FailureResponse($"Échec de l'inscription: {errors}");
        }

        // Ajouter le rôle par défaut (optionnel)
        // await _userManager.AddToRoleAsync(user, "User");

        _logger.LogInformation("Utilisateur créé avec succès: {UserId}", user.Id);

        // Générer le token
        var token = await _jwtService.GenerateTokenAsync(user);
        var expiration = _jwtService.GetTokenExpiration();
        var refreshToken = _jwtService.GenerateRefreshToken();

        var userDto = MapToUserDto(user, []);

        return AuthResponseDto.SuccessResponse(token, expiration, userDto, refreshToken);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        // Trouver l'utilisateur par email ou nom d'utilisateur
        var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(loginDto.EmailOrUserName);

        if (user == null)
        {
            return AuthResponseDto.FailureResponse("Identifiants invalides");
        }

        // Vérifier si l'utilisateur est actif
        if (!user.IsActive)
        {
            return AuthResponseDto.FailureResponse("Ce compte a été désactivé");
        }

        // Vérifier le mot de passe
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Compte verrouillé pour l'utilisateur: {UserId}", user.Id);
            return AuthResponseDto.FailureResponse("Compte verrouillé. Veuillez réessayer plus tard.");
        }

        if (!result.Succeeded)
        {
            return AuthResponseDto.FailureResponse("Identifiants invalides");
        }

        _logger.LogInformation("Connexion réussie pour l'utilisateur: {UserId}", user.Id);

        // Générer le token
        var token = await _jwtService.GenerateTokenAsync(user);
        var expiration = _jwtService.GetTokenExpiration(loginDto.RememberMe);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = MapToUserDto(user, roles.ToList());

        return AuthResponseDto.SuccessResponse(token, expiration, userDto, refreshToken);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // PERSONNALISATION : Implémentez la logique de rafraîchissement du token
        // Vous devrez stocker les refresh tokens dans la base de données
        // et valider le token reçu contre celui stocké

        await Task.CompletedTask;
        return AuthResponseDto.FailureResponse("Refresh token non implémenté. Voir les commentaires dans le code.");
    }

    public async Task<bool> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default)
    {
        // PERSONNALISATION : Implémentez la révocation du token
        // Supprimez ou invalidez le refresh token dans la base de données

        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        await Task.CompletedTask;
        return _jwtService.ValidateToken(token);
    }

    // ============================================================
    // MÉTHODES PRIVÉES
    // ============================================================

    private static UserDto MapToUserDto(ApplicationUser user, List<string> roles)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            Roles = roles
        };
    }
}
