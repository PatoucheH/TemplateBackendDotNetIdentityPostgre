using MyTemplate.Application.DTOs.Auth;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface for authentication service.
/// Supports JWT (for SPA/Mobile) and Cookies (for Blazor).
/// </summary>
public interface IAuthService
{
    // ============================================================
    // [JWT] - JWT METHODS (for Angular, React, Vue, Mobile)
    // Delete this section if you only use Blazor
    // ============================================================

    /// <summary>
    /// Registers a new user and returns a JWT token
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs in a user and returns a JWT token
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the JWT token
    /// </summary>
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the refresh token
    /// </summary>
    Task<bool> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    Task<bool> ValidateTokenAsync(string token);

    // ============================================================
    // [COOKIES] - COOKIE METHODS (for Blazor)
    // Delete this section if you only use JWT
    // ============================================================

    /// <summary>
    /// Logs in a user via cookie (for Blazor)
    /// </summary>
    Task<AuthResponseDto> LoginWithCookieAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out the user by removing the cookie (for Blazor)
    /// </summary>
    Task LogoutWithCookieAsync(CancellationToken cancellationToken = default);
}
