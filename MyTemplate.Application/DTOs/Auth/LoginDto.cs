using System.ComponentModel.DataAnnotations;

namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO pour la connexion d'un utilisateur.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Email ou nom d'utilisateur
    /// </summary>
    [Required(ErrorMessage = "L'email ou nom d'utilisateur est requis")]
    public string EmailOrUserName { get; set; } = string.Empty;

    /// <summary>
    /// Mot de passe
    /// </summary>
    [Required(ErrorMessage = "Le mot de passe est requis")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Se souvenir de moi (prolonge la durée du token)
    /// </summary>
    public bool RememberMe { get; set; } = false;
}
