using IPL.ECommerce.Data;
using IPL.ECommerce.Domain.Entities;
using IPL.ECommerce.Domain.Enums;
using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IPL.ECommerce.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public OrderService(
        ApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<OrderDto> CheckoutAsync(
        CheckoutRequest request)
    {
        if (string.IsNullOrWhiteSpace(
                request.ShippingAddress))
        {
            throw new ArgumentException(
                "Shipping address is required.");
        }

        if (request.ShippingAddress.Length > 1000)
        {
            throw new ArgumentException(
                "Shipping address cannot exceed 1000 characters.");
        }

        var userId = _currentUser.UserId;

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId);

            if (cart is null || cart.Items.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cart is empty.");
            }

            var productIds = cart.Items
                .Select(x => x.ProductId)
                .ToList();

            var products = await _context.Products
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Confirmed,
                ShippingAddress =
                    request.ShippingAddress.Trim()
            };

            decimal totalAmount = 0;

            foreach (var cartItem in cart.Items)
            {
                if (!products.TryGetValue(
                        cartItem.ProductId,
                        out var product))
                {
                    throw new InvalidOperationException(
                        $"Product {cartItem.ProductId} no longer exists.");
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException(
                        $"Product '{product.Name}' is no longer available.");
                }

                if (cartItem.Quantity >
                    product.StockQuantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product.Name}'. " +
                        $"Available: {product.StockQuantity}, " +
                        $"Requested: {cartItem.Quantity}.");
                }

                var unitPrice = product.Price;

                var itemTotal =
                    unitPrice * cartItem.Quantity;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = cartItem.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = itemTotal
                });

                totalAmount += itemTotal;

                product.StockQuantity -=
                    cartItem.Quantity;

                product.ModifiedDate =
                    DateTime.UtcNow;
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(
                cart.Items);

            cart.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return MapOrder(order);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IReadOnlyList<OrderSummaryDto>>
        GetOrderHistoryAsync()
    {
        var userId = _currentUser.UserId;

        return await _context.Orders
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.OrderDate)
            .Select(x => new OrderSummaryDto
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,
                Status = x.Status.ToString(),
                TotalItems = x.Items.Sum(i => i.Quantity)
            })
            .ToListAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(
        int orderId)
    {
        var userId = _currentUser.UserId;

        return await _context.Orders
            .AsNoTracking()
            .Where(x =>
                x.Id == orderId &&
                x.UserId == userId)
            .Select(x => new OrderDto
            {
                Id = x.Id,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,
                Status = x.Status.ToString(),
                ShippingAddress = x.ShippingAddress,

                Items = x.Items
                    .OrderBy(i => i.Id)
                    .Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    private static OrderDto MapOrder(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),

            ShippingAddress =
                order.ShippingAddress,

            Items = order.Items
                .Select(x => new OrderItemDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice
                })
                .ToList()
        };
    }
}