using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
  public class ProductCategoryService : IProductCategoryService
  {
    private readonly IProductCategoryRepository _repository;
    private readonly IMapper _mapper;

    public ProductCategoryService(IProductCategoryRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ProductCategoryReadDto>> GetAllAsync()
    {
      var categories = await _repository.GetAllAsync();
      return _mapper.Map<IEnumerable<ProductCategoryReadDto>>(categories);
    }

    public async Task<ProductCategoryReadDto> GetByIdAsync(Guid id)
    {
      var category = await _repository.GetByIdAsync(id);
      return _mapper.Map<ProductCategoryReadDto>(category);
    }

    public async Task<ProductCategoryReadDto> CreateAsync(ProductCategoryCreateDto dto)
    {
      var entity = _mapper.Map<ProductCategory>(dto);
      var created = await _repository.CreateAsync(entity);
      return _mapper.Map<ProductCategoryReadDto>(created);
    }

    public async Task<ProductCategoryReadDto> UpdateAsync(Guid id, ProductCategoryCreateDto dto)
    {
      var existing = await _repository.GetByIdAsync(id);

      _mapper.Map(dto, existing);

      await _repository.UpdateAsync(existing);

      return _mapper.Map<ProductCategoryReadDto>(existing);
    }

    public async Task DeleteAsync(Guid id)
    {
      var existing = await _repository.GetByIdAsync(id);
      await _repository.DeleteAsync(existing);
    }
  }
}
