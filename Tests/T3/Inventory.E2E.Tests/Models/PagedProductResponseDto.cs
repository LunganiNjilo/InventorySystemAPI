namespace Inventory.E2E.Tests.Models
{
    public class PagedProductResponseDto
    {
        public bool Success { get; set; }

        public List<ProductDto> Result { get; set; } = new();

        public int TotalRecordCount { get; set; }

        public int TotalPages { get; set; }

        public string PageNumberMessage { get; set; } = string.Empty;

        public bool IsPrevious { get; set; }

        public bool IsNext { get; set; }
    }
}
