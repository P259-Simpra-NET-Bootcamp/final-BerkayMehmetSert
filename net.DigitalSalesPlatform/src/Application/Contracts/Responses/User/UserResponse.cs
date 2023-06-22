using Core.Application.Response;

namespace Application.Contracts.Responses.User;

public class UserResponse : BaseResponse
{
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int PointBalance { get; set; }
    public DateTime LastTransactionDate { get; set; }
    public bool IsActive { get; set; }
}