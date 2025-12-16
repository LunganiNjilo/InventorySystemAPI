namespace Inventory.E2E.Tests.Models
{
    public class PagedSupplierResponseDto
    {
        public List<SupplierReadDto> Result { get; set; } = new();

        public int TotalRecordCount { get; set; }

        public int TotalPages { get; set; }

        public string PageNumberMessage { get; set; } = string.Empty;

        public bool IsPrevious { get; set; }

        public bool IsNext { get; set; }
    }
}
