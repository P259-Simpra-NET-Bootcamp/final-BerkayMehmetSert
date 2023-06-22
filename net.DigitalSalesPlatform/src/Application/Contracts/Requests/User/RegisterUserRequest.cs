using Core.Application.Request;

namespace Application.Contracts.Requests.User;

public class RegisterUserRequest : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}