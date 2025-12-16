using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
  public class ProductSupplier : BaseEntity
  {
    public Guid FkProductId { get; set; }
    public Guid FkSupplierId { get; set; }

    [JsonIgnore]
    public virtual Product? Product { get; set; }

    [JsonIgnore]
    public virtual Supplier? Supplier { get; set; }
  }
}
