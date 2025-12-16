using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
  public class ProductSupplierProfile : Profile
  {
    public ProductSupplierProfile()
    {
      CreateMap<ProductSupplier, ProductSupplierReadDto>()
          .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
          .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier!.SupplierName));

      CreateMap<ProductSupplierCreateDto, ProductSupplier>();
    }
  }
}
