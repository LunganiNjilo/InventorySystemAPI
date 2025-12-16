using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.E2E.Tests.Models
{
    public class GetAllProductCategoryDTO
    {
        public IEnumerable<ProductCategoryReadDto>? result { get; set; }
    }
}
