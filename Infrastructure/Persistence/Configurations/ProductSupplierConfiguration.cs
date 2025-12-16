using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
  {
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
      builder.ToTable("tb_ProductSuppliers");
      builder.HasKey(ps => ps.Id);

      builder.HasIndex(ps => new { ps.FkProductId, ps.FkSupplierId }).IsUnique();

      builder.HasOne(ps => ps.Product)
             .WithMany(p => p.ProductSuppliers)
             .HasForeignKey(ps => ps.FkProductId)
             .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(ps => ps.Supplier)
             .WithMany(s => s.ProductSuppliers)
             .HasForeignKey(ps => ps.FkSupplierId)
             .OnDelete(DeleteBehavior.Cascade);

      builder.Property(ps => ps.FkProductId).IsRequired();
      builder.Property(ps => ps.FkSupplierId).IsRequired();
    }
  }
}
