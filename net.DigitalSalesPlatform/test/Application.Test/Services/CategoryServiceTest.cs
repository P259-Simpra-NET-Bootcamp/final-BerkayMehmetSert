using Application.Contracts.Constants.Category;
using Application.Contracts.Requests.Category;
using Application.Contracts.Validations.Category;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Xunit;

namespace Application.Test.Services;

public class CategoryServiceTest : CategoryMockRepository
{
    private readonly CategoryService _categoryService;
    private readonly CreateCategoryRequestValidator _categoryRequestValidator;
    private readonly UpdateCategoryRequestValidator _updateCategoryRequestValidator;

    public CategoryServiceTest(
        CategoryFakeData categoryFakeData,
        CreateCategoryRequestValidator categoryRequestValidator,
        UpdateCategoryRequestValidator updateCategoryRequestValidator) : base(categoryFakeData)
    {
        _categoryRequestValidator = categoryRequestValidator;
        _updateCategoryRequestValidator = updateCategoryRequestValidator;
        _categoryService = new CategoryService(MockRepository.Object, UnitOfWork.Object, Mapper);
    }

    [Fact]
    public void CreateCategory_ValidRequest_ShouldReturnSuccess()
    {
        var request = new CreateCategoryRequest { Name = "Category Test" };
        _categoryService.CreateCategory(request);
        var category = _categoryService.GetCategoryByName(request.Name);
        Assert.NotNull(category);
        Assert.Equal(request.Name, category.Name);
    }

    [Fact]
    public void CreateCategoryValidRequestShouldThrowCategoryNameAlreadyExistsException()
    {
        var request = new CreateCategoryRequest { Name = "Category 1" };
        var exception = Assert.Throws<BusinessException>(() => _categoryService.CreateCategory(request));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void CreateCategoryInValidRequestShouldThrowValidationException()
    {
        var request = new CreateCategoryRequest { Name = "" };
        var result = _categoryRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(CategoryValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateCategoryValidRequestShouldReturnSuccess()
    {
        var request = new UpdateCategoryRequest { Name = "Category Test" };
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        _categoryService.UpdateCategory(id, request);
        var category = _categoryService.GetCategoryByName(request.Name);
        Assert.NotNull(category);
        Assert.Equal(request.Name, category.Name);
    }

    [Fact]
    public void UpdateCategoryValidRequestShouldThrowCategoryIsNotActiveException()
    {
        var request = new UpdateCategoryRequest { Name = "Category Test" };
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var exception = Assert.Throws<BusinessException>(() => _categoryService.UpdateCategory(id, request));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryIsNotActive, exception.Message);
    }
    
    [Fact]
    public void UpdateCategoryValidRequestShouldThrowCategoryNameAlreadyExistsException()
    {
        var request = new UpdateCategoryRequest { Name = "Category 1" };
        var id = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _categoryService.UpdateCategory(id, request));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryAlreadyExistsByName, exception.Message);
    }
    
    [Fact]
    public void UpdateCategoryValidRequestShouldThrowCategoryNotFoundException()
    {
        var request = new UpdateCategoryRequest { Name = "Category Test" };
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.UpdateCategory(id, request));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateCategoryInValidRequestShouldThrowValidationException()
    {
        var request = new UpdateCategoryRequest { Name = "" };
        var result = _updateCategoryRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage).FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(CategoryValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateCategoryStatusValidRequestShouldReturnSuccess()
    {
        const bool isActive = true;
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        _categoryService.UpdateCategoryStatus(id, isActive);
        var category = _categoryService.GetCategoryById(id);
        Assert.NotNull(category);
        Assert.Equal(isActive, category.IsActive);
    }
    
    [Fact]
    public void UpdateCategoryStatusValidRequestShouldThrowCategoryNotFoundException()
    {
        const bool isActive = true;
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.UpdateCategoryStatus(id, isActive));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundById, exception.Message);
    }

    [Fact]
    public void DeleteCategoryValidRequestShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        _categoryService.DeleteCategory(id);
        Assert.Throws<NotFoundException>(() => _categoryService.GetCategoryById(id));
    }
    
    [Fact]
    public void DeleteCategoryValidRequestShouldThrowCategoryNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.DeleteCategory(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundById, exception.Message);
    }
    
    [Fact]
    public void DeleteCategoryValidRequestShouldThrowCategoryHasProductsException()
    {
        var id = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _categoryService.DeleteCategory(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryHasProducts, exception.Message);
    }

    [Fact]
    public void GetCategoryByIdShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var category = _categoryService.GetCategoryById(id);
        Assert.NotNull(category);
        Assert.Equal(id, category.Id);
    }
    
    [Fact]
    public void GetCategoryByIdShouldThrowCategoryNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.GetCategoryById(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetCategoryByIdShouldThrowCategoryIsNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var exception = Assert.Throws<BusinessException>(() => _categoryService.GetCategoryById(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryIsNotActive, exception.Message);
    }
    
    [Fact]
    public void GetCategoryByNameShouldReturnSuccess()
    {
        const string name = "Category 1";
        var category = _categoryService.GetCategoryByName(name);
        Assert.NotNull(category);
        Assert.Equal(name, category.Name);
    }
    
    [Fact]
    public void GetCategoryByNameShouldThrowCategoryNotFoundException()
    {
        const string name = "Category Test";
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.GetCategoryByName(name));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundByName, exception.Message);
    }
    
    [Fact]
    public void GetCategoryByNameShouldThrowCategoryIsNotActiveException()
    {
        const string name = "Category 3";
        var exception = Assert.Throws<BusinessException>(() => _categoryService.GetCategoryByName(name));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryIsNotActive, exception.Message);
    }
    
    [Fact]
    public void GetAllCategoriesShouldReturnSuccess()
    {
        var categories = _categoryService.GetAllCategories();
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
    }
    
    [Fact]
    public void GetAllCategoriesOrderedBySortOrderShouldReturnSuccess()
    {
        var categories = _categoryService.GetAllCategoriesOrderedBySortOrder();
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
        Assert.Equal(1, categories[0].SortOrder);
        Assert.Equal(2, categories[1].SortOrder);
        Assert.Equal(3, categories[2].SortOrder);
    }
    
    [Fact]
    public void GetAllCategoriesOrderedBySortOrderDescendingShouldReturnSuccess()
    {
        var categories = _categoryService.GetAllCategoriesOrderedBySortOrderDescending();
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
        Assert.Equal(3, categories[0].SortOrder);
        Assert.Equal(2, categories[1].SortOrder);
        Assert.Equal(1, categories[2].SortOrder);
    }
    
    [Fact]
    public void GetActiveCategoryEntityShouldReturnSuccess()
    {
        var id = new Guid("11111111-1111-1111-1111-111111111111");
        var category = _categoryService.GetActiveCategoryEntity(id);
        Assert.NotNull(category);
        Assert.Equal("Category 1", category.Name);
    }
    
    [Fact]
    public void GetActiveCategoryEntityShouldThrowCategoryNotFoundException()
    {
        var id = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _categoryService.GetActiveCategoryEntity(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetActiveCategoryEntityShouldThrowCategoryIsNotActiveException()
    {
        var id = new Guid("33333333-3333-3333-3333-333333333333");
        var exception = Assert.Throws<BusinessException>(() => _categoryService.GetActiveCategoryEntity(id));
        Assert.NotNull(exception);
        Assert.Equal(CategoryBusinessMessages.CategoryIsNotActive, exception.Message);
    }
}