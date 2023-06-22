using Core.Application.Response;

namespace Application.Contracts.Responses.User;

public class UserPointResponse : BaseResponse
{
    public int PointBalance { get; set; }
    public DateTime LastTransactionDate { get; set; }
    public bool IsActive { get; set; }
}