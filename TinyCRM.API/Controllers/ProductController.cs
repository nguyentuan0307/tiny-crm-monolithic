using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Serilog;
using TinyCRM.Application.Models.Product;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers;

[Authorize]
[ApiController]
[Route("api/products")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchDto search)
    {
        var productDtOs = await _productService.GetProductsAsync(search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Products: {JsonSerializer.Serialize(productDtOs)}");
        return Ok(productDtOs);
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetProductAsync))]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var productDto = await _productService.GetProductAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Product: {JsonSerializer.Serialize(productDto)}");
        return Ok(productDto);
    }

    [HttpPost]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDto productDto)
    {
        var productNewDto = await _productService.CreateProductAsync(productDto);
        Log.Information($"[{DateTime.Now}]Successfully Created Product: {JsonSerializer.Serialize(productNewDto)}");
        return CreatedAtAction(nameof(GetProductAsync), new { id = productNewDto.Id }, productNewDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductUpdateDto productDto)
    {
        var productUpdateDto = await _productService.UpdateProductAsync(id, productDto);
        Log.Information($"[{DateTime.Now}]Successfully Updated Product: {JsonSerializer.Serialize(productUpdateDto)}");
        return Ok(productUpdateDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Deleted Product: {id}");
        return Ok("Successfully Deleted Product");
    }
}