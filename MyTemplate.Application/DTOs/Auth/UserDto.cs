namespace MyTemplate.Application.DTOs.Auth;

/// <summary>
/// DTO représentant un utilisateur.
/// Utilisé pour les réponses API.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Identifiant unique de l'utilisateur
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nom d'utilisateur
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Prénom
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Nom de famille
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Nom complet (calculé)
    /// </summary>
    public string? FullName => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)
        ? null
        : $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Email confirmé
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Numéro de téléphone
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Date de création du compte
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Utilisateur actif
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Rôles de l'utilisateur
    /// </summary>
    public List<string> Roles { get; set; } = [];
}
