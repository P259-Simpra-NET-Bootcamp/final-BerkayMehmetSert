using Application.Contracts.Constants.Product;
using Application.Contracts.Repositories;
using Application.Contracts.Requests.Product;
using Application.Contracts.Responses.Product;
using Application.Contracts.Services;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryService _categoryService;
    private readonly IPointCalculationService _pointCalculationService;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ICategoryService categoryService,
        IPointCalculationService pointCalculationService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _categoryService = categoryService;
        _pointCalculationService = pointCalculationService;
        _mapper = mapper;
    }

    public void CreateProduct(CreateProductRequest request)
    {
        var categories = GetCategories(request.CategoryIds);

        var product = _mapper.Map<Product>(request);
        product.ProductCategories = MapProductCategories(categories, product.Id);

        CalculateProductScore(product);

        _productRepository.Add(product);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProduct(Guid id, UpdateProductRequest request)
    {
        var product = GetActiveProductEntity(id);
        var updatedProduct = _mapper.Map(request, product);

        _productRepository.Update(updatedProduct);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProductStatus(Guid id, bool isActive)
    {
        var product = GetProductEntity(id);
        product.IsActive = isActive;

        _productRepository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProductPrice(Guid id, UpdateProductPriceRequest request)
    {
        var product = GetActiveProductEntity(id);
        product.Price = request.Price;

        CalculateProductScore(product);

        _productRepository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProductCategories(Guid id, UpdateProductChangeCategoriesRequest request)
    {
        var product = GetActiveProductEntity(id);
        var categories = GetCategories(request.CategoryIds);

        product.ProductCategories?.Clear();
        product.ProductCategories = MapProductCategories(categories, product.Id);

        _productRepository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProductStockIncrement(Guid id, int stock)
    {
        var product = GetActiveProductEntity(id);
        product.Stock += stock;

        _productRepository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void UpdateProductStockDecrement(Guid id, int stock)
    {
        var product = GetActiveProductEntity(id);
        product.Stock -= stock;

        _productRepository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void DeleteProduct(Guid id)
    {
        var product = GetProductEntity(id);
        _productRepository.Delete(product);
        _unitOfWork.SaveChanges();
    }

    public ProductResponse GetProductById(Guid id)
    {
        var product = GetActiveProductEntity(id);
        return _mapper.Map<ProductResponse>(product);
    }

    public List<ProductResponse> GetProductsByCategoryId(Guid categoryId)
    {
        var products = _productRepository.GetAll(
            predicate: x =>
                x.ProductCategories.Any(pc => pc.CategoryId.Equals(categoryId) && pc.Category.IsActive),
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
        );
        return _mapper.Map<List<ProductResponse>>(products);
    }

    public List<ProductResponse> GetAllProducts()
    {
        var products = _productRepository.GetAll(
            x => x.IsActive,
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
        );
        return _mapper.Map<List<ProductResponse>>(products);
    }

    public Product GetActiveProductEntity(Guid id)
    {
        var product = GetProductEntity(id);
        if (!product.IsActive)
            throw new BusinessException(ProductBusinessMessages.ProductIsNotActive);
        return product;
    }

    private Product GetProductEntity(Guid id)
    {
        var product = _productRepository.Get(
            predicate: x => x.Id.Equals(id),
            include: source => source
                .Include(x => x.ProductCategories)
                .ThenInclude(x => x.Category)
        );

        if (product is null)
            throw new NotFoundException(ProductBusinessMessages.ProductNotFoundById);

        return product;
    }

    private List<Category> GetCategories(ICollection<Guid> categoryIds)
    {
        var categories = new List<Category>();

        if (categoryIds is not null)
        {
            categories.AddRange(categoryIds.Select(categoryId => _categoryService.GetActiveCategoryEntity(categoryId)));
        }

        return categories;
    }

    private static List<ProductCategoryMap> MapProductCategories(List<Category> categories, Guid productId)
    {
        return categories.Select(category => new ProductCategoryMap
        {
            CategoryId = category.Id,
            ProductId = productId
        }).ToList();
    }

    private void CalculateProductScore(Product product)
    {
        product.ScorePercentage = _pointCalculationService.CalculateScorePercentage(product.Price);
        product.MaxScore = _pointCalculationService.CalculateMaxScore(product.Price, product.ScorePercentage);
    }
}