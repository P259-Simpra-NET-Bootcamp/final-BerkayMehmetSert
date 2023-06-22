using System.ComponentModel.DataAnnotations;
using Application.Contracts.Requests.Category;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult CreateCategory([FromBody] CreateCategoryRequest request)
    {
        _categoryService.CreateCategory(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCategory([FromRoute, Required] Guid id, [FromBody] UpdateCategoryRequest request)
    {
        _categoryService.UpdateCategory(id, request);
        return Ok();
    }
    
    
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateCategoryStatus([FromRoute, Required] Guid id,
        [FromQuery, Required] bool isActive)
    {
        _categoryService.UpdateCategoryStatus(id, isActive);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteCategory([FromRoute, Required] Guid id)
    {
        _categoryService.DeleteCategory(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCategoryById([FromRoute, Required] Guid id)
    {
        var category = _categoryService.GetCategoryById(id);
        return Ok(category);
    }

    [HttpGet("name/{name}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetCategoryByName([FromRoute, Required] string name)
    {
        var category = _categoryService.GetCategoryByName(name);
        return Ok(category);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllCategories()
    {
        var categories = _categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("sort-order")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllCategoriesOrderedBySortOrder()
    {
        var categories = _categoryService.GetAllCategoriesOrderedBySortOrder();
        return Ok(categories);
    }

    [HttpGet("sort-order-desc")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllCategoriesOrderedBySortOrderDescending()
    {
        var categories = _categoryService.GetAllCategoriesOrderedBySortOrderDescending();
        return Ok(categories);
    }
}