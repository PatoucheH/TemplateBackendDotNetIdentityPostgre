namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO de réponse après authentification réussie.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Indique si l'authentification a réussi
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message de résultat
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Token JWT d'accès
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Token de rafraîchissement (optionnel)
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Date d'expiration du token
    /// </summary>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// Informations de l'utilisateur
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
            Message = "Authentification réussie",
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
