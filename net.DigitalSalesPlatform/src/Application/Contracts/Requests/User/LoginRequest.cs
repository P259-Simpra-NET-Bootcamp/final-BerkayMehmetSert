using Core.Application.Request;

namespace Application.Contracts.Requests.User;

public class LoginRequest : BaseRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}