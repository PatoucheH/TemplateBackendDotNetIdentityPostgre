using MyTemplate.Application.DTOs.Auth;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface for user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by their identifier
    /// </summary>
    Task<UserDto?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email
    /// </summary>
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active users
    /// </summary>
    Task<IReadOnlyList<UserDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user's information
    /// </summary>
    Task<UserDto?> UpdateAsync(string userId, UpdateUserDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's password
    /// </summary>
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a user
    /// </summary>
    Task<bool> DeactivateAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a role to a user
    /// </summary>
    Task<bool> AddToRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a role from a user
    /// </summary>
    Task<bool> RemoveFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user's roles
    /// </summary>
    Task<IList<string>> GetRolesAsync(string userId, CancellationToken cancellationToken = default);
}
