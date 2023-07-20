using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.Product;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchDTO search)
        {
            var productDTOs = await _productService.GetProductsAsync(search);
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var productDTO = await _productService.GetProductByIdAsync(id);
            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDTO ProductDTO)
        {
            var productNewDTO = await _productService.CreateProductAsync(ProductDTO);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = productNewDTO.Id }, productNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductUpdateDTO ProductDTO)
        {
            var productUpdateDTO = await _productService.UpdateProductAsync(id, ProductDTO);
            return Ok(productUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Successfully Deleted Product");
        }
    }
}
