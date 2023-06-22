namespace Application.Contracts.Constants.Category;

public static class CategoryBusinessMessages
{
    public const string CategoryNotFoundById = "Category not found with the given ID.";
    public const string CategoryAlreadyExistsByName = "A category with the same name already exists.";
    public const string CategoryNotFoundByName = "No category found with the specified name.";
    public const string CategoryIsNotActive = "The category is not active.";

    public const string CategoryHasProducts =
        "The category cannot be deleted because there are products associated with the specified category ID.";
}