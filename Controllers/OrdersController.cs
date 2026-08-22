using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPL.ECommerce.Controllers;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderDto>> Checkout(
        CheckoutRequest request)
    {
        var order =
            await _orderService.CheckoutAsync(request);

        return Ok(order);
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyList<OrderSummaryDto>>>
        GetOrderHistory()
    {
        var orders =
            await _orderService.GetOrderHistoryAsync();

        return Ok(orders);
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderDto>>
        GetOrderDetails(int orderId)
    {
        var order =
            await _orderService.GetOrderByIdAsync(
                orderId);

        if (order is null)
        {
            return NotFound(new
            {
                message =
                    $"Order {orderId} was not found."
            });
        }

        return Ok(order);
    }
}