﻿using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.Product;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductController(IProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchDTO search)
        {
            var productDTOs = await _productService.GetProductsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Products");
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetProductByIdAsync))]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var productDTO = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product");
            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDTO ProductDTO)
        {
            var productNewDTO = await _productService.CreateProductAsync(ProductDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Product");
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = productNewDTO.Id }, productNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductUpdateDTO ProductDTO)
        {
            var productUpdateDTO = await _productService.UpdateProductAsync(id, ProductDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Product");
            return Ok(productUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Product");
            return Ok("Successfully Deleted Product");
        }
    }
}
