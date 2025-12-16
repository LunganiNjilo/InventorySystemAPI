using Domain.Entities;

namespace Domain.Interfaces
{
  public interface IInventoryRepository
  {
    Task<List<Inventory>> GetAllAsync();
    Task<Inventory?> GetByIdAsync(Guid id);
    Task<Inventory> AddAsync(Inventory entity);
    Task UpdateAsync(Inventory entity);
    Task DeleteAsync(Inventory entity);
  }
}
