using Application.DTOs;

namespace Application.Interfaces
{
  public interface IProductCategoryService
  {
    Task<IEnumerable<ProductCategoryReadDto>> GetAllAsync();
    Task<ProductCategoryReadDto> GetByIdAsync(Guid id);
    Task<ProductCategoryReadDto> CreateAsync(ProductCategoryCreateDto dto);
    Task<ProductCategoryReadDto> UpdateAsync(Guid id, ProductCategoryCreateDto dto);
    Task DeleteAsync(Guid id);
  }
}
