using System.Security.Claims;
using IPL.ECommerce.Services.Interfaces;

namespace IPL.ECommerce.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var value =
                _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(
                        ClaimTypes.NameIdentifier);

            if (!int.TryParse(value, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            return userId;
        }
    }

    public string? Email =>
        _httpContextAccessor.HttpContext?
            .User
            .FindFirstValue(ClaimTypes.Email);
}