using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using TinyCRM.Application.Models.Lead;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/leads")]
public class LeadController : Controller
{
    private readonly ILeadService _leadService;

    public LeadController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    [HttpGet]
    [Authorize(Policy = TinyCrmPermissions.Leads.Read)]
    public async Task<IActionResult> GetLeadsAsync([FromQuery] LeadSearchDto search)
    {
        var leadDtOs = await _leadService.GetLeadsAsync(search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDtOs)}");
        return Ok(leadDtOs);
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetLeadAsync))]
    [Authorize(Policy = TinyCrmPermissions.Leads.Read)]
    public async Task<IActionResult> GetLeadAsync(Guid id)
    {
        var leadDto = await _leadService.GetLeadAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Lead: {JsonSerializer.Serialize(leadDto)}");
        return Ok(leadDto);
    }

    [HttpPost]
    [Authorize(Policy = TinyCrmPermissions.Leads.Create)]
    public async Task<IActionResult> CreateLeadAsync([FromBody] LeadCreateDto leadDto)
    {
        var leadCreateDto = await _leadService.CreateLeadAsync(leadDto);
        Log.Information($"[{DateTime.Now}]Successfully Created Lead: {JsonSerializer.Serialize(leadCreateDto)}");
        return CreatedAtAction(nameof(GetLeadAsync), new { id = leadCreateDto.Id }, leadCreateDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Edit)]
    public async Task<IActionResult> UpdateLeadAsync(Guid id, [FromBody] LeadUpdateDto leadDto)
    {
        var leadUpdateDto = await _leadService.UpdateLeadAsync(id, leadDto);
        Log.Information($"[{DateTime.Now}]Successfully Updated Lead: {JsonSerializer.Serialize(leadUpdateDto)}");
        return Ok(leadUpdateDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Delete)]
    public async Task<IActionResult> DeleteLeadAsync(Guid id)
    {
        await _leadService.DeleteLeadAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Deleted Lead: {id}");
        return Ok("Successfully Deleted Lead");
    }

    [HttpPost("{id:guid}/qualify")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Edit)]
    public async Task<IActionResult> QualifyLeadAsync(Guid id)
    {
        var dealDto = await _leadService.QualifyLeadAsync(id);
        Log.Information($"[{DateTime.Now}]Successfully Qualified Lead: {JsonSerializer.Serialize(dealDto)}");
        return CreatedAtRoute(new { id = dealDto.Id, controller = "deal", action = nameof(DealController.GetDealAsync) }, dealDto);
    }

    [HttpPost("{id:guid}/disqualify")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Edit)]
    public async Task<IActionResult> DisqualifyLeadAsync(Guid id, [FromBody] DisqualifyDto disqualifyDto)
    {
        await _leadService.DisqualifyLeadAsync(id, disqualifyDto);
        Log.Information($"[{DateTime.Now}]Successfully Disqualified Lead: {id}");
        return Ok("Successfully Disqualify Lead");
    }

    [HttpGet("statistic")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Read)]
    public async Task<IActionResult> GetStatisticLeadAsync()
    {
        var leadStatisticDto = await _leadService.GetStatisticLeadAsync();
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Lead Statistic: {JsonSerializer.Serialize(leadStatisticDto)}");
        return Ok(leadStatisticDto);
    }

    [HttpGet("account/{accountId:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Leads.Read)]
    public async Task<IActionResult> GetLeadsAsync(Guid accountId, [FromQuery] LeadSearchDto search)
    {
        var leadDtOs = await _leadService.GetLeadsAsync(accountId, search);
        Log.Information($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDtOs)}");
        return Ok(leadDtOs);
    }
}