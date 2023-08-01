using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Lead;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/leads")]
    public class LeadController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly ILogger<LeadController> _logger;

        public LeadController(ILeadService leadService, ILogger<LeadController> logger)
        {
            _leadService = leadService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeadsAsync([FromQuery] LeadSearchDto search)
        {
            var leadDtOs = await _leadService.GetLeadsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDtOs)}");
            return Ok(leadDtOs);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetLeadByIdAsync))]
        public async Task<IActionResult> GetLeadByIdAsync(Guid id)
        {
            var leadDto = await _leadService.GetLeadByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Lead: {JsonSerializer.Serialize(leadDto)}");
            return Ok(leadDto);
        }

        [HttpPost]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> CreateLeadAsync([FromBody] LeadCreateDto leadDto)
        {
            var leadCreateDto = await _leadService.CreateLeadAsync(leadDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Lead: {JsonSerializer.Serialize(leadCreateDto)}");
            return CreatedAtAction(nameof(GetLeadByIdAsync), new { id = leadCreateDto.Id }, leadCreateDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> UpdateLeadAsync(Guid id, [FromBody] LeadUpdateDto leadDto)
        {
            var leadUpdateDto = await _leadService.UpdateLeadAsync(id, leadDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Lead: {JsonSerializer.Serialize(leadUpdateDto)}");
            return Ok(leadUpdateDto);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> DeleteLeadAsync(Guid id)
        {
            await _leadService.DeleteLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Lead: {id}");
            return Ok("Successfully Deleted Lead");
        }

        [HttpPost("{id:guid}/qualify")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> QualifyLeadAsync(Guid id)
        {
            var dealDto = await _leadService.QualifyLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Qualified Lead: {JsonSerializer.Serialize(dealDto)}");
            return CreatedAtRoute(new { id = dealDto.Id, controller = "deal", action = nameof(DealController.GetDealByIdAsync) }, dealDto);
        }

        [HttpPost("{id:guid}/disqualify")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> DisqualifyLeadAsync(Guid id, [FromBody] DisqualifyDto disqualifyDto)
        {
            await _leadService.DisqualifyLeadAsync(id, disqualifyDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Disqualified Lead: {id}");
            return Ok("Successfully Disqualify Lead");
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetStatisticLeadAsync()
        {
            var leadStatisticDto = await _leadService.GetStatisticLeadAsync();
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Lead Statistic: {JsonSerializer.Serialize(leadStatisticDto)}");
            return Ok(leadStatisticDto);
        }

        [HttpGet("account/{id:guid}")]
        public async Task<IActionResult> GetLeadsByAccountIdAsync(Guid id, [FromQuery] LeadSearchDto search)
        {
            var leadDtOs = await _leadService.GetLeadsByAccountIdAsync(id, search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDtOs)}");
            return Ok(leadDtOs);
        }
    }
}