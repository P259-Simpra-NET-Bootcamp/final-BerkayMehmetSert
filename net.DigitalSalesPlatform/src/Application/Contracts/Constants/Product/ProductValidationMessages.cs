namespace Application.Contracts.Constants.Product;

public static class ProductValidationMessages
{
    public const string NameRequired = "Name is required";
    public const string NameLength = "Name is required and must be between 3 and 250 characters";
    public const string DescriptionRequired = "Description is required";
    public const string DescriptionLength = "Description is required and must be between 3 and 250 characters";
    public const string FeaturesRequired = "Features is required";
    public const string FeaturesLength = "Features is required and must be between 3 and 250 characters";
    public const string PriceRequired = "Price is required";
    public const string PriceGreaterThanZero = "Price must be greater than zero";
    public const string StockRequired = "Stock is required";
    public const string StockGreaterThanZero = "Stock must be greater than zero";
    public const string CategoryIdsRequired = "CategoryIds is required";
    public const string StatusRequired = "Status is required";
}