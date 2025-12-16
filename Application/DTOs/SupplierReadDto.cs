namespace Application.DTOs
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
