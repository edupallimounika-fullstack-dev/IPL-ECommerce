using IPL.ECommerce.DTOs;
using IPL.ECommerce.Domain.Enums;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPL.ECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Gets products with optional search, filtering and pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
        [FromQuery] ProductSearchRequest request)
    {
        var result =
            await _productService.GetProductsAsync(request);

        return Ok(result);
    }

    /// <summary>
    /// Gets a product by its ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(
        int id)
    {
        var product =
            await _productService.GetProductByIdAsync(id);

        if (product is null)
        {
            return NotFound(new
            {
                message = $"Product with ID {id} was not found."
            });
        }

        return Ok(product);
    }
}