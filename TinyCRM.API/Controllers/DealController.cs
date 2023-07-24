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
        public async Task<IActionResult> GetDealsAsync([FromQuery] DealSearchDTO search)
        {
            var dealDTO = await _dealService.GetDealsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDTO)}");
            return Ok(dealDTO);
        }

        [HttpGet("{dealid}")]
        [ActionName(nameof(GetDealByIdAsync))]
        public async Task<IActionResult> GetDealByIdAsync(Guid dealId)
        {
            var dealDTO = await _dealService.GetDealByIdAsync(dealId);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deal: {JsonSerializer.Serialize(dealDTO)}");
            return Ok(dealDTO);
        }

        [HttpPut("{dealid}")]
        public async Task<IActionResult> UpdateDealAsync(Guid dealId, [FromBody] DealUpdateDTO dealDTO)
        {
            var dealUpdateDTO = await _dealService.UpdateDealAsync(dealId, dealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Deal: {JsonSerializer.Serialize(dealUpdateDTO)}");
            return Ok(dealUpdateDTO);
        }

        [HttpDelete("{dealid}")]
        public async Task<IActionResult> DeleteDealAsync(Guid dealId)
        {
            await _dealService.DeleteDealAsync(dealId);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Deal: {dealId}");
            return Ok("Successfully Deleted Deal");
        }

        [HttpGet("{dealid}/productdeals")]
        public async Task<IActionResult> GetProductDealsByDealIdAsync(Guid dealId, [FromQuery]ProductDealSearchDTO search)
        {
            var productDealDTO = await _productDealService.GetProductDealsByDealIdAsync(dealId, search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product Deals: {JsonSerializer.Serialize(productDealDTO)}");
            return Ok(productDealDTO);
        }

        [HttpGet("{dealid}/productdeals/{productDealId}")]
        [ActionName(nameof(GetProductDealByIdAsync))]
        public async Task<IActionResult> GetProductDealByIdAsync(Guid dealId, Guid productDealId)
        {
            var productDealDTO = await _productDealService.GetProductDealByIdAsync(dealId, productDealId);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Product Deal: {JsonSerializer.Serialize(productDealDTO)}");
            return Ok(productDealDTO);
        }

        [HttpPost("{dealid}/productdeals")]
        public async Task<IActionResult> CreateProductDealAsync(Guid dealId, [FromBody] ProductDealCreateDTO productDealDTO)
        {
            var productDealCreateDTO = await _productDealService.CreateProductDealAsync(dealId, productDealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Product Deal: {JsonSerializer.Serialize(productDealCreateDTO)}");
            return CreatedAtAction(nameof(GetProductDealByIdAsync), new { id = productDealCreateDTO.DealId, productDealId = productDealCreateDTO.Id }, productDealCreateDTO);
        }

        [HttpPut("{dealid}/productdeals/{productDealId}")]
        public async Task<IActionResult> UpdateProductDealAsync(Guid dealId, Guid productDealId, [FromBody] ProductDealUpdateDTO productDealDTO)
        {
            var productDealUpdateDTO = await _productDealService.UpdateProductDealAsync(dealId, productDealId, productDealDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Product Deal: {JsonSerializer.Serialize(productDealUpdateDTO)}");
            return Ok(productDealUpdateDTO);
        }

        [HttpDelete("{dealid}/productdeals/{productDealId}")]
        public async Task<IActionResult> DeleteProductDealAsync(Guid dealId, Guid productDealId)
        {
            await _productDealService.DeleteProductDealAsync(dealId, productDealId);
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