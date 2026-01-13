using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Application.Interfaces;
using MyTemplate.Application.Services;

namespace MyTemplate.Application;

/// <summary>
/// Extension for Application dependency injection.
/// Centralizes application service configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services to the DI container.
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
        // ADD YOUR SERVICES HERE
        // ============================================================

        // services.AddScoped<IProductService, ProductService>();
        // services.AddScoped<IOrderService, OrderService>();
        // services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
