namespace Inventory.E2E.Tests.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string? ProductCategoryName { get; set; } 
    }
}
