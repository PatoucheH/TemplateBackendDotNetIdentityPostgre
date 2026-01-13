namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for managing transactions.
/// Allows coordinating operations across multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // ============================================================
    // REPOSITORIES - Add your repositories here
    // ============================================================

    // /// <summary>
    // /// Products repository (example)
    // /// </summary>
    // IRepository<Product> Products { get; }

    // /// <summary>
    // /// Categories repository (example)
    // /// </summary>
    // IRepository<Category> Categories { get; }

    // /// <summary>
    // /// Orders repository (example)
    // /// </summary>
    // IRepository<Order> Orders { get; }

    // ============================================================
    // TRANSACTION METHODS
    // ============================================================

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a new transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
