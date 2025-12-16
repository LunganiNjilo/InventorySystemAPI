using Application.DTOs;

namespace Application.Interfaces
{
  public interface IInventoryService
  {
    Task<List<InventoryResponseDto>> SearchAsync(string? search, string? sortBy, bool desc);
    Task<InventoryResponseDto?> GetByIdAsync(Guid id);
    Task<InventoryResponseDto> CreateAsync(InventoryCreateDto dto);
    Task UpdateAsync(Guid id, InventoryUpdateDto dto);
    Task DeleteAsync(Guid id);
  }
}
