using AutoMapper;
using YungChingAssessment.Api.DTOs;
using YungChingAssessment.Core.Entities;

namespace YungChingAssessment.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}
