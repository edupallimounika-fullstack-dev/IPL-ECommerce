namespace IPL.ECommerce.Services.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }

    string? Email { get; }
}