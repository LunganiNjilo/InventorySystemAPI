using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
  public class ProductSupplierCreateDto
  {
    public Guid FkProductId { get; set; }
    public Guid FkSupplierId { get; set; }
  }
}
