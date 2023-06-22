using Application.Contracts.Requests.Category;
using Application.Contracts.Responses.Category;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface ICategoryService
{
    void CreateCategory(CreateCategoryRequest request);
    void UpdateCategory(Guid id, UpdateCategoryRequest request);
    void UpdateCategoryStatus(Guid id, bool isActive);
    void DeleteCategory(Guid id);
    CategoryResponse GetCategoryById(Guid id);
    CategoryResponse GetCategoryByName(string name);
    List<CategoryResponse> GetAllCategories();
    List<CategoryResponse> GetAllCategoriesOrderedBySortOrder();
    List<CategoryResponse> GetAllCategoriesOrderedBySortOrderDescending();
    Category GetActiveCategoryEntity(Guid id);
}