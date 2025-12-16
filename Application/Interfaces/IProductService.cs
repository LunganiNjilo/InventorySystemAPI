using Application.DTOs;

namespace Application.Interfaces
{
  public interface IProductService
  {
    Task<PagedResult<ProductDto>> SearchAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize);
    Task<ProductDto> GetByIdAsync(Guid id);
    Task<ProductDto> CreateAsync(CreateProductRequest request);
    Task UpdateAsync(Guid id, UpdateProductRequest request);
    Task DeleteAsync(Guid id);
  }
}
