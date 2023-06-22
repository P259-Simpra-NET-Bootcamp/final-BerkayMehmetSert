using Application.Contracts.Requests.Category;
using Application.Contracts.Responses.Category;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();
    }
}