using IPL.ECommerce.Domain.Enums;

namespace IPL.ECommerce.DTOs;

public class ProductSearchRequest
{
    public string? Search { get; set; }

    public ProductType? Type { get; set; }

    public int? FranchiseId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}