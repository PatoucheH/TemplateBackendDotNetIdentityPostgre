using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Domain.Entities;

/// <summary>
/// Custom application user.
/// Inherits from IdentityUser to benefit from all Identity properties.
///
/// CUSTOMIZATION: Add your custom properties here.
/// </summary>
public class ApplicationUser : IdentityUser
{
    // ============================================================
    // CUSTOM PROPERTIES - Add yours below
    // ============================================================

    /// <summary>
    /// User's first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Account creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    // ============================================================
    // RELATIONSHIPS - Uncomment and adapt as needed
    // ============================================================

    // /// <summary>
    // /// User's orders collection (example)
    // /// </summary>
    // public virtual ICollection<Order>? Orders { get; set; }

    // /// <summary>
    // /// User's profile (example 1-1 relationship)
    // /// </summary>
    // public virtual UserProfile? Profile { get; set; }
}
