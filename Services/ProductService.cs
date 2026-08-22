using IPL.ECommerce.Data;
using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPL.ECommerce.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(
        ProductSearchRequest request)
    {
        var pageNumber = request.PageNumber < 1
            ? 1
            : request.PageNumber;

        var pageSize = request.PageSize switch
        {
            < 1 => 10,
            > 100 => 100,
            _ => request.PageSize
        };

        IQueryable<Domain.Entities.Product> query =
            _context.Products
                .AsNoTracking()
                .Include(x => x.Franchise)
                .Where(x => x.IsActive);

        // Search by product name or description
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Description.Contains(search));
        }

        // Filter by product type
        if (request.Type.HasValue)
        {
            query = query.Where(x =>
                x.ProductType == request.Type.Value);
        }

        // Filter by franchise
        if (request.FranchiseId.HasValue)
        {
            query = query.Where(x =>
                x.FranchiseId == request.FranchiseId.Value);
        }

        // Total records before pagination
        var totalCount = await query.CountAsync();

        // Pagination
        var products = await query
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProductType = x.ProductType.ToString(),
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                ImageUrl = x.ImageUrl,
                FranchiseId = x.FranchiseId,
                FranchiseName = x.Franchise.Name
            })
            .ToListAsync();

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)pageSize);

        return new PagedResult<ProductDto>
        {
            Items = products,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProductType = x.ProductType.ToString(),
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                ImageUrl = x.ImageUrl,
                FranchiseId = x.FranchiseId,
                FranchiseName = x.Franchise.Name
            })
            .FirstOrDefaultAsync();
    }
}