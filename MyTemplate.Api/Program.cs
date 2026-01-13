using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyTemplate.Application;
using MyTemplate.Infrastructure;
using MyTemplate.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// SERVICE CONFIGURATION
// ============================================================

// Infrastructure (DbContext, Identity, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Application (Business services)
builder.Services.AddApplication();

// Database Seeder
builder.Services.AddScoped<DatabaseSeeder>();

// ============================================================
// AUTHENTICATION - CHOOSE YOUR MODE
// ============================================================
//
// This template supports TWO authentication modes:
//
// 1. JWT (for Angular, React, Vue, Mobile)
//    - Endpoints: /api/auth/login, /api/auth/register
//    - Client stores the token and sends it in the Authorization header
//
// 2. COOKIES (for Blazor Server, Blazor WASM hosted, MVC)
//    - Endpoints: /api/auth/cookie/login, /api/auth/cookie/logout
//    - Browser automatically manages cookies
//
// TO REMOVE A MODE:
// - Delete the corresponding section below
// - Delete the corresponding endpoints in AuthController.cs
// - Delete the corresponding methods in AuthService.cs
// ============================================================

// ============================================================
// [JWT] - JWT SECTION (for SPA / Mobile)
// Delete this section if you only use Blazor
// ============================================================

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey not configured. Please set JwtSettings:SecretKey in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    // Default scheme: JWT
    // If you only use cookies, change these lines
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
})
// ============================================================
// [COOKIES] - COOKIE SECTION (for Blazor)
// Delete this section if you only use JWT
// ============================================================
.AddCookie(options =>
{
    options.Cookie.Name = "MyTemplate.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;

    // API responses (no redirect)
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

// ============================================================
// CORS
// ============================================================

builder.Services.AddCors(options =>
{
    // CUSTOMIZATION: Configure allowed origins based on your environment
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Restrictive policy for production (recommended)
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["https://localhost:3000"]
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ============================================================
// CONTROLLERS & SWAGGER
// ============================================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyTemplate API",
        Version = "v1",
        Description = "Backend API Template with Identity and PostgreSQL",
        // CUSTOMIZATION: Add your information
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "contact@example.com"
        }
    });

    // JWT authentication configuration in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ============================================================
// DATABASE SEEDING
// ============================================================

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// ============================================================
// MIDDLEWARE PIPELINE
// ============================================================

// Swagger (available in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyTemplate API v1");
        options.RoutePrefix = string.Empty; // Swagger at root
    });
}

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS - CUSTOMIZATION: Change policy based on environment
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// ============================================================
// STARTUP
// ============================================================

app.Run();
