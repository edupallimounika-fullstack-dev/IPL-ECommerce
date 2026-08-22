using IPL.ECommerce.DTOs;

namespace IPL.ECommerce.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(
        RegisterRequest request);

    Task<LoginResponse> LoginAsync(
        LoginRequest request);
}