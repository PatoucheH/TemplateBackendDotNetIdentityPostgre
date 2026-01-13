using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO pour l'inscription d'un nouvel utilisateur.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Adresse email (sera aussi utilisée comme nom d'utilisateur par défaut)
    /// </summary>
    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    [MaxLength(256, ErrorMessage = "L'email ne peut pas dépasser 256 caractères")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nom d'utilisateur (optionnel, utilise l'email si non fourni)
    /// </summary>
    [MaxLength(256, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 256 caractères")]
    public string? UserName { get; set; }

    /// <summary>
    /// Mot de passe
    /// </summary>
    [Required(ErrorMessage = "Le mot de passe est requis")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
    [MaxLength(100, ErrorMessage = "Le mot de passe ne peut pas dépasser 100 caractères")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation du mot de passe
    /// </summary>
    [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
    [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Prénom de l'utilisateur (optionnel)
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Nom de famille de l'utilisateur (optionnel)
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    public string? LastName { get; set; }
}
