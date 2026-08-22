namespace IPL.ECommerce.Domain.Entities
{
    public class Franchise
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

    }
}
