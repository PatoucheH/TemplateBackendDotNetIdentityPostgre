namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO response after successful authentication.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Indicates if authentication was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// JWT access token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Refresh token (optional)
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Token expiration date
    /// </summary>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserDto? User { get; set; }

    // ============================================================
    // FACTORY METHODS
    // ============================================================

    public static AuthResponseDto SuccessResponse(string token, DateTime expiration, UserDto user, string? refreshToken = null)
    {
        return new AuthResponseDto
        {
            Success = true,
            Message = "Authentication successful",
            Token = token,
            RefreshToken = refreshToken,
            Expiration = expiration,
            User = user
        };
    }

    public static AuthResponseDto FailureResponse(string message)
    {
        return new AuthResponseDto
        {
            Success = false,
            Message = message
        };
    }
}
