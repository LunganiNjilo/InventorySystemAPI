using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
  public class ProductCategoryRepository
      : GenericRepository<ProductCategory>, IProductCategoryRepository
  {
    public ProductCategoryRepository(ApiDbContext context) : base(context)
    {
    }
  }
}
