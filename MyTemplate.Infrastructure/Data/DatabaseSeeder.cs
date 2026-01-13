using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Infrastructure.Data;

/// <summary>
/// Seeder to initialize roles and default admin user.
/// </summary>
public class DatabaseSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    // Role names
    public const string RoleAdmin = "Admin";
    public const string RoleUser = "User";

    public DatabaseSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Initializes base data (roles and admin).
    /// </summary>
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    /// <summary>
    /// Creates User and Admin roles if they don't exist.
    /// </summary>
    private async Task SeedRolesAsync()
    {
        string[] roles = [RoleUser, RoleAdmin];

        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    _logger.LogInformation("Role '{Role}' created successfully", roleName);
                }
                else
                {
                    _logger.LogError("Failed to create role '{Role}': {Errors}",
                        roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    /// <summary>
    /// Creates the default admin user if it doesn't exist.
    /// Credentials are read from configuration (appsettings.json).
    /// </summary>
    private async Task SeedAdminUserAsync()
    {
        var adminSettings = _configuration.GetSection("AdminSettings");

        var adminEmail = adminSettings["Email"] ?? "admin@example.com";
        var adminUserName = adminSettings["UserName"] ?? "admin";
        var adminPassword = adminSettings["Password"] ?? "Admin123!";
        var adminFirstName = adminSettings["FirstName"] ?? "Admin";
        var adminLastName = adminSettings["LastName"] ?? "System";

        // Check if admin already exists
        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            _logger.LogInformation("Admin user already exists");

            // Ensure they have the Admin role
            if (!await _userManager.IsInRoleAsync(existingAdmin, RoleAdmin))
            {
                await _userManager.AddToRoleAsync(existingAdmin, RoleAdmin);
                _logger.LogInformation("Admin role added to existing user");
            }
            return;
        }

        // Create admin user
        var adminUser = new ApplicationUser
        {
            UserName = adminUserName,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = adminFirstName,
            LastName = adminLastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(adminUser, adminPassword);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create admin user: {Errors}",
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        // Assign Admin and User roles
        await _userManager.AddToRolesAsync(adminUser, [RoleAdmin, RoleUser]);

        _logger.LogInformation("Admin user created successfully: {Email}", adminEmail);
    }
}
