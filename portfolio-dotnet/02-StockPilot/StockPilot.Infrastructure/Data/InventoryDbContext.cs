using Microsoft.EntityFrameworkCore;
using StockPilot.Core.Domain;
namespace StockPilot.Infrastructure.Data;
public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> Movements => Set<StockMovement>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Sku).IsUnique();
            e.Property(x => x.Sku).HasMaxLength(40).IsRequired();
            e.Property(x => x.Name).HasMaxLength(160).IsRequired();
            e.Property(x => x.Version).IsRequired().IsConcurrencyToken().HasMaxLength(32);
            e.Ignore(x => x.IsLowStock);
        });
        b.Entity<StockMovement>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
            e.HasIndex(x => new { x.ProductId, x.CreatedAtUtc });
            e.Property(x => x.Type).HasConversion<string>().HasMaxLength(10);
            e.Property(x => x.Reason).HasMaxLength(300).IsRequired();
        });
    }
}
