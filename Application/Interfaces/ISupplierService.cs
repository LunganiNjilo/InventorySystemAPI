using Application.DTOs;

namespace Application.Interfaces
{
  public interface ISupplierService
  {
    Task<PagedResult<SupplierReadDto>> SearchAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize);
    Task<SupplierReadDto> GetByIdAsync(Guid id);
    Task<SupplierReadDto> CreateAsync(SupplierCreateDto dto);
    Task UpdateAsync(Guid id, SupplierUpdateDto dto);
    Task DeleteAsync(Guid id);
  }
}
