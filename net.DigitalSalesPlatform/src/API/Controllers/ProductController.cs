using System.ComponentModel.DataAnnotations;
using Application.Contracts.Requests.Product;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateProduct([FromBody] CreateProductRequest request)
    {
        _productService.CreateProduct(request);
        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProduct([FromRoute, Required] Guid id, [FromBody] UpdateProductRequest request)
    {
        _productService.UpdateProduct(id, request);
        return Ok();
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProductStatus([FromRoute, Required] Guid id,
        [FromQuery, Required] bool isActive)
    {
        _productService.UpdateProductStatus(id, isActive);
        return Ok();
    }

    [HttpPatch("{id}/price")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProductPrice([FromRoute, Required] Guid id,
        [FromBody] UpdateProductPriceRequest request)
    {
        _productService.UpdateProductPrice(id, request);
        return Ok();
    }

    [HttpPatch("{id}/categories")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProductCategories([FromRoute, Required] Guid id,
        [FromBody] UpdateProductChangeCategoriesRequest request)
    {
        _productService.UpdateProductCategories(id, request);
        return Ok();
    }

    [HttpPatch("{id}/stock/increment")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProductStockIncrement([FromRoute, Required] Guid id,
        [FromQuery, Required, Range(1, int.MaxValue)]
        int stock)
    {
        _productService.UpdateProductStockIncrement(id, stock);
        return Ok();
    }

    [HttpPatch("{id}/stock/decrement")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProductStockDecrement([FromRoute, Required] Guid id,
        [FromQuery, Required, Range(1, int.MaxValue)] int stock)
    {
        _productService.UpdateProductStockDecrement(id, stock);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteProduct([FromRoute, Required] Guid id)
    {
        _productService.DeleteProduct(id);
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetProductById([FromRoute, Required] Guid id)
    {
        var product = _productService.GetProductById(id);
        return Ok(product);
    }

    [HttpGet("category/{categoryId}")]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetProductsByCategoryId([FromRoute, Required] Guid categoryId)
    {
        var products = _productService.GetProductsByCategoryId(categoryId);
        return Ok(products);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }
}