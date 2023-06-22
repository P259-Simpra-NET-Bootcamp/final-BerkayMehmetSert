using Application.Contracts.Constants.Category;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Category;
using Application.Contracts.Responses.Category;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public void CreateCategory(CreateCategoryRequest request)
    {
        CheckIfCategoryExistsByName(request.Name);

        var category = _mapper.Map<Category>(request);
        category.IsActive = true;
        
        _categoryRepository.Add(category);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        var category = GetActiveCategoryEntity(id);

        if (!string.Equals(category.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            CheckIfCategoryExistsByName(request.Name);

        var updatedCategory = _mapper.Map(request, category);

        _categoryRepository.Update(updatedCategory);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCategoryStatus(Guid id, bool isActive)
    {
        var category = GetCategoryEntity(id);
        category.IsActive = isActive;
        _categoryRepository.Update(category);
        _unitOfWork.SaveChanges();
    }

    public void DeleteCategory(Guid id)
    {
        var category = GetCategoryEntity(id);

        if (category.ProductCategories is not null && category.ProductCategories.Any())
            throw new BusinessException(CategoryBusinessMessages.CategoryHasProducts);

        _categoryRepository.Delete(category);
        _unitOfWork.SaveChanges();
    }

    public CategoryResponse GetCategoryById(Guid id)
    {
        var category = GetActiveCategoryEntity(id);
        return _mapper.Map<CategoryResponse>(category);
    }

    public CategoryResponse GetCategoryByName(string name)
    {
        var category = _categoryRepository.Get(
            predicate: x => x.Name.Equals(name),
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Product)
        );

        if (category is null)
            throw new NotFoundException(CategoryBusinessMessages.CategoryNotFoundByName);
        if (!category.IsActive)
            throw new BusinessException(CategoryBusinessMessages.CategoryIsNotActive);

        return _mapper.Map<CategoryResponse>(category);
    }

    public List<CategoryResponse> GetAllCategories()
    {
        var categories = _categoryRepository.GetAll(
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Product)
        );

        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public List<CategoryResponse> GetAllCategoriesOrderedBySortOrder()
    {
        var categories = _categoryRepository.GetAll(
            orderBy: source => source.OrderBy(x => x.SortOrder),
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Product)
        );

        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public List<CategoryResponse> GetAllCategoriesOrderedBySortOrderDescending()
    {
        var categories = _categoryRepository.GetAll(
            orderBy: source => source.OrderByDescending(x => x.SortOrder)
        );

        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public Category GetActiveCategoryEntity(Guid id)
    {
        var category = GetCategoryEntity(id);
        if (!category.IsActive)
            throw new BusinessException(CategoryBusinessMessages.CategoryIsNotActive);
        return category;
    }

    private void CheckIfCategoryExistsByName(string name)
    {
        var category = _categoryRepository.Get(predicate: x => x.Name.Equals(name));
        if (category is not null)
            throw new BusinessException(CategoryBusinessMessages.CategoryAlreadyExistsByName);
    }

    private Category GetCategoryEntity(Guid id)
    {
        var category = _categoryRepository.Get(
            predicate: x => x.Id.Equals(id),
            include: source => source
                .Include(x => x.ProductCategories)
        );
        
        if (category is null)
            throw new NotFoundException(CategoryBusinessMessages.CategoryNotFoundById);
        
        return category;
    }
}