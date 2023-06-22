using Application.Contracts.Responses.Token;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ITokenService
{
    TokenResponse CreateToken(User user);
}