using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.DTOs.Common;
using MyTemplate.Application.Interfaces;
using System.Security.Claims;

namespace MyTemplate.Api.Controllers;

/// <summary>
/// Controller for user management.
/// Requires authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all active users
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")] // CUSTOMIZATION: Adjust required roles
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserDto>>>> GetAll()
    {
        var users = await _userService.GetAllActiveAsync();
        return Ok(ApiResponse<IReadOnlyList<UserDto>>.SuccessResult(users));
    }

    /// <summary>
    /// Gets a user by their ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(string id)
    {
        // Check that user is accessing their own data or is admin
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (currentUserId != id && !isAdmin)
        {
            return Forbid();
        }

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.FailureResult("User not found"));
        }

        return Ok(ApiResponse<UserDto>.SuccessResult(user));
    }

    /// <summary>
    /// Updates the current user's information
    /// </summary>
    /// <param name="updateDto">Data to update</param>
    /// <returns>Updated user</returns>
    [HttpPut("me")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateCurrentUser([FromBody] UpdateUserDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<UserDto>.FailureResult("Invalid data"));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var updatedUser = await _userService.UpdateAsync(userId, updateDto);
        if (updatedUser == null)
        {
            return BadRequest(ApiResponse<UserDto>.FailureResult("Update failed"));
        }

        return Ok(ApiResponse<UserDto>.SuccessResult(updatedUser, "Profile updated successfully"));
    }

    /// <summary>
    /// Changes the current user's password
    /// </summary>
    /// <param name="changePasswordDto">Password change data</param>
    /// <returns>Change result</returns>
    [HttpPut("me/password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse.Failure("Invalid data"));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
        if (!result)
        {
            return BadRequest(ApiResponse.Failure("Password change failed. Please verify your current password."));
        }

        return Ok(ApiResponse.SuccessResponse("Password changed successfully"));
    }

    /// <summary>
    /// Deactivates a user (Admin only)
    /// </summary>
    /// <param name="id">ID of user to deactivate</param>
    /// <returns>Deactivation result</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeactivateUser(string id)
    {
        var result = await _userService.DeactivateAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse.Failure("User not found"));
        }

        _logger.LogInformation("User deactivated: {UserId}", id);
        return Ok(ApiResponse.SuccessResponse("User deactivated successfully"));
    }

    // ============================================================
    // ROLE MANAGEMENT (Admin only)
    // ============================================================

    /// <summary>
    /// Adds a role to a user
    /// </summary>
    [HttpPost("{id}/roles/{roleName}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> AddRole(string id, string roleName)
    {
        var result = await _userService.AddToRoleAsync(id, roleName);
        if (!result)
        {
            return BadRequest(ApiResponse.Failure("Failed to add role"));
        }

        return Ok(ApiResponse.SuccessResponse($"Role '{roleName}' added successfully"));
    }

    /// <summary>
    /// Removes a role from a user
    /// </summary>
    [HttpDelete("{id}/roles/{roleName}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> RemoveRole(string id, string roleName)
    {
        var result = await _userService.RemoveFromRoleAsync(id, roleName);
        if (!result)
        {
            return BadRequest(ApiResponse.Failure("Failed to remove role"));
        }

        return Ok(ApiResponse.SuccessResponse($"Role '{roleName}' removed successfully"));
    }
}
