using Domain.Entities;

namespace Domain.Specifications
{
  public class ProductWithCategorySpecification : BaseSpecification<Product>
  {
    public ProductWithCategorySpecification(Guid id) : base(p => p.Id == id)
    {
      AddInclude(p => p.ProductCategory);
    }
  }
}
