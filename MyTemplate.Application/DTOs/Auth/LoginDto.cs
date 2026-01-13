using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Email or username
    /// </summary>
    [Required(ErrorMessage = "Email or username is required")]
    public string EmailOrUserName { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Remember me (extends token duration)
    /// </summary>
    public bool RememberMe { get; set; } = false;
}
