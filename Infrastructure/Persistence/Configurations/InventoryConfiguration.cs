using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
  {
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
      builder.ToTable("tb_Inventory");

      builder.HasKey(x => x.Id);

      builder.Property(x => x.QuantityInStock).IsRequired();
      builder.Property(x => x.MinStockLevel).IsRequired();
      builder.Property(x => x.MaxStockLevel).IsRequired();

      builder.HasOne(i => i.Product)
             .WithOne(p => p.Inventory)
             .HasForeignKey<Inventory>(i => i.FkProductId)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
