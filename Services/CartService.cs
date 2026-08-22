using IPL.ECommerce.Data;
using IPL.ECommerce.Domain.Entities;
using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPL.ECommerce.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CartService(
        ApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CartDto> GetCartAsync()
    {
        var cart = await GetOrCreateCartAsync();

        return MapCart(cart);
    }

    public async Task<CartDto> AddItemAsync(
        AddCartItemRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductId &&
                x.IsActive);

        if (product is null)
        {
            throw new KeyNotFoundException(
                "Product not found.");
        }

        var cart = await GetOrCreateCartAsync();

        var existingItem = cart.Items
            .FirstOrDefault(x =>
                x.ProductId == request.ProductId);

        var requestedQuantity =
            request.Quantity +
            (existingItem?.Quantity ?? 0);

        if (requestedQuantity > product.StockQuantity)
        {
            throw new InvalidOperationException(
                $"Only {product.StockQuantity} units " +
                $"are available.");
        }

        if (existingItem is not null)
        {
            existingItem.Quantity = requestedQuantity;
            existingItem.UnitPrice = product.Price;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.Price
            });
        }

        cart.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapCart(cart);
    }

    public async Task<CartDto> UpdateItemAsync(
        int productId,
        UpdateCartItemRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        var cart = await GetOrCreateCartAsync();

        var item = cart.Items
            .FirstOrDefault(x =>
                x.ProductId == productId);

        if (item is null)
        {
            throw new KeyNotFoundException(
                "Product is not present in the cart.");
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(x =>
                x.Id == productId &&
                x.IsActive);

        if (product is null)
        {
            throw new KeyNotFoundException(
                "Product not found.");
        }

        if (request.Quantity > product.StockQuantity)
        {
            throw new InvalidOperationException(
                $"Only {product.StockQuantity} units " +
                $"are available.");
        }

        item.Quantity = request.Quantity;
        item.UnitPrice = product.Price;

        cart.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapCart(cart);
    }

    public async Task<CartDto> RemoveItemAsync(
        int productId)
    {
        var cart = await GetOrCreateCartAsync();

        var item = cart.Items
            .FirstOrDefault(x =>
                x.ProductId == productId);

        if (item is null)
        {
            throw new KeyNotFoundException(
                "Product is not present in the cart.");
        }

        cart.Items.Remove(item);

        cart.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapCart(cart);
    }

    public async Task ClearCartAsync()
    {
        var cart = await GetOrCreateCartAsync();

        cart.Items.Clear();

        cart.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private async Task<Cart> GetOrCreateCartAsync()
    {
        var userId = _currentUser.UserId;

        var cart = await _context.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId);

        if (cart is not null)
        {
            return cart;
        }

        cart = new Cart
        {
            UserId = userId
        };

        _context.Carts.Add(cart);

        await _context.SaveChangesAsync();

        return cart;
    }

    private static CartDto MapCart(Cart cart)
    {
        var items = cart.Items
            .Select(x => new CartItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                ImageUrl = x.Product.ImageUrl,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.UnitPrice * x.Quantity,
                AvailableStock = x.Product.StockQuantity
            })
            .ToList();

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = items,
            TotalAmount = items.Sum(x => x.TotalPrice),
            TotalItems = items.Sum(x => x.Quantity)
        };
    }
}