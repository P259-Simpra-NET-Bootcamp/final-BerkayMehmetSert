using Core.Application.Request;

namespace Application.Contracts.Requests.Category;

public class UpdateCategoryRequest : BaseRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
    public int SortOrder { get; set; }
}