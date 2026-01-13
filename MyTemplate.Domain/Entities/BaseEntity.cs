namespace MyTemplate.Domain.Entities;

/// <summary>
/// Classe de base pour toutes les entités du domaine.
/// Fournit des propriétés communes d'audit.
///
/// UTILISATION : Héritez de cette classe pour vos entités.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identifiant unique de l'entité
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Date de création
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date de dernière modification
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Soft delete - indique si l'entité est supprimée
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}

// ============================================================
// EXEMPLES D'ENTITÉS - Décommentez et adaptez selon vos besoins
// ============================================================

// /// <summary>
// /// Exemple d'entité Product
// /// </summary>
// public class Product : BaseEntity
// {
//     public string Name { get; set; } = string.Empty;
//     public string? Description { get; set; }
//     public decimal Price { get; set; }
//     public int Stock { get; set; }
//     public bool IsAvailable { get; set; } = true;
//
//     // Relation avec Category (exemple)
//     public Guid? CategoryId { get; set; }
//     public virtual Category? Category { get; set; }
// }

// /// <summary>
// /// Exemple d'entité Category
// /// </summary>
// public class Category : BaseEntity
// {
//     public string Name { get; set; } = string.Empty;
//     public string? Description { get; set; }
//
//     // Navigation inverse
//     public virtual ICollection<Product>? Products { get; set; }
// }

// /// <summary>
// /// Exemple d'entité Order
// /// </summary>
// public class Order : BaseEntity
// {
//     public string OrderNumber { get; set; } = string.Empty;
//     public DateTime OrderDate { get; set; } = DateTime.UtcNow;
//     public decimal TotalAmount { get; set; }
//     public OrderStatus Status { get; set; } = OrderStatus.Pending;
//
//     // Relation avec ApplicationUser
//     public string UserId { get; set; } = string.Empty;
//     public virtual ApplicationUser? User { get; set; }
//
//     // Relation avec OrderItems
//     public virtual ICollection<OrderItem>? Items { get; set; }
// }

// /// <summary>
// /// Exemple d'énumération OrderStatus
// /// </summary>
// public enum OrderStatus
// {
//     Pending,
//     Processing,
//     Shipped,
//     Delivered,
//     Cancelled
// }
