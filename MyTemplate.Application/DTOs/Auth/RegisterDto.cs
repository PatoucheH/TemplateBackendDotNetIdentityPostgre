using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO for registering a new user.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Email address (will also be used as default username)
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Username (optional, uses email if not provided)
    /// </summary>
    [MaxLength(256, ErrorMessage = "Username cannot exceed 256 characters")]
    public string? UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Password confirmation
    /// </summary>
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// User's first name (optional)
    /// </summary>
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name (optional)
    /// </summary>
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }
}
