using Microsoft.AspNetCore.Mvc;
using YungChingAssessment.Api.DTOs;
using YungChingAssessment.Core.Entities;
using YungChingAssessment.Core.Interfaces;

namespace YungChingAssessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            IsActive = p.IsActive,
            CategoryName = p.Category?.Name ?? "Unknown" // Note: Simplify, might be null if not included
        });
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive,
            CategoryName = product.Category?.Name ?? "Unknown"
        });
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto createDto)
    {
        var product = new Product
        {
            Name = createDto.Name,
            Price = createDto.Price,
            IsActive = createDto.IsActive,
            CategoryId = createDto.CategoryId
        };

        var createdProduct = await _productService.CreateProductAsync(product);

        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, new ProductDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Price = createdProduct.Price,
            IsActive = createdProduct.IsActive,
            CategoryName = "Unknown" // Fetching category needs extra call or include
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto updateDto)
    {
        var product = new Product
        {
            Name = updateDto.Name,
            Price = updateDto.Price,
            IsActive = updateDto.IsActive,
            CategoryId = updateDto.CategoryId
        };

        await _productService.UpdateProductAsync(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}
