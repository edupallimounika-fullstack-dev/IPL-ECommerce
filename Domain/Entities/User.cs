namespace IPL.ECommerce.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public Cart? Cart { get; set; }

    public ICollection<Order> Orders { get; set; }
        = new List<Order>();
}