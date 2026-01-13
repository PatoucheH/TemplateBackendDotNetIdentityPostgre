using System.Linq.Expressions;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Interface générique pour les opérations de repository.
/// Implémente le pattern Repository pour l'accès aux données.
/// </summary>
/// <typeparam name="T">Type d'entité qui hérite de BaseEntity</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    // ============================================================
    // LECTURE
    // ============================================================

    /// <summary>
    /// Récupère une entité par son identifiant
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère toutes les entités
    /// </summary>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère les entités correspondant à un prédicat
    /// </summary>
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Récupère la première entité correspondant à un prédicat
    /// </summary>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Vérifie si une entité existe selon un prédicat
    /// </summary>
    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Compte le nombre d'entités correspondant à un prédicat
    /// </summary>
    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    // ============================================================
    // ÉCRITURE
    // ============================================================

    /// <summary>
    /// Ajoute une nouvelle entité
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ajoute plusieurs entités
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Met à jour une entité existante
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Supprime une entité
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Supprime plusieurs entités
    /// </summary>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Soft delete - marque l'entité comme supprimée sans la supprimer physiquement
    /// </summary>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
