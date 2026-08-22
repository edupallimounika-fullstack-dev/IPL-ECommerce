using IPL.ECommerce.DTOs;

namespace IPL.ECommerce.Services.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync();

    Task<CartDto> AddItemAsync(
        AddCartItemRequest request);

    Task<CartDto> UpdateItemAsync(
        int productId,
        UpdateCartItemRequest request);

    Task<CartDto> RemoveItemAsync(
        int productId);

    Task ClearCartAsync();
}