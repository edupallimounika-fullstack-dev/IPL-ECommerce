using IPL.ECommerce.Domain.Entities;
using IPL.ECommerce.Domain.Enums;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public string ShippingAddress { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();
}