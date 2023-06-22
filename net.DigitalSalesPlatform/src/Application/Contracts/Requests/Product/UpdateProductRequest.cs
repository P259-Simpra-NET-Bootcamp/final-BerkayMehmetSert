using Core.Application.Request;

namespace Application.Contracts.Requests.Product;

public class UpdateProductRequest : BaseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
}