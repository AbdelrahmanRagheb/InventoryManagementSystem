using InventoryManagementSystem.Application.DTOs.Products;
using InventoryManagementSystem.Application.Services;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(ToResponse(product));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CategoryId = request.CategoryId,
            Price = request.Price,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        try
        {
            await _productService.AddAsync(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, ToResponse(product));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        if (request.Name != null) product.Name = request.Name;
        if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId.Value;
        if (request.Price.HasValue) product.Price = request.Price.Value;
        if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;
        try
        {
            await _productService.UpdateAsync(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        return Ok(ToResponse(product));
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateProduct(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound(new { message = $"Product with id: {id} does not exist" });
        await _productService.DeactivateAsync(id);
        return Ok(ToResponse(product));
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var product = await _productService.ActivateAsync(id);
        if (product == null) return NotFound(new { message = $"Product with id: {id} does not exist" });
        return Ok(ToResponse(product));
    }

    private static ProductResponse ToResponse(Product product) =>
        new(product.Id, product.Name, product.CategoryId, product.Price, product.IsActive, product.CreatedAt);
}