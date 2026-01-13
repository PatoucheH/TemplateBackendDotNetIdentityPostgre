using MyTemplate.Application.DTOs.Auth;

namespace MyTemplate.Application.Interfaces;

/// <summary>
/// Interface pour le service utilisateur.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Récupère un utilisateur par son identifiant
    /// </summary>
    Task<UserDto?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un utilisateur par son email
    /// </summary>
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les utilisateurs actifs
    /// </summary>
    Task<IReadOnlyList<UserDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Met à jour les informations d'un utilisateur
    /// </summary>
    Task<UserDto?> UpdateAsync(string userId, UpdateUserDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Change le mot de passe d'un utilisateur
    /// </summary>
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Désactive un utilisateur
    /// </summary>
    Task<bool> DeactivateAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ajoute un rôle à un utilisateur
    /// </summary>
    Task<bool> AddToRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retire un rôle d'un utilisateur
    /// </summary>
    Task<bool> RemoveFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère les rôles d'un utilisateur
    /// </summary>
    Task<IList<string>> GetRolesAsync(string userId, CancellationToken cancellationToken = default);
}
