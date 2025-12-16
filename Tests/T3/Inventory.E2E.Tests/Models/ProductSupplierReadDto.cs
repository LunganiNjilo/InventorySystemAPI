namespace Inventory.E2E.Tests.Models
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
