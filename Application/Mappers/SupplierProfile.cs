using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
  public class SupplierProfile : Profile
  {
    public SupplierProfile()
    {
      CreateMap<Supplier, SupplierReadDto>();
      CreateMap<SupplierCreateDto, Supplier>();
      CreateMap<SupplierUpdateDto, Supplier>();
    }
  }
}
