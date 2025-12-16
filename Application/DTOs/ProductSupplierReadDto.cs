using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
  public class ProductSupplierReadDto
  {
    public Guid Id { get; set; }
    public Guid FkProductId { get; set; }
    public string? ProductName { get; set; }
    public Guid FkSupplierId { get; set; }
    public string? SupplierName { get; set; }
  }

}
