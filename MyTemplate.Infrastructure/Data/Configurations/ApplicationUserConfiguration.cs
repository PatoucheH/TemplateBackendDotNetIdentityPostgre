using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration Fluent API pour ApplicationUser.
/// Définit les contraintes et index pour la table des utilisateurs.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // ============================================================
        // CONFIGURATION DE LA TABLE
        // ============================================================

        // Nom de la table (optionnel, hérite d'Identity par défaut)
        // builder.ToTable("Users");

        // ============================================================
        // PROPRIÉTÉS
        // ============================================================

        builder.Property(u => u.FirstName)
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        // ============================================================
        // INDEX
        // ============================================================

        // Index sur IsActive pour les requêtes de filtrage
        builder.HasIndex(u => u.IsActive);

        // Index sur CreatedAt pour les requêtes de tri
        builder.HasIndex(u => u.CreatedAt);

        // ============================================================
        // RELATIONS - Décommentez selon vos besoins
        // ============================================================

        // // Relation 1-N avec Orders
        // builder.HasMany(u => u.Orders)
        //     .WithOne(o => o.User)
        //     .HasForeignKey(o => o.UserId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // Relation 1-1 avec UserProfile
        // builder.HasOne(u => u.Profile)
        //     .WithOne(p => p.User)
        //     .HasForeignKey<UserProfile>(p => p.UserId);
    }
}
