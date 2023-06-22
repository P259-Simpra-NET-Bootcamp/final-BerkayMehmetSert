using Application.Contracts.Responses.Category;
using Core.Application.Response;

namespace Application.Contracts.Responses.Product;

public class ProductResponse : BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Features { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public decimal ScorePercentage { get; set; }
    public decimal MaxScore { get; set; }
    public ICollection<CategoryResponse> Categories { get; set; }
}