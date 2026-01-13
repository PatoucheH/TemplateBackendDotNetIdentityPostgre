using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Domain.Entities;
using MyTemplate.Domain.Interfaces;
using MyTemplate.Infrastructure.Data;
using MyTemplate.Infrastructure.Repositories;

namespace MyTemplate.Infrastructure;

/// <summary>
/// Extension pour l'injection de dépendances de l'Infrastructure.
/// Centralise la configuration des services d'infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Ajoute les services d'infrastructure au conteneur DI.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================================
        // POSTGRESQL / ENTITY FRAMEWORK
        // ============================================================

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Résilience des connexions
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);

                // Migration assembly
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            }));

        // ============================================================
        // IDENTITY
        // ============================================================

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // PERSONNALISATION : Configurez les options de mot de passe
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // PERSONNALISATION : Configurez les options de lockout
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // PERSONNALISATION : Configurez les options utilisateur
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            // PERSONNALISATION : Configurez les options de sign-in
            options.SignIn.RequireConfirmedEmail = false; // Mettre à true en production
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // ============================================================
        // REPOSITORIES
        // ============================================================

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
