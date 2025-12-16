using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
  public class ProductSupplierService : IProductSupplierService
  {
    private readonly IProductSupplierRepository _repository;
    private readonly IMapper _mapper;

    public ProductSupplierService(IProductSupplierRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ProductSupplierReadDto>> GetAllAsync()
    {
      var items = await _repository.GetAllAsync();
      return _mapper.Map<IEnumerable<ProductSupplierReadDto>>(items);
    }

    public async Task<ProductSupplierReadDto> GetByIdAsync(Guid id)
    {
      var item = await _repository.GetByIdAsync(id);
      return _mapper.Map<ProductSupplierReadDto>(item);
    }

    public async Task<ProductSupplierReadDto> CreateAsync(ProductSupplierCreateDto dto)
    {
      var entity = _mapper.Map<ProductSupplier>(dto);
      var created = await _repository.CreateAsync(entity);
      return _mapper.Map<ProductSupplierReadDto>(created);
    }

    public async Task<ProductSupplierReadDto> UpdateAsync(Guid id, ProductSupplierCreateDto dto)
    {
      var entity = await _repository.GetByIdAsync(id);
      _mapper.Map(dto, entity);
      await _repository.UpdateAsync(entity);

      return _mapper.Map<ProductSupplierReadDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
      var entity = await _repository.GetByIdAsync(id);
      await _repository.DeleteAsync(entity);
    }
  }
}
