using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO for updating a user.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// First name
    /// </summary>
    [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO for password change.
/// </summary>
public class ChangePasswordDto
{
    /// <summary>
    /// Current password
    /// </summary>
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    [Required(ErrorMessage = "New password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password confirmation
    /// </summary>
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
