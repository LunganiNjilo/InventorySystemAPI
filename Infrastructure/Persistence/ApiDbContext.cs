using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
  public class ApiDbContext : DbContext
  {
    public DbSet<Inventory>? Inventory { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<ProductCategory>? ProductCategories { get; set; }
    public DbSet<ProductSupplier>? ProductSuppliers { get; set; }
    public DbSet<Supplier>? Suppliers { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfiguration(new ProductConfiguration());
      modelBuilder.ApplyConfiguration(new InventoryConfiguration());
      modelBuilder.ApplyConfiguration(new ProductSupplierConfiguration());
      modelBuilder.ApplyConfiguration(new SupplierConfiguration());
      modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

      modelBuilder.Entity<ProductCategory>(entity =>
      {
        entity.HasIndex(pc => pc.ProductCategoryName)
              .IsUnique();
      });

      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasIndex(p => p.ProductName).IsUnique();
        entity.Property(p => p.SellPrice).HasColumnType("decimal(10,2)");
        entity.Property(p => p.CostPrice).HasColumnType("decimal(10,2)");
      });

      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasOne(p => p.ProductCategory)
        .WithMany(pc => pc.Products)
        .HasForeignKey(p => p.FkProductCategory)
        .OnDelete(DeleteBehavior.Restrict);

      });

      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasOne(p => p.Inventory)
        .WithOne(i => i.Product)
        .HasForeignKey<Inventory>(i => i.FkProductId);

      });
    }
  }
}
