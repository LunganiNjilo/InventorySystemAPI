using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductSupplierRepository : GenericRepository<ProductSupplier>, IProductSupplierRepository
    {
        public ProductSupplierRepository(ApiDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ProductSupplier>> GetAllAsync()
        {
            return await _context.ProductSuppliers!
                .Include(ps => ps.Product)
                .Include(ps => ps.Supplier)
                .ToListAsync();
        }

        public override async Task<ProductSupplier> GetByIdAsync(Guid id)
        {
            var entity = await _context.ProductSuppliers!
                .Include(ps => ps.Product)
                .Include(ps => ps.Supplier)
                .FirstOrDefaultAsync(ps => ps.Id == id);

            if (entity == null)
                throw new KeyNotFoundException(
                    $"{typeof(ProductSupplier).Name} with id {id} was not found");


            return entity;
        }
    }

}
