using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
  public class InventoryRepository : IInventoryRepository
  {
    private readonly ApiDbContext _context;

    public InventoryRepository(ApiDbContext context)
    {
      _context = context;
    }

    public async Task<List<Inventory>> GetAllAsync()
    {
      return await _context.Inventory!
          .Include(i => i.Product)
          .ToListAsync();
    }

    public async Task<Inventory?> GetByIdAsync(Guid id)
    {
      return await _context.Inventory!
          .Include(i => i.Product)
          .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Inventory> AddAsync(Inventory entity)
    {
      _context.Inventory!.Add(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public async Task UpdateAsync(Inventory entity)
    {
      _context.Inventory!.Update(entity);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Inventory entity)
    {
      _context.Inventory!.Remove(entity);
      await _context.SaveChangesAsync();
    }
  }
}
