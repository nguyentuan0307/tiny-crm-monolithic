using Microsoft.AspNetCore.Mvc;
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

        public DealController(IDealService dealService, IProductDealService productDealService)
        {
            _dealService = dealService;
            _productDealService = productDealService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDealsAsync([FromQuery] DealSearchDTO search)
        {
            var dealDTO = await _dealService.GetDealsAsync(search);
            return Ok(dealDTO);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetDealByIdAsync))]
        public async Task<IActionResult> GetDealByIdAsync(Guid id)
        {
            var dealDTO = await _dealService.GetDealByIdAsync(id);
            return Ok(dealDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealAsync(Guid id, [FromBody] DealUpdateDTO dealDTO)
        {
            var dealUpdateDTO = await _dealService.UpdateDealAsync(id, dealDTO);
            return Ok(dealUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealAsync(Guid id)
        {
            await _dealService.DeleteDealAsync(id);
            return Ok("Successfully Deleted Deal");
        }


        [HttpGet("{id}/productdeals")]
        public async Task<IActionResult> GetProductDealsByDealIdAsync(Guid id)
        {
            IList<ProductDealDTO> productDealDTO = await _productDealService.GetProductDealsByDealIdAsync(id);
            return Ok(productDealDTO);
        }

        [HttpGet("{id}/productdeals/{productDealId}")]
        [ActionName(nameof(GetProductDealByIdAsync))]
        public async Task<IActionResult> GetProductDealByIdAsync(Guid id,Guid productDealId)
        {
            ProductDealDTO productDealDTO = await _productDealService.GetProductDealByIdAsync(id, productDealId);
            return Ok(productDealDTO);
        }

        [HttpPost("{id}/productdeals")]
        public async Task<IActionResult> CreateProductDealAsync(Guid id, [FromBody] ProductDealCreateDTO productDealDTO)
        {
            ProductDealDTO productDealCreateDTO = await _productDealService.CreateProductDealAsync(id, productDealDTO);
            return CreatedAtAction(nameof(GetProductDealByIdAsync), new { id = productDealCreateDTO.DealId, productDealId=productDealCreateDTO.Id }, productDealCreateDTO);
        }

        [HttpPut("{id}/productdeals/{productDealId}")]
        public async Task<IActionResult> UpdateProductDealAsync(Guid id,Guid productDealId, [FromBody] ProductDealUpdateDTO productDealDTO)
        {
            ProductDealDTO productDealUpdateDTO = await _productDealService.UpdateProductDealAsync(id, productDealId, productDealDTO);
            return Ok(productDealUpdateDTO);
        }

        [HttpDelete("{id}/productdeals/{productDealId}")]
        public async Task<IActionResult> DeleteProductDealAsync(Guid id, Guid productDealId)
        {
            await _productDealService.DeleteProductDealAsync(id, productDealId);
            return Ok("Successfully Deleted ProductDeal");
        }
    }
}
