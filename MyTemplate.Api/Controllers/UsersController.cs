using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemplate.Application.DTOs.Auth;
using MyTemplate.Application.DTOs.Common;
using MyTemplate.Application.Interfaces;
using System.Security.Claims;

namespace MyTemplate.Api.Controllers;

/// <summary>
/// Controller pour la gestion des utilisateurs.
/// Nécessite une authentification.
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
    /// Récupère tous les utilisateurs actifs
    /// </summary>
    /// <returns>Liste des utilisateurs</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")] // PERSONNALISATION : Ajustez les rôles requis
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserDto>>>> GetAll()
    {
        var users = await _userService.GetAllActiveAsync();
        return Ok(ApiResponse<IReadOnlyList<UserDto>>.SuccessResult(users));
    }

    /// <summary>
    /// Récupère un utilisateur par son ID
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>Informations de l'utilisateur</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(string id)
    {
        // Vérifier que l'utilisateur accède à ses propres données ou est admin
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (currentUserId != id && !isAdmin)
        {
            return Forbid();
        }

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.FailureResult("Utilisateur non trouvé"));
        }

        return Ok(ApiResponse<UserDto>.SuccessResult(user));
    }

    /// <summary>
    /// Met à jour les informations de l'utilisateur connecté
    /// </summary>
    /// <param name="updateDto">Données à mettre à jour</param>
    /// <returns>Utilisateur mis à jour</returns>
    [HttpPut("me")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateCurrentUser([FromBody] UpdateUserDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<UserDto>.FailureResult("Données invalides"));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var updatedUser = await _userService.UpdateAsync(userId, updateDto);
        if (updatedUser == null)
        {
            return BadRequest(ApiResponse<UserDto>.FailureResult("Échec de la mise à jour"));
        }

        return Ok(ApiResponse<UserDto>.SuccessResult(updatedUser, "Profil mis à jour avec succès"));
    }

    /// <summary>
    /// Change le mot de passe de l'utilisateur connecté
    /// </summary>
    /// <param name="changePasswordDto">Données de changement de mot de passe</param>
    /// <returns>Résultat du changement</returns>
    [HttpPut("me/password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse.Failure("Données invalides"));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
        if (!result)
        {
            return BadRequest(ApiResponse.Failure("Échec du changement de mot de passe. Vérifiez votre mot de passe actuel."));
        }

        return Ok(ApiResponse.SuccessResponse("Mot de passe changé avec succès"));
    }

    /// <summary>
    /// Désactive un utilisateur (Admin uniquement)
    /// </summary>
    /// <param name="id">ID de l'utilisateur à désactiver</param>
    /// <returns>Résultat de la désactivation</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeactivateUser(string id)
    {
        var result = await _userService.DeactivateAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse.Failure("Utilisateur non trouvé"));
        }

        _logger.LogInformation("Utilisateur désactivé: {UserId}", id);
        return Ok(ApiResponse.SuccessResponse("Utilisateur désactivé avec succès"));
    }

    // ============================================================
    // GESTION DES RÔLES (Admin uniquement)
    // ============================================================

    /// <summary>
    /// Ajoute un rôle à un utilisateur
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
            return BadRequest(ApiResponse.Failure("Échec de l'ajout du rôle"));
        }

        return Ok(ApiResponse.SuccessResponse($"Rôle '{roleName}' ajouté avec succès"));
    }

    /// <summary>
    /// Retire un rôle d'un utilisateur
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
            return BadRequest(ApiResponse.Failure("Échec de la suppression du rôle"));
        }

        return Ok(ApiResponse.SuccessResponse($"Rôle '{roleName}' retiré avec succès"));
    }
}
