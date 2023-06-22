using Core.Application.Request;

namespace Application.Contracts.Requests.User;

public class UpdateUserRequest : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}