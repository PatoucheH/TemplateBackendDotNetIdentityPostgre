using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Data.Configurations;

/// <summary>
/// Fluent API configuration for ApplicationUser.
/// Defines constraints and indexes for the users table.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // ============================================================
        // TABLE CONFIGURATION
        // ============================================================

        // Table name (optional, inherits from Identity by default)
        // builder.ToTable("Users");

        // ============================================================
        // PROPERTIES
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
        // INDEXES
        // ============================================================

        // Index on IsActive for filtering queries
        builder.HasIndex(u => u.IsActive);

        // Index on CreatedAt for sorting queries
        builder.HasIndex(u => u.CreatedAt);

        // ============================================================
        // RELATIONSHIPS - Uncomment as needed
        // ============================================================

        // // One-to-Many relationship with Orders
        // builder.HasMany(u => u.Orders)
        //     .WithOne(o => o.User)
        //     .HasForeignKey(o => o.UserId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // // One-to-One relationship with UserProfile
        // builder.HasOne(u => u.Profile)
        //     .WithOne(p => p.User)
        //     .HasForeignKey<UserProfile>(p => p.UserId);
    }
}
