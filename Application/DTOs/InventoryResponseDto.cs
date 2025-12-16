namespace Application.DTOs
{
  public class InventoryResponseDto
  {
    public Guid Id { get; set; }
    public string ProductName { get; set; } = "";
    public int QuantityInStock { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
  }
}
