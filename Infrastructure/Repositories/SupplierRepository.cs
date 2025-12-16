using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
  public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
  {
    private readonly ApiDbContext _context;
    public SupplierRepository(ApiDbContext context) : base(context)
    {
      _context = context;
    }

    public async Task<(ICollection<Supplier> Result, int TotalRecordCount, int TotalPages, string PageNumberMessage, bool IsPrevious, bool IsNext)>
        SearchSortAndPaginationAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isDescending = false, int? pageNumber = 1, int? pageSize = 10)
    {
      Expression<Func<Supplier, bool>>? predicate = null;
      if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
      {
        switch (filterOn.ToUpperInvariant())
        {
          case "NAME":
          case "SUPPLIERNAME":
            predicate = s => s.SupplierName != null && s.SupplierName.Contains(filterQuery);
            break;
          case "EMAIL":
          case "CONTACTEMAIL":
            predicate = s => s.ContactEmail != null && s.ContactEmail.Contains(filterQuery);
            break;
          default:
            predicate = s => s.SupplierName != null && s.SupplierName.Contains(filterQuery);
            break;
        }
      }

      Expression<Func<Supplier, object>>? orderBy = null;
      if (!string.IsNullOrEmpty(sortBy))
      {
        switch (sortBy.ToUpperInvariant())
        {
          case "SUPPLIERNAME":
            orderBy = s => s.SupplierName ?? "";
            break;
          case "CONTACTEMAIL":
            orderBy = s => s.ContactEmail ?? "";
            break;
          default:
            orderBy = s => s.SupplierName ?? "";
            break;
        }
      }

      return await SearchSortAndPaginationAsync(predicate, orderBy, isDescending, pageNumber, pageSize);
    }
  }
}
