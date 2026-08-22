using IPL.ECommerce.DTOs;

namespace IPL.ECommerce.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(
        ProductSearchRequest request);

    Task<ProductDto?> GetProductByIdAsync(int id);
}