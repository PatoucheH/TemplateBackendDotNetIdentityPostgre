using MyTemplate.Domain.Entities;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface pour le service JWT.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Génère un token JWT pour un utilisateur
    /// </summary>
    Task<string> GenerateTokenAsync(ApplicationUser user);

    /// <summary>
    /// Génère un token de rafraîchissement
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Récupère l'identifiant utilisateur depuis un token
    /// </summary>
    string? GetUserIdFromToken(string token);

    /// <summary>
    /// Récupère la date d'expiration du token
    /// </summary>
    DateTime GetTokenExpiration(bool rememberMe = false);

    /// <summary>
    /// Valide un token JWT
    /// </summary>
    bool ValidateToken(string token);
}
