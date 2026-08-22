using IPL.ECommerce.Domain.Enums;

namespace IPL.ECommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProductType ProductType { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    // Foreign Key
    public int FranchiseId { get; set; }

    // Navigation Property
    public Franchise Franchise { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; }
        = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; }
    = new List<OrderItem>();
}