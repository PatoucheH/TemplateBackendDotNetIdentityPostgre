using MyTemplate.Application.DTOs.Auth;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface pour le service d'authentification.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Inscrit un nouvel utilisateur
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Connecte un utilisateur existant
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rafraîchit le token d'accès
    /// </summary>
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Révoque le token de rafraîchissement
    /// </summary>
    Task<bool> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valide un token JWT
    /// </summary>
    Task<bool> ValidateTokenAsync(string token);
}
