namespace Application.DTOs
{
  public class CreateProductRequest
  {
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public decimal SellPrice { get; set; }
    public decimal CostPrice { get; set; }
    public Guid? FkProductCategory { get; set; }
  }
}
