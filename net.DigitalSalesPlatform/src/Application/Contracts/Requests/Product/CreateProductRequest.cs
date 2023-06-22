using Core.Application.Request;

namespace Application.Contracts.Requests.Product;

public class CreateProductRequest : BaseRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ICollection<Guid> CategoryIds { get; set; }
}