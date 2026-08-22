using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPL.ECommerce.Controllers;

[Authorize]
[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        var cart = await _cartService.GetCartAsync();

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartDto>> AddItem(
        AddCartItemRequest request)
    {
        var cart = await _cartService.AddItemAsync(request);

        return Ok(cart);
    }

    [HttpPut("items/{productId:int}")]
    public async Task<ActionResult<CartDto>> UpdateItem(
        int productId,
        UpdateCartItemRequest request)
    {
        var cart = await _cartService.UpdateItemAsync(
            productId,
            request);

        return Ok(cart);
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<ActionResult<CartDto>> RemoveItem(
        int productId)
    {
        var cart = await _cartService.RemoveItemAsync(
            productId);

        return Ok(cart);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync();

        return NoContent();
    }
}