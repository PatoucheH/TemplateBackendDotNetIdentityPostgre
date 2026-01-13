using Microsoft.EntityFrameworkCore.Storage;
using MyTemplate.Domain.Interfaces;
using MyTemplate.Infrastructure.Data;

namespace MyTemplate.Infrastructure.Repositories;

/// <summary>
/// Implémentation du pattern Unit of Work.
/// Coordonne les transactions entre plusieurs repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    // ============================================================
    // REPOSITORIES - Ajoutez vos repositories ici
    // ============================================================

    // private IRepository<Product>? _products;
    // private IRepository<Category>? _categories;
    // private IRepository<Order>? _orders;

    // public IRepository<Product> Products =>
    //     _products ??= new Repository<Product>(_context);

    // public IRepository<Category> Categories =>
    //     _categories ??= new Repository<Category>(_context);

    // public IRepository<Order> Orders =>
    //     _orders ??= new Repository<Order>(_context);

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // MÉTHODES DE TRANSACTION
    // ============================================================

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            _transaction.Dispose();
            _transaction = null;
        }
    }

    // ============================================================
    // DISPOSE
    // ============================================================

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
