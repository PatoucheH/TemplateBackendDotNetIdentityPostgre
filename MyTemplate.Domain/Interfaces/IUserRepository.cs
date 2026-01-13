using MyTemplate.Domain.Entities;

namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Interface spécifique pour les opérations sur les utilisateurs.
/// Complète les fonctionnalités d'Identity avec des méthodes personnalisées.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Récupère un utilisateur par son identifiant
    /// </summary>
    Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un utilisateur par son email
    /// </summary>
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère un utilisateur par son nom d'utilisateur
    /// </summary>
    Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère tous les utilisateurs actifs
    /// </summary>
    Task<IReadOnlyList<ApplicationUser>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Met à jour les informations d'un utilisateur
    /// </summary>
    Task UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Désactive un utilisateur (soft delete)
    /// </summary>
    Task DeactivateAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vérifie si un email est déjà utilisé
    /// </summary>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vérifie si un nom d'utilisateur est déjà utilisé
    /// </summary>
    Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
}
