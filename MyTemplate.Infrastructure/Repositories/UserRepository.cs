using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Entities;
using MyTemplate.Domain.Interfaces;
using MyTemplate.Infrastructure.Data;

namespace MyTemplate.Infrastructure.Repositories;

/// <summary>
/// User repository implementation.
/// Handles user-specific operations.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName != null && u.UserName.ToLower() == userName.ToLower(), cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationUser>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user != null)
        {
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.UserName != null && u.UserName.ToLower() == userName.ToLower(), cancellationToken);
    }
}
