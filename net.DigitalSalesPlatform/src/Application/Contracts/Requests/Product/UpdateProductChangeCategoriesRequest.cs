using Core.Application.Request;

namespace Application.Contracts.Requests.Product;

public class UpdateProductChangeCategoriesRequest : BaseRequest
{
    public ICollection<Guid> CategoryIds { get; set; }
}
