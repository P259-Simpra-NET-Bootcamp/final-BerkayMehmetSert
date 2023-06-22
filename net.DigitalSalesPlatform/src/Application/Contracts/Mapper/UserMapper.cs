using Application.Contracts.Requests.User;
using Application.Contracts.Responses.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Contracts.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterUserRequest, User>();
        CreateMap<RegisterAdminRequest, User>();
        CreateMap<UpdateUserRequest, User>();
        CreateMap<User, UserResponse>();
        CreateMap<User, UserPointResponse>();
    }
}