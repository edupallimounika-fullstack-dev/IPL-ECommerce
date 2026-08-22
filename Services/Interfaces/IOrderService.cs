using IPL.ECommerce.DTOs;

namespace IPL.ECommerce.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CheckoutAsync(
        CheckoutRequest request);

    Task<IReadOnlyList<OrderSummaryDto>>
        GetOrderHistoryAsync();

    Task<OrderDto?> GetOrderByIdAsync(
        int orderId);
}