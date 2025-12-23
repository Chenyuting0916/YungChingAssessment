using AutoMapper;
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
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        var dto = _mapper.Map<ProductDto>(product);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto createDto)
    {
        var product = _mapper.Map<Product>(createDto);
        var createdProduct = await _productService.CreateProductAsync(product);
        var dto = _mapper.Map<ProductDto>(createdProduct);

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto updateDto)
    {
        var product = _mapper.Map<Product>(updateDto);
        await _productService.UpdateProductAsync(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/price-details")]
    public async Task<ActionResult<object>> GetDiscountPrice(int id)
    {
        var details = await _productService.GetPriceDetailsAsync(id);
        
        return Ok(new 
        { 
            ProductPrice = details.OriginalPrice, 
            FinalPrice = details.FinalPrice, 
            HasDiscount = details.HasDiscount 
        });
    }
}
