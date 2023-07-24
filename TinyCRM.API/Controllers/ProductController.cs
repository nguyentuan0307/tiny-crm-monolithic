using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Product;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchDto search)
        {
            var productDtOs = await _productService.GetProductsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Products: {JsonSerializer.Serialize(productDtOs)}");
            return Ok(productDtOs);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product: {JsonSerializer.Serialize(productDto)}");
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDto productDto)
        {
            var productNewDto = await _productService.CreateProductAsync(productDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Product: {JsonSerializer.Serialize(productNewDto)}");
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = productNewDto.Id }, productNewDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductUpdateDto productDto)
        {
            var productUpdateDto = await _productService.UpdateProductAsync(id, productDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Product: {JsonSerializer.Serialize(productUpdateDto)}");
            return Ok(productUpdateDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Product: {id}");
            return Ok("Successfully Deleted Product");
        }
    }
}