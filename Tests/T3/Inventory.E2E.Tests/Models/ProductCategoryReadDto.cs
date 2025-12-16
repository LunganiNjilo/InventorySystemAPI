using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.E2E.Tests.Models
{
    public class ProductCategoryReadDto
    {
        public Guid Id { get; set; }
        public string ProductCategoryName { get; set; } = string.Empty;
    }
}
