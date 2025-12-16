using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> SearchAsync(string? filterOn, string? filterQuery, string? sortBy, bool isDescending, int pageNumber, int pageSize)
    {
      var (result, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext)
          = await _repo.SearchSortAndPaginationAsync(filterOn, filterQuery, sortBy, isDescending, pageNumber, pageSize);

      var dtoList = result.Select(p => _mapper.Map<ProductDto>(p)).ToList();

      return new PagedResult<ProductDto>(dtoList, totalRecordCount, totalPages, pageNumberMessage, isPrevious, isNext);
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
      var entity = await _repo.GetByIdAsync(id);
      return _mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
      var entity = _mapper.Map<Product>(request);
      var created = await _repo.CreateAsync(entity);
      // reload including category for mapping productCategoryName
      var withSpec = await _repo.GetEntityWithSpecAsync(new ProductWithCategorySpecification(created.Id));
      return _mapper.Map<ProductDto>(withSpec!);
    }

    public async Task UpdateAsync(Guid id, UpdateProductRequest request)
    {
      var existing = await _repo.GetByIdAsync(id);
      _mapper.Map(request, existing);
      await _repo.UpdateAsync(existing);
    }

    public async Task DeleteAsync(Guid id)
    {
      var existing = await _repo.GetByIdAsync(id);
      await _repo.DeleteAsync(existing);
    }
  }
}
