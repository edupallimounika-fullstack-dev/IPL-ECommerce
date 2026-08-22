using IPL.ECommerce.Domain.Entities;
using IPL.ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IPL.ECommerce.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Franchise> Franchises => Set<Franchise>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureFranchise(modelBuilder);
        ConfigureProduct(modelBuilder);
        ConfigureCart(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigureUser(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureFranchise(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Franchise>(entity =>
        {
            entity.ToTable("Franchises");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.Property(x => x.CreatedDate)
                .IsRequired();

            entity.HasIndex(x => x.Code)
                .IsUnique();

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });
    }

    private static void ConfigureProduct(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasMaxLength(1000);

            entity.Property(x => x.ProductType)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.StockQuantity)
                .IsRequired();

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.Property(x => x.CreatedDate)
                .IsRequired();

            entity.HasIndex(x => x.FranchiseId);

            entity.HasIndex(x => x.ProductType);

            entity.HasIndex(x => x.IsActive);

            entity.HasOne(x => x.Franchise)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.FranchiseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCart(
    ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Carts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.CreatedDate)
                .IsRequired();

            entity.Property(x => x.ModifiedDate)
                .IsRequired();

            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItems");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.HasOne(x => x.Cart)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // A product should appear only once in a cart.
            entity.HasIndex(x => new
            {
                x.CartId,
                x.ProductId
            })
            .IsUnique();
        });
    }
    private static void ConfigureOrder(
    ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.OrderDate)
                .IsRequired();

            entity.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.ShippingAddress)
                .IsRequired()
                .HasMaxLength(1000);

            entity.HasIndex(x => x.UserId);

            entity.HasIndex(x => x.OrderDate);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.TotalPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.OrderId);

            entity.HasIndex(x => x.ProductId);
        });
    }
    private static void ConfigureUser(
    ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.HasOne(x => x.Cart)
                .WithOne(x => x.User)
                .HasForeignKey<Cart>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    private static void SeedData(ModelBuilder modelBuilder)
    {
        var franchises = new[]
        {
            new Franchise
            {
                Id = 1,
                Name = "Chennai Super Kings",
                Code = "CSK",
                LogoUrl = "/images/franchises/csk.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 2,
                Name = "Mumbai Indians",
                Code = "MI",
                LogoUrl = "/images/franchises/mi.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 3,
                Name = "Royal Challengers Bengaluru",
                Code = "RCB",
                LogoUrl = "/images/franchises/rcb.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 4,
                Name = "Kolkata Knight Riders",
                Code = "KKR",
                LogoUrl = "/images/franchises/kkr.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 5,
                Name = "Sunrisers Hyderabad",
                Code = "SRH",
                LogoUrl = "/images/franchises/srh.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 6,
                Name = "Rajasthan Royals",
                Code = "RR",
                LogoUrl = "/images/franchises/rr.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 7,
                Name = "Delhi Capitals",
                Code = "DC",
                LogoUrl = "/images/franchises/dc.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 8,
                Name = "Punjab Kings",
                Code = "PBKS",
                LogoUrl = "/images/franchises/pbks.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 9,
                Name = "Gujarat Titans",
                Code = "GT",
                LogoUrl = "/images/franchises/gt.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Franchise
            {
                Id = 10,
                Name = "Lucknow Super Giants",
                Code = "LSG",
                LogoUrl = "/images/franchises/lsg.png",
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<Franchise>()
            .HasData(franchises);

        var products = new[]
        {
            new Product
            {
                Id = 1,
                Name = "CSK Official Jersey",
                Description = "Official Chennai Super Kings jersey.",
                ProductType = ProductType.Jersey,
                Price = 2999.00m,
                StockQuantity = 100,
                ImageUrl = "/images/products/csk-jersey.png",
                FranchiseId = 1,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 2,
                Name = "CSK Official Cap",
                Description = "Official Chennai Super Kings cap.",
                ProductType = ProductType.Cap,
                Price = 999.00m,
                StockQuantity = 150,
                ImageUrl = "/images/products/csk-cap.png",
                FranchiseId = 1,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 3,
                Name = "CSK Supporter Flag",
                Description = "Chennai Super Kings supporter flag.",
                ProductType = ProductType.Flag,
                Price = 499.00m,
                StockQuantity = 200,
                ImageUrl = "/images/products/csk-flag.png",
                FranchiseId = 1,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 4,
                Name = "MI Official Jersey",
                Description = "Official Mumbai Indians jersey.",
                ProductType = ProductType.Jersey,
                Price = 2999.00m,
                StockQuantity = 100,
                ImageUrl = "/images/products/mi-jersey.png",
                FranchiseId = 2,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 5,
                Name = "MI Official Cap",
                Description = "Official Mumbai Indians cap.",
                ProductType = ProductType.Cap,
                Price = 999.00m,
                StockQuantity = 150,
                ImageUrl = "/images/products/mi-cap.png",
                FranchiseId = 2,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 6,
                Name = "RCB Official Jersey",
                Description = "Official Royal Challengers Bengaluru jersey.",
                ProductType = ProductType.Jersey,
                Price = 2999.00m,
                StockQuantity = 100,
                ImageUrl = "/images/products/rcb-jersey.png",
                FranchiseId = 3,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 7,
                Name = "RCB Official Cap",
                Description = "Official Royal Challengers Bengaluru cap.",
                ProductType = ProductType.Cap,
                Price = 999.00m,
                StockQuantity = 150,
                ImageUrl = "/images/products/rcb-cap.png",
                FranchiseId = 3,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 8,
                Name = "KKR Official Jersey",
                Description = "Official Kolkata Knight Riders jersey.",
                ProductType = ProductType.Jersey,
                Price = 2999.00m,
                StockQuantity = 100,
                ImageUrl = "/images/products/kkr-jersey.png",
                FranchiseId = 4,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 9,
                Name = "SRH Official Jersey",
                Description = "Official Sunrisers Hyderabad jersey.",
                ProductType = ProductType.Jersey,
                Price = 2999.00m,
                StockQuantity = 100,
                ImageUrl = "/images/products/srh-jersey.png",
                FranchiseId = 5,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new Product
            {
                Id = 10,
                Name = "Autographed CSK Photo",
                Description = "Signed Chennai Super Kings photo.",
                ProductType = ProductType.AutographedPhoto,
                Price = 4999.00m,
                StockQuantity = 25,
                ImageUrl = "/images/products/csk-signed-photo.png",
                FranchiseId = 1,
                IsActive = true,
                CreatedDate = new DateTime(
                    2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<Product>()
            .HasData(products);
    }
}