namespace MyTemplate.Domain.Interfaces;

/// <summary>
/// Interface Unit of Work pour gérer les transactions.
/// Permet de coordonner les opérations sur plusieurs repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // ============================================================
    // REPOSITORIES - Ajoutez vos repositories ici
    // ============================================================

    // /// <summary>
    // /// Repository des produits (exemple)
    // /// </summary>
    // IRepository<Product> Products { get; }

    // /// <summary>
    // /// Repository des catégories (exemple)
    // /// </summary>
    // IRepository<Category> Categories { get; }

    // /// <summary>
    // /// Repository des commandes (exemple)
    // /// </summary>
    // IRepository<Order> Orders { get; }

    // ============================================================
    // MÉTHODES DE TRANSACTION
    // ============================================================

    /// <summary>
    /// Sauvegarde tous les changements dans la base de données
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Démarre une nouvelle transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Valide la transaction en cours
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Annule la transaction en cours
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
