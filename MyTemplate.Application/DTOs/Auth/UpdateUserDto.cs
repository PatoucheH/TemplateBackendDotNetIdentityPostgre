using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO pour la mise à jour d'un utilisateur.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// Prénom
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Nom de famille
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    public string? LastName { get; set; }

    /// <summary>
    /// Numéro de téléphone
    /// </summary>
    [Phone(ErrorMessage = "Format de numéro de téléphone invalide")]
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO pour le changement de mot de passe.
/// </summary>
public class ChangePasswordDto
{
    /// <summary>
    /// Mot de passe actuel
    /// </summary>
    [Required(ErrorMessage = "Le mot de passe actuel est requis")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Nouveau mot de passe
    /// </summary>
    [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
    [MaxLength(100, ErrorMessage = "Le mot de passe ne peut pas dépasser 100 caractères")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation du nouveau mot de passe
    /// </summary>
    [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
    [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
