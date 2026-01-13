using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Domain.Entities;

/// <summary>
/// Utilisateur personnalisé de l'application.
/// Hérite de IdentityUser pour bénéficier de toutes les propriétés Identity.
///
/// PERSONNALISATION : Ajoutez vos propriétés personnalisées ici.
/// </summary>
public class ApplicationUser : IdentityUser
{
    // ============================================================
    // PROPRIÉTÉS PERSONNALISÉES - Ajoutez les vôtres ci-dessous
    // ============================================================

    /// <summary>
    /// Prénom de l'utilisateur
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Nom de famille de l'utilisateur
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Date de création du compte
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date de dernière mise à jour
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indique si l'utilisateur est actif
    /// </summary>
    public bool IsActive { get; set; } = true;

    // ============================================================
    // RELATIONS - Décommentez et adaptez selon vos besoins
    // ============================================================

    // /// <summary>
    // /// Collection des commandes de l'utilisateur (exemple)
    // /// </summary>
    // public virtual ICollection<Order>? Orders { get; set; }

    // /// <summary>
    // /// Profil de l'utilisateur (exemple relation 1-1)
    // /// </summary>
    // public virtual UserProfile? Profile { get; set; }
}
