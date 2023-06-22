using Application.Contracts.Responses.Cart;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class CartMapper : Profile
{
    public CartMapper()
    {
        CreateMap<Product, CartItem>();
    }
}