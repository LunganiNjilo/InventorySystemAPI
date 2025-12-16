using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
  public interface IProductRepository : IGenericRepository<Product>
  {
    Task<(ICollection<Product> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)>
        SearchSortAndPaginationAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize);
  }
}
