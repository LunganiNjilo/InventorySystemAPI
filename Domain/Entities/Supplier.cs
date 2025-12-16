using System.Text.Json.Serialization;

namespace Domain.Entities
{
  public class Supplier : BaseEntity
  {
    public string? SupplierName { get; set; }
    public string? SupplierAddress { get; set; }

    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
    public string? ContactEmail { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProductSupplier> ProductSuppliers { get; set; }
        = new HashSet<ProductSupplier>();
  }
}
