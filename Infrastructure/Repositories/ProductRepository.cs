using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
  public class ProductRepository : GenericRepository<Product>, IProductRepository
  {
    private new readonly ApiDbContext _context;

    public ProductRepository(ApiDbContext context) : base(context)
    {
      _context = context;
    }


    private Expression<Func<Product, bool>> GetPricePredicate(Expression<Func<Product, decimal>> priceSelector, string filterQuery)
    {
      var parameter = priceSelector.Parameters[0];
      var property = (MemberExpression)priceSelector.Body;

      if (filterQuery.StartsWith('-'))
      {
        throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'. It cannot be < 0", nameof(filterQuery));
      }
      else if (filterQuery.Contains('-'))
      {
        var bounds = filterQuery.Split('-');
        if (bounds.Length == 2
            && decimal.TryParse(bounds[0], out decimal min)
            && decimal.TryParse(bounds[1], out decimal max))
        {
          var minExpression = Expression.GreaterThanOrEqual(property, Expression.Constant(min));
          var maxExpression = Expression.LessThanOrEqual(property, Expression.Constant(max));
          var andExpression = Expression.AndAlso(minExpression, maxExpression);
          return Expression.Lambda<Func<Product, bool>>(andExpression, parameter);
        }
      }
      else if (decimal.TryParse(filterQuery, out decimal amount))
      {
        var equalExpression = Expression.Equal(property, Expression.Constant(amount));
        return Expression.Lambda<Func<Product, bool>>(equalExpression, parameter);
      }
      else
      {
        throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'", nameof(filterQuery));
      }

      throw new ArgumentException($"Invalid filter query for price: '{filterQuery}'", nameof(filterQuery));
    }

    Task<(ICollection<Product> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)> IProductRepository.SearchSortAndPaginationAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize)
    {
      // reuse and adapt existing logic you provided earlier
      Expression<Func<Product, bool>>? searchPredicate = null;

      if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
      {
        switch (filterOn.ToUpperInvariant())
        {
          case "PRODUCTNAME":
            searchPredicate = p => !string.IsNullOrEmpty(p.ProductName) && p.ProductName.Contains(filterQuery);
            break;
          case "CATEGORY":
            var categoryId = _context.ProductCategories!
                                    .Where(pc => pc.ProductCategoryName != null && pc.ProductCategoryName.Contains(filterQuery))
                                    .Select(pc => pc.Id)
                                    .FirstOrDefault();

            searchPredicate = p => p.FkProductCategory == categoryId;
            break;
          case "DESCRIPTION":
            searchPredicate = p => !string.IsNullOrEmpty(p.ProductDescription) && p.ProductDescription.Contains(filterQuery);
            break;
          case "SELLPRICE":
            searchPredicate = GetPricePredicate(p => p.SellPrice, filterQuery);
            break;
          case "COSTPRICE":
            searchPredicate = GetPricePredicate(p => p.CostPrice, filterQuery);
            break;
          default:
            throw new ArgumentException("Invalid filterOn value.");
        }
      }

      Expression<Func<Product, object>>? orderBy = null;

      if (!string.IsNullOrEmpty(sortBy))
      {
        switch (sortBy.ToUpperInvariant())
        {
          case "PRODUCTNAME":
            orderBy = p => !string.IsNullOrEmpty(p.ProductName) ? p.ProductName : "";
            break;
          case "CATEGORY":
            orderBy = p => p.ProductCategory != null && p.ProductCategory.ProductCategoryName != null
            ? p.ProductCategory.ProductCategoryName
            : "";
            break;
          case "DESCRIPTION":
            orderBy = p => !string.IsNullOrEmpty(p.ProductDescription) ? p.ProductDescription : "";
            break;
          case "SELLPRICE":
            orderBy = p => p.SellPrice;
            break;
          case "COSTPRICE":
            orderBy = p => p.CostPrice;
            break;
          default:
            throw new ArgumentException("Invalid sortBy value.");
        }
      }

      return base.SearchSortAndPaginationAsync(
         searchPredicate,
         orderBy,
         isDescending,
         pageNumber,
         pageSize,
         p => p.ProductCategory!
      );
    }
  }
}
