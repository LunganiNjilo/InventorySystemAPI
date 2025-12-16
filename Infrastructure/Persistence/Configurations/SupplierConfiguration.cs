using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
  {
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
      builder.ToTable("tb_Suppliers");
      builder.HasKey(s => s.Id);

      builder.Property(s => s.SupplierName).HasMaxLength(200).IsRequired();
      builder.Property(s => s.SupplierAddress).HasMaxLength(1000).IsRequired(false);
      builder.Property(s => s.ContactFirstName).HasMaxLength(100).IsRequired(false);
      builder.Property(s => s.ContactLastName).HasMaxLength(100).IsRequired(false);
      builder.Property(s => s.ContactEmail).HasMaxLength(255).IsRequired(false);

      builder.HasIndex(s => s.SupplierName).IsUnique(false);
      builder.HasIndex(s => s.ContactEmail).IsUnique(false);
    }
  }
}
