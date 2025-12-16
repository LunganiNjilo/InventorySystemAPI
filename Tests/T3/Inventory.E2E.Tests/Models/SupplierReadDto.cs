namespace Inventory.E2E.Tests.Models
{
    public class SupplierReadDto
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? SupplierAddress { get; set; }
        public string? ContactFirstName { get; set; }
        public string? ContactLastName { get; set; }
        public string? ContactEmail { get; set; }
    }
}
