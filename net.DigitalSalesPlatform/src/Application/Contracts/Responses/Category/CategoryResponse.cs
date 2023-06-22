using Core.Application.Response;

namespace Application.Contracts.Responses.Category;

public class CategoryResponse : BaseResponse
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}