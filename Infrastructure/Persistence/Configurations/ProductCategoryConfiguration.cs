using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
  {
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
      builder.ToTable("tb_ProductCategories");

      builder.HasKey(pc => pc.Id);

      builder.Property(pc => pc.ProductCategoryName)
          .HasMaxLength(200)
          .IsRequired();

      builder.HasIndex(pc => pc.ProductCategoryName)
          .IsUnique(false); // change to true if required
    }
  }
}
