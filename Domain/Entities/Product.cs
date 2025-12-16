using System.Text.Json.Serialization;

namespace Domain.Entities
{
  public class Product : BaseEntity
  {
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public Guid FkProductCategory { get; set; }
    public decimal SellPrice { get; set; }
    public decimal CostPrice { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }

    [JsonIgnore]
    public virtual Inventory? Inventory { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProductSupplier> ProductSuppliers { get; set; }
        = new HashSet<ProductSupplier>();
  }
}
