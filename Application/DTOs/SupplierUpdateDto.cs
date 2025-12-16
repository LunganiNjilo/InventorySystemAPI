using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
  public class SupplierUpdateDto
  {
    public string? SupplierName { get; set; }
    public string? SupplierAddress { get; set; }

    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
    public string? ContactEmail { get; set; }
  }
}
