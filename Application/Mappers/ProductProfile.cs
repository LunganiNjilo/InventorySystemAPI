using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductName,
                    o => o.MapFrom(s => s.ProductName))
                .ForMember(d => d.ProductDescription,
                    o => o.MapFrom(s => s.ProductDescription))
                .ForMember(d => d.SellPrice,
                    o => o.MapFrom(s => s.SellPrice))
                .ForMember(d => d.CostPrice,
                    o => o.MapFrom(s => s.CostPrice))
                .ForMember(d => d.ProductCategoryName,
                    o => o.MapFrom(s =>
                        s.ProductCategory != null
                            ? s.ProductCategory.ProductCategoryName
                            : null));

            CreateMap<CreateProductRequest, Product>()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<UpdateProductRequest, Product>()
                .ForMember(d => d.Id, o => o.Ignore());
        }
    }
}
