﻿using Application.Contracts.Mapper;
using Application.Contracts.Repositories;
using Application.Test.Mocks.FakeData;
using Core.Test.Repositories;
using Domain.Entities;

namespace Application.Test.Mocks.Repositories;

public class UserMockRepository : BaseMockRepository<IUserRepository, User, UserMapper, UserFakeData>
{
    public UserMockRepository(UserFakeData fakeData) : base(fakeData)
    {
    }
}