using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : Controller
    {
        private readonly ILeadService _leadServicce;
        private readonly ILogger<LeadController> _logger;
        public LeadController(ILeadService leadService, ILogger<LeadController> logger)
        {
            _leadServicce = leadService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeadsAsync([FromQuery] LeadSearchDTO search)
        {
            var leadDTOs = await _leadServicce.GetLeadsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDTOs)}");
            return Ok(leadDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetLeadByIdAsync))]
        public async Task<IActionResult> GetLeadByIdAsync(Guid id)
        {
            var leadDTO = await _leadServicce.GetLeadByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Lead: {JsonSerializer.Serialize(leadDTO)}");
            return Ok(leadDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadAsync([FromBody] LeadCreateDTO leadDTO)
        {
            var leadCreateDTO = await _leadServicce.CreateLeadAsync(leadDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Lead: {JsonSerializer.Serialize(leadCreateDTO)}");
            return CreatedAtAction(nameof(GetLeadByIdAsync), new { id = leadCreateDTO.Id }, leadCreateDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeadAsync(Guid id, [FromBody] LeadUpdateDTO leadDTO)
        {
            var leadUpdateDTO = await _leadServicce.UpdateLeadAsync(id, leadDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Lead: {JsonSerializer.Serialize(leadUpdateDTO)}");
            return Ok(leadUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeadAsync(Guid id)
        {
            await _leadServicce.DeleteLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Lead: {id}");
            return Ok("Successfully Deleted Lead");
        }

        [HttpPost("{id}/qualify")]
        public async Task<IActionResult> QualifyLeadAsync(Guid id)
        {
            var dealDTO = await _leadServicce.QualifyLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Qualified Lead: {JsonSerializer.Serialize(dealDTO)}");
            return CreatedAtRoute(new { id = dealDTO.Id, controller = "deal", action = nameof(DealController.GetDealByIdAsync) }, dealDTO);
        }

        [HttpPost("{id}/disqualify")]
        public async Task<IActionResult> DisqualifyLeadAsync(Guid id, [FromBody] DisqualifyDTO disqualifyDTO)
        {
            await _leadServicce.DisqualifyLeadAsync(id, disqualifyDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Disqualified Lead: {id}");
            return Ok("Successfully Disqualify Lead");
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> GetStatisticLeadAsync()
        {
            var leadStatisticDTO = await _leadServicce.GetStatisticLeadAsync();
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Lead Statistic: {JsonSerializer.Serialize(leadStatisticDTO)}");
            return Ok(leadStatisticDTO);
        }
    }
}