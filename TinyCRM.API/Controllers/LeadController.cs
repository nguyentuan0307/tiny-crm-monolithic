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
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads");
            return Ok(leadDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetLeadByIdAsync))]
        public async Task<IActionResult> GetLeadByIdAsync(Guid id)
        {
            var leadDTO = await _leadServicce.GetLeadByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Lead");
            return Ok(leadDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadAsync([FromBody] LeadCreateDTO leadDTO)
        {
            var leadCreateDTO = await _leadServicce.CreateLeadAsync(leadDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Lead");
            return CreatedAtAction(nameof(GetLeadByIdAsync), new { id = leadCreateDTO.Id }, leadCreateDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeadAsync(Guid id, [FromBody] LeadUpdateDTO leadDTO)
        {
            var leadUpdateDTO = await _leadServicce.UpdateLeadAsync(id, leadDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Lead");
            return Ok(leadUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeadAsync(Guid id)
        {
            await _leadServicce.DeleteLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Lead");
            return Ok("Successfully Deleted Lead");
        }

        [HttpPost("{id}/qualify")]
        public async Task<IActionResult> QualifyLeadAsync(Guid id)
        {
            var dealDTO = await _leadServicce.QualifyLeadAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Qualified Lead");
            return CreatedAtRoute(new { id = dealDTO.Id, controller = "deal", action = nameof(DealController.GetDealByIdAsync) }, dealDTO);
        }

        [HttpPost("{id}/disqualify")]
        public async Task<IActionResult> DisqualifyLeadAsync(Guid id, [FromBody] DisqualifyDTO disqualifyDTO)
        {
            await _leadServicce.DisqualifyLeadAsync(id, disqualifyDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Disqualified Lead");
            return Ok("Successfully Disqualify Lead");
        }
    }
}