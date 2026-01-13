using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Data;

/// <summary>
/// DbContext principal de l'application.
/// Hérite de IdentityDbContext pour intégrer Identity.
///
/// PERSONNALISATION : Ajoutez vos DbSet et configurations ici.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ============================================================
    // DBSETS - Ajoutez vos entités ici
    // ============================================================

    // /// <summary>
    // /// Table des produits (exemple)
    // /// </summary>
    // public DbSet<Product> Products => Set<Product>();

    // /// <summary>
    // /// Table des catégories (exemple)
    // /// </summary>
    // public DbSet<Category> Categories => Set<Category>();

    // /// <summary>
    // /// Table des commandes (exemple)
    // /// </summary>
    // public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Applique toutes les configurations du dossier Configurations
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // ============================================================
        // CONFIGURATION DU SCHÉMA IDENTITY (optionnel)
        // ============================================================

        // Renomme les tables Identity si nécessaire
        // builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
        // builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
        // builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        // builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        // builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        // builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        // builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
    }

    /// <summary>
    /// Override pour gérer automatiquement les dates d'audit
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
