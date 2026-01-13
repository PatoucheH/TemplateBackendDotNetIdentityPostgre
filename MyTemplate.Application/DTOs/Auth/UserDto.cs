namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO representing a user.
/// Used for API responses.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique user identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Full name (computed)
    /// </summary>
    public string? FullName => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)
        ? null
        : $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Email confirmed
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Account creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// User is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = [];
}
