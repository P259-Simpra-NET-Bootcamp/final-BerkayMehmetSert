using Application.Contracts.Requests.Product;
using Application.Contracts.Responses.Category;
using Application.Contracts.Responses.Product;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.ProductCategories.Select(ur => ur.Category)));
        
        CreateMap<Category, CategoryResponse>();
    }
}