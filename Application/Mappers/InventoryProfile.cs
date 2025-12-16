using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
  public class InventoryProfile : Profile
  {
    public InventoryProfile()
    {
      CreateMap<Inventory, InventoryResponseDto>()
          .ForMember(dest => dest.ProductName,
              opt => opt.MapFrom(src => src.Product.ProductName));

      CreateMap<InventoryCreateDto, Inventory>();
      CreateMap<InventoryUpdateDto, Inventory>();
    }
  }
}
