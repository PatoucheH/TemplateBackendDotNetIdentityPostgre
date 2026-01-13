using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Application.Interfaces;
using MyTemplate.Application.Services;

namespace MyTemplate.Application;

/// <summary>
/// Extension pour l'injection de dépendances de l'Application.
/// Centralise la configuration des services applicatifs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Ajoute les services applicatifs au conteneur DI.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ============================================================
        // SERVICES
        // ============================================================

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();

        // ============================================================
        // AJOUTEZ VOS SERVICES ICI
        // ============================================================

        // services.AddScoped<IProductService, ProductService>();
        // services.AddScoped<IOrderService, OrderService>();
        // services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
