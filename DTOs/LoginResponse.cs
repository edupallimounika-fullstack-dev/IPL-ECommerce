namespace IPL.ECommerce.DTOs;

public class LoginResponse
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}