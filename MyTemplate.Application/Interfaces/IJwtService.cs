using MyTemplate.Domain.Entities;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface for JWT service.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for a user
    /// </summary>
    Task<string> GenerateTokenAsync(ApplicationUser user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Gets the user identifier from a token
    /// </summary>
    string? GetUserIdFromToken(string token);

    /// <summary>
    /// Gets the token expiration date
    /// </summary>
    DateTime GetTokenExpiration(bool rememberMe = false);

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    bool ValidateToken(string token);
}
