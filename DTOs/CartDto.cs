namespace IPL.ECommerce.DTOs;

public class CartDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<CartItemDto> Items { get; set; } = [];

    public decimal TotalAmount { get; set; }

    public int TotalItems { get; set; }
}