namespace Application.DTOs
{
  public class InventoryCreateDto
  {
    public Guid FkProductId { get; set; }
    public int QuantityInStock { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
  }
}
