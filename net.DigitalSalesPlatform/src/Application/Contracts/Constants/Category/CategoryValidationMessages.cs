namespace Application.Contracts.Constants.Category;

public static class CategoryValidationMessages
{
    public const string NameRequired = "Name is required";
    public const string NameMaxLength = "Name is required and must be between 3 and 50 characters";
    public const string DescriptionMaxLength = "Description is required and must be between 3 and 500 characters";
    public const string UrlRequired = "Url is required";
    public const string UrlMaxLength = "Url is required and must be between 3 and 200 characters";
    public const string TagsRequired = "Tags is required";
    public const string TagsMaxLength = "Tags is required and must be between 3 and 200 characters";
    public const string SortOrderRequired = "SortOrder is required";
}