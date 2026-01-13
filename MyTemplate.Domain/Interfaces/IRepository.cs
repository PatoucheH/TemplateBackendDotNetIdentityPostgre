using System.Linq.Expressions;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Generic interface for repository operations.
/// Implements the Repository pattern for data access.
/// </summary>
/// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    // ============================================================
    // READ
    // ============================================================

    /// <summary>
    /// Gets an entity by its identifier
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities
    /// </summary>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entities matching a predicate
    /// </summary>
    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity matching a predicate
    /// </summary>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity exists based on a predicate
    /// </summary>
    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities matching a predicate
    /// </summary>
    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    // ============================================================
    // WRITE
    // ============================================================

    /// <summary>
    /// Adds a new entity
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple entities
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Deletes multiple entities
    /// </summary>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    /// Soft delete - marks the entity as deleted without physically removing it
    /// </summary>
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
