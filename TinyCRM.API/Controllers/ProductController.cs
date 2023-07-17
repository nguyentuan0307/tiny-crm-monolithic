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
            List<ProductDTO> productDTOs = await _productService.GetProductsAsync(search);
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<IActionResult> GetProductByIdAsync(string id)
        {
            ProductDTO productDTO = await _productService.GetProductByIdAsync(id);
            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductCreateDTO ProductDTO)
        {
            ProductDTO productNewDTO = await _productService.CreateProductAsync(ProductDTO);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = productNewDTO.Id }, productNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(string id, ProductUpdateDTO ProductDTO)
        {
            ProductDTO productUpdateDTO = await _productService.UpdateProductAsync(id, ProductDTO);
            return Ok(productUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(string id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Successfully Deleted Product");
        }
    }
}
