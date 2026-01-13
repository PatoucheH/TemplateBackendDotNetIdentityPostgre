using MyTemplate.Domain.Entities;

namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Specific interface for user operations.
/// Complements Identity features with custom methods.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their identifier
    /// </summary>
    Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email
    /// </summary>
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their username
    /// </summary>
    Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active users
    /// </summary>
    Task<IReadOnlyList<ApplicationUser>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user's information
    /// </summary>
    Task UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a user (soft delete)
    /// </summary>
    Task DeactivateAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email is already in use
    /// </summary>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a username is already in use
    /// </summary>
    Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
}
