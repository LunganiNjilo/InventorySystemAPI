using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
  public class InventoryService : IInventoryService
  {
    private readonly IInventoryRepository _repo;
    private readonly IMapper _mapper;

    public InventoryService(IInventoryRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    public async Task<List<InventoryResponseDto>> SearchAsync(string? search, string? sortBy, bool desc)
    {
      var data = await _repo.GetAllAsync();

      // FILTER
      if (!string.IsNullOrWhiteSpace(search))
      {
        data = data.Where(i =>
            i.Product.ProductName!.Contains(search, StringComparison.OrdinalIgnoreCase)
        ).ToList();
      }

      // SORT
      data = sortBy?.ToLower() switch
      {
        "product" =>
            desc ? data.OrderByDescending(i => i.Product.ProductName).ToList()
                 : data.OrderBy(i => i.Product.ProductName).ToList(),

        "quantity" =>
            desc ? data.OrderByDescending(i => i.QuantityInStock).ToList()
                 : data.OrderBy(i => i.QuantityInStock).ToList(),

        _ => data
      };

      return _mapper.Map<List<InventoryResponseDto>>(data);
    }

    public async Task<InventoryResponseDto?> GetByIdAsync(Guid id)
    {
      var entity = await _repo.GetByIdAsync(id);
      return entity == null ? null : _mapper.Map<InventoryResponseDto>(entity);
    }

    public async Task<InventoryResponseDto> CreateAsync(InventoryCreateDto dto)
    {
      var entity = _mapper.Map<Inventory>(dto);
      await _repo.AddAsync(entity);

      var created = await _repo.GetByIdAsync(entity.Id);
      return _mapper.Map<InventoryResponseDto>(created);
    }

    public async Task UpdateAsync(Guid id, InventoryUpdateDto dto)
    {
      var entity = await _repo.GetByIdAsync(id);
      if (entity == null)
        throw new Exception("Inventory item not found.");

      _mapper.Map(dto, entity);
      await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
      var entity = await _repo.GetByIdAsync(id);
      if (entity == null)
        throw new Exception("Inventory item not found.");

      await _repo.DeleteAsync(entity);
    }
  }
}
