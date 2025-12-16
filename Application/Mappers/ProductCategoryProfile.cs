using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
  public class ProductCategoryProfile : Profile
  {
    public ProductCategoryProfile()
    {
      CreateMap<ProductCategory, ProductCategoryReadDto>();
      CreateMap<ProductCategoryCreateDto, ProductCategory>();
    }
  }
}
