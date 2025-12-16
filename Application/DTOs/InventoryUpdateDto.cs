namespace Application.DTOs
{
  public class InventoryUpdateDto
  {
    public int QuantityInStock { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
  }
}
