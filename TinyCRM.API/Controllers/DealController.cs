using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Helper.Filters;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealController : Controller
    {
        private readonly IDealService _dealService;
        private readonly IProductDealService _productDealService;
        private readonly ILogger<DealController> _logger;

        public DealController(IDealService dealService, IProductDealService productDealService, ILogger<DealController> logger)
        {
            _dealService = dealService;
            _productDealService = productDealService;
            _logger = logger;
        }

        [HttpGet]
        [SortFilterAttributeQuery(Filters = "Title")]
        public async Task<IActionResult> GetDealsAsync([FromQuery] DealSearchDTO search)
        {
            var dealDTO = await _dealService.GetDealsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDTO)}");
            return Ok(dealDTO);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetDealByIdAsync))]
        public async Task<IActionResult> GetDealByIdAsync(Guid id)
        {
            var dealDTO = await _dealService.GetDealByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deal: {JsonSerializer.Serialize(dealDTO)}");
            return Ok(dealDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealAsync(Guid id, [FromBody] DealUpdateDTO dealDTO)
        {
            var dealUpdateDTO = await _dealService.UpdateDealAsync(id, dealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Deal: {JsonSerializer.Serialize(dealUpdateDTO)}");
            return Ok(dealUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealAsync(Guid id)
        {
            await _dealService.DeleteDealAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Deal: {id}");
            return Ok("Successfully Deleted Deal");
        }

        [HttpGet("{id}/productdeals")]
        public async Task<IActionResult> GetProductDealsByDealIdAsync(Guid id)
        {
            IList<ProductDealDTO> productDealDTO = await _productDealService.GetProductDealsByDealIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product Deals: {JsonSerializer.Serialize(productDealDTO)}");
            return Ok(productDealDTO);
        }

        [HttpGet("{id}/productdeals/{productDealId}")]
        [ActionName(nameof(GetProductDealByIdAsync))]
        public async Task<IActionResult> GetProductDealByIdAsync(Guid id, Guid productDealId)
        {
            ProductDealDTO productDealDTO = await _productDealService.GetProductDealByIdAsync(id, productDealId);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product Deal: {JsonSerializer.Serialize(productDealDTO)}");
            return Ok(productDealDTO);
        }

        [HttpPost("{id}/productdeals")]
        public async Task<IActionResult> CreateProductDealAsync(Guid id, [FromBody] ProductDealCreateDTO productDealDTO)
        {
            ProductDealDTO productDealCreateDTO = await _productDealService.CreateProductDealAsync(id, productDealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Product Deal: {JsonSerializer.Serialize(productDealCreateDTO)}");
            return CreatedAtAction(nameof(GetProductDealByIdAsync), new { id = productDealCreateDTO.DealId, productDealId = productDealCreateDTO.Id }, productDealCreateDTO);
        }

        [HttpPut("{id}/productdeals/{productDealId}")]
        public async Task<IActionResult> UpdateProductDealAsync(Guid id, Guid productDealId, [FromBody] ProductDealUpdateDTO productDealDTO)
        {
            ProductDealDTO productDealUpdateDTO = await _productDealService.UpdateProductDealAsync(id, productDealId, productDealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Product Deal: {JsonSerializer.Serialize(productDealUpdateDTO)}");
            return Ok(productDealUpdateDTO);
        }

        [HttpDelete("{id}/productdeals/{productDealId}")]
        public async Task<IActionResult> DeleteProductDealAsync(Guid id, Guid productDealId)
        {
            await _productDealService.DeleteProductDealAsync(id, productDealId);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Product Deal: {productDealId}");
            return Ok("Successfully Deleted ProductDeal");
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetStatisticDealAsync()
        {
            var dealStatisticDTO = await _dealService.GetStatisticDealAsync();
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Statistic Deal: {JsonSerializer.Serialize(dealStatisticDTO)}");
            return Ok(dealStatisticDTO);
        }
    }
}