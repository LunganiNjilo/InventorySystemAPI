using Application.DTOs;

namespace Application.Interfaces
{
  public interface IProductSupplierService
  {
    Task<IEnumerable<ProductSupplierReadDto>> GetAllAsync();
    Task<ProductSupplierReadDto> GetByIdAsync(Guid id);
    Task<ProductSupplierReadDto> CreateAsync(ProductSupplierCreateDto dto);
    Task<ProductSupplierReadDto> UpdateAsync(Guid id, ProductSupplierCreateDto dto);
    Task DeleteAsync(Guid id);
  }
}
