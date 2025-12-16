using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
  public class SupplierService : ISupplierService
  {
    private readonly ISupplierRepository _repo;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    public async Task<PagedResult<SupplierReadDto>> SearchAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize)
    {
      var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext) =
          await _repo.SearchSortAndPaginationAsync(filterOn, filterQuery, sortBy, isDescending, pageNumber, pageSize);

      var dtos = result.Select(r => _mapper.Map<SupplierReadDto>(r)).ToList();
      return new PagedResult<SupplierReadDto>(dtos, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext);
    }

    public async Task<SupplierReadDto> GetByIdAsync(Guid id)
    {
      var entity = await _repo.GetByIdAsync(id);
      return _mapper.Map<SupplierReadDto>(entity);
    }

    public async Task<SupplierReadDto> CreateAsync(SupplierCreateDto dto)
    {
      var entity = _mapper.Map<Supplier>(dto);
      var created = await _repo.CreateAsync(entity);
      return _mapper.Map<SupplierReadDto>(created);
    }

    public async Task UpdateAsync(Guid id, SupplierUpdateDto dto)
    {
      var existing = await _repo.GetByIdAsync(id);
      _mapper.Map(dto, existing);
      await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(Guid id)
    {
      var existing = await _repo.GetByIdAsync(id);
      await _repo.DeleteAsync(existing);
    }
  }
}
