namespace Domain.Entities
{
  public class ProductCategory : BaseEntity
  {
    public string ProductCategoryName { get; set; } = string.Empty;

    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
  }
}
