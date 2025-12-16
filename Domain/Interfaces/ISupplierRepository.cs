using Domain.Entities;

namespace Domain.Interfaces
{
  public interface ISupplierRepository : IGenericRepository<Supplier>
  {
    Task<(ICollection<Supplier> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)>
        SearchSortAndPaginationAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isDescending = false, int? pageNumber = 1, int? pageSize = 10);
  }
}
