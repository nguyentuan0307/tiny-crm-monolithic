using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Serilog;
using TinyCRM.Application.Models.Deal;
using TinyCRM.Application.Models.ProductDeal;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers;

[Authorize]
[ApiController]
[Route("api/deals")]
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
    public async Task<IActionResult> GetDealsAsync([FromQuery] DealSearchDto search)
    {
        var dealDto = await _dealService.GetDealsAsync(search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDto)}");
        return Ok(dealDto);
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetDealAsync))]
    public async Task<IActionResult> GetDealAsync(Guid id)
    {
        var dealDto = await _dealService.GetDealAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Deal: {JsonSerializer.Serialize(dealDto)}");
        return Ok(dealDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> UpdateDealAsync(Guid id, [FromBody] DealUpdateDto dealDto)
    {
        var dealUpdateDto = await _dealService.UpdateDealAsync(id, dealDto);
        Log.Information($"[{DateTime.Now}]Successfully Updated Deal: {JsonSerializer.Serialize(dealUpdateDto)}");
        return Ok(dealUpdateDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> DeleteDealAsync(Guid id)
    {
        await _dealService.DeleteDealAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Deleted Deal: {id}");
        return Ok("Successfully Deleted Deal");
    }

    [HttpGet("{dealId:guid}/product-deals")]
    public async Task<IActionResult> GetProductDealsByDealIdAsync(Guid dealId, [FromQuery] ProductDealSearchDto search)
    {
        var productDealDto = await _productDealService.GetProductDealsByDealIdAsync(dealId, search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Product Deals: {JsonSerializer.Serialize(productDealDto)}");
        return Ok(productDealDto);
    }

    [HttpGet("{dealId:guid}/product-deals/{productDealId}")]
    [ActionName(nameof(GetProductDealByIdAsync))]
    public async Task<IActionResult> GetProductDealByIdAsync(Guid dealId, Guid productDealId)
    {
        var productDealDto = await _productDealService.GetProductDealByIdAsync(dealId, productDealId);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Product Deal: {JsonSerializer.Serialize(productDealDto)}");
        return Ok(productDealDto);
    }

    [HttpPost("{dealId:guid}/product-deals")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> CreateProductDealAsync(Guid dealId, [FromBody] ProductDealCreateDto productDealDto)
    {
        var productDealCreateDto = await _productDealService.CreateProductDealAsync(dealId, productDealDto);
        Log.Information($"[{DateTime.Now}]Successfully Created Product Deal: {JsonSerializer.Serialize(productDealCreateDto)}");
        return CreatedAtAction(nameof(GetProductDealByIdAsync), new { id = productDealCreateDto.DealId, productDealId = productDealCreateDto.Id }, productDealCreateDto);
    }

    [HttpPut("{dealId:guid}/product-deals/{productDealId}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> UpdateProductDealAsync(Guid dealId, Guid productDealId, [FromBody] ProductDealUpdateDto productDealDto)
    {
        var productDealUpdateDto = await _productDealService.UpdateProductDealAsync(dealId, productDealId, productDealDto);
        Log.Information($"[{DateTime.Now}]Successfully Updated Product Deal: {JsonSerializer.Serialize(productDealUpdateDto)}");
        return Ok(productDealUpdateDto);
    }

    [HttpDelete("{dealId:guid}/product-deals/{productDealId}")]
    [Authorize(Policy = Policy.AdminPolicy)]
    public async Task<IActionResult> DeleteProductDealAsync(Guid dealId, Guid productDealId)
    {
        await _productDealService.DeleteProductDealAsync(dealId, productDealId);
        Log.Information($"[{DateTime.Now}]Successfully Deleted Product Deal: {productDealId}");
        return Ok("Successfully Deleted ProductDeal");
    }

    [HttpGet("statistic")]
    public async Task<IActionResult> GetStatisticDealAsync()
    {
        var dealStatisticDto = await _dealService.GetStatisticDealAsync();
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Statistic Deal: {JsonSerializer.Serialize(dealStatisticDto)}");
        return Ok(dealStatisticDto);
    }

    [HttpGet("account/{accountId:guid}")]
    public async Task<IActionResult> GetDealsAsync(Guid accountId, [FromQuery] DealSearchDto search)
    {
        var dealDtOs = await _dealService.GetDealsAsync(accountId, search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDtOs)}");
        return Ok(dealDtOs);
    }
}