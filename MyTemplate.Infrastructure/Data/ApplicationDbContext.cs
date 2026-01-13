using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Data;

/// <summary>
/// Main application DbContext.
/// Inherits from IdentityDbContext to integrate Identity.
///
/// CUSTOMIZATION: Add your DbSets and configurations here.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ============================================================
    // DBSETS - Add your entities here
    // ============================================================

    // /// <summary>
    // /// Products table (example)
    // /// </summary>
    // public DbSet<Product> Products => Set<Product>();

    // /// <summary>
    // /// Categories table (example)
    // /// </summary>
    // public DbSet<Category> Categories => Set<Category>();

    // /// <summary>
    // /// Orders table (example)
    // /// </summary>
    // public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all configurations from the Configurations folder
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // ============================================================
        // IDENTITY SCHEMA CONFIGURATION (optional)
        // ============================================================

        // Rename Identity tables if needed
        // builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
        // builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
        // builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        // builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        // builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        // builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        // builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
    }

    /// <summary>
    /// Override to automatically handle audit dates
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
