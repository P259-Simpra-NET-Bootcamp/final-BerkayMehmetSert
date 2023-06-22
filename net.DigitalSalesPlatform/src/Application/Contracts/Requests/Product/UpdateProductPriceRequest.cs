using Core.Application.Request;

namespace Application.Contracts.Requests.Product;

public class UpdateProductPriceRequest : BaseRequest
{
    public decimal Price { get; set; }
}