namespace MyTemplate.Domain.Entities;

/// <summary>
/// Base class for all domain entities.
/// Provides common audit properties.
///
/// USAGE: Inherit from this class for your entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier of the entity
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last modification date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Soft delete - indicates if the entity is deleted
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}

// ============================================================
// ENTITY EXAMPLES - Uncomment and adapt as needed
// ============================================================

// /// <summary>
// /// Product entity example
// /// </summary>
// public class Product : BaseEntity
// {
//     public string Name { get; set; } = string.Empty;
//     public string? Description { get; set; }
//     public decimal Price { get; set; }
//     public int Stock { get; set; }
//     public bool IsAvailable { get; set; } = true;
//
//     // Relationship with Category (example)
//     public Guid? CategoryId { get; set; }
//     public virtual Category? Category { get; set; }
// }

// /// <summary>
// /// Category entity example
// /// </summary>
// public class Category : BaseEntity
// {
//     public string Name { get; set; } = string.Empty;
//     public string? Description { get; set; }
//
//     // Inverse navigation
//     public virtual ICollection<Product>? Products { get; set; }
// }

// /// <summary>
// /// Order entity example
// /// </summary>
// public class Order : BaseEntity
// {
//     public string OrderNumber { get; set; } = string.Empty;
//     public DateTime OrderDate { get; set; } = DateTime.UtcNow;
//     public decimal TotalAmount { get; set; }
//     public OrderStatus Status { get; set; } = OrderStatus.Pending;
//
//     // Relationship with ApplicationUser
//     public string UserId { get; set; } = string.Empty;
//     public virtual ApplicationUser? User { get; set; }
//
//     // Relationship with OrderItems
//     public virtual ICollection<OrderItem>? Items { get; set; }
// }

// /// <summary>
// /// OrderStatus enumeration example
// /// </summary>
// public enum OrderStatus
// {
//     Pending,
//     Processing,
//     Shipped,
//     Delivered,
//     Cancelled
// }
