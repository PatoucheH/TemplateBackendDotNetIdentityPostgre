using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.Interfaces;
using MyTemplate.Domain.Entities;

namespace MyTemplate.Application.Services;

/// <summary>
/// Authentication service.
/// Handles registration, login and token management.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return AuthResponseDto.FailureResponse("A user with this email already exists");
        }

        // Create the new user
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName ?? registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to create user for {Email}: {Errors}", registerDto.Email, errors);
            return AuthResponseDto.FailureResponse($"Registration failed: {errors}");
        }

        // Add default User role
        await _userManager.AddToRoleAsync(user, "User");

        _logger.LogInformation("User created successfully: {UserId}", user.Id);

        // Generate token
        var token = await _jwtService.GenerateTokenAsync(user);
        var expiration = _jwtService.GetTokenExpiration();
        var refreshToken = _jwtService.GenerateRefreshToken();

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = MapToUserDto(user, roles.ToList());

        return AuthResponseDto.SuccessResponse(token, expiration, userDto, refreshToken);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        // Find user by email or username
        var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(loginDto.EmailOrUserName);

        if (user == null)
        {
            return AuthResponseDto.FailureResponse("Invalid credentials");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return AuthResponseDto.FailureResponse("This account has been deactivated");
        }

        // Verify password
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Account locked for user: {UserId}", user.Id);
            return AuthResponseDto.FailureResponse("Account locked. Please try again later.");
        }

        if (!result.Succeeded)
        {
            return AuthResponseDto.FailureResponse("Invalid credentials");
        }

        _logger.LogInformation("Login successful for user: {UserId}", user.Id);

        // Generate token
        var token = await _jwtService.GenerateTokenAsync(user);
        var expiration = _jwtService.GetTokenExpiration(loginDto.RememberMe);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = MapToUserDto(user, roles.ToList());

        return AuthResponseDto.SuccessResponse(token, expiration, userDto, refreshToken);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // CUSTOMIZATION: Implement the token refresh logic
        // You will need to store refresh tokens in the database
        // and validate the received token against the stored one

        await Task.CompletedTask;
        return AuthResponseDto.FailureResponse("Refresh token not implemented. See comments in code.");
    }

    public async Task<bool> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default)
    {
        // CUSTOMIZATION: Implement token revocation
        // Delete or invalidate the refresh token in the database

        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        await Task.CompletedTask;
        return _jwtService.ValidateToken(token);
    }

    // ============================================================
    // [COOKIES] - COOKIE METHODS (for Blazor)
    // Delete this section if you only use JWT
    // ============================================================

    public async Task<AuthResponseDto> LoginWithCookieAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        // Find user by email or username
        var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(loginDto.EmailOrUserName);

        if (user == null)
        {
            return AuthResponseDto.FailureResponse("Invalid credentials");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return AuthResponseDto.FailureResponse("This account has been deactivated");
        }

        // Sign in user with cookie creation
        var result = await _signInManager.PasswordSignInAsync(
            user,
            loginDto.Password,
            isPersistent: loginDto.RememberMe,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Account locked for user: {UserId}", user.Id);
            return AuthResponseDto.FailureResponse("Account locked. Please try again later.");
        }

        if (!result.Succeeded)
        {
            return AuthResponseDto.FailureResponse("Invalid credentials");
        }

        _logger.LogInformation("Cookie login successful for user: {UserId}", user.Id);

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = MapToUserDto(user, roles.ToList());

        // Return without JWT token (cookie is managed by Identity)
        return AuthResponseDto.SuccessResponse(token: null!, DateTime.UtcNow.AddDays(7), userDto, refreshToken: null);
    }

    public async Task LogoutWithCookieAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out via cookie");
    }

    // ============================================================
    // PRIVATE METHODS
    // ============================================================

    private static UserDto MapToUserDto(ApplicationUser user, List<string> roles)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            Roles = roles
        };
    }
}
