using System.Text.Json.Serialization;

namespace Domain.Entities
{
  public class Inventory : BaseEntity
  {
    public Guid FkProductId { get; set; }
    public int QuantityInStock { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; } = default!;
  }
}
