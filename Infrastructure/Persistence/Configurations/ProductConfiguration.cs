using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      // Table name
      builder.ToTable("tb_Products");

      // Key
      builder.HasKey(p => p.Id);

      // Properties
      builder.Property(p => p.ProductName)
             .HasMaxLength(200)
             .IsRequired(false);

      builder.Property(p => p.ProductDescription)
             .HasMaxLength(2000)
             .IsRequired(false);

      builder.Property(p => p.SellPrice)
             .HasColumnType("decimal(10,2)");

      builder.Property(p => p.CostPrice)
             .HasColumnType("decimal(10,2)");

      // Indexes
      builder.HasIndex(p => p.ProductName)
             .IsUnique(false); // You may set this to true later

      // Category relationship (Corrected)
      builder.HasOne(p => p.ProductCategory)
             .WithMany(pc => pc.Products)
             .HasForeignKey(p => p.FkProductCategory)
             .OnDelete(DeleteBehavior.Restrict);

      // Inventory 1:1
      builder.HasOne(p => p.Inventory)
             .WithOne(i => i.Product)
             .HasForeignKey<Inventory>(i => i.FkProductId);

      // ProductSupplier Many-to-Many (via join table)
      builder.HasMany(p => p.ProductSuppliers)
             .WithOne(ps => ps.Product)
             .HasForeignKey(ps => ps.FkProductId)
             .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
