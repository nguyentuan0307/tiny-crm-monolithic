using Microsoft.AspNetCore.Mvc;
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

        public LeadController(ILeadService leadService)
        {
            _leadServicce = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeadsAsync([FromQuery] LeadSearchDTO search)
        {
            var leadDTOs = await _leadServicce.GetLeadsAsync(search);
            return Ok(leadDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetLeadByIdAsync))]
        public async Task<IActionResult> GetLeadByIdAsync(Guid id)
        {
            var leadDTO = await _leadServicce.GetLeadByIdAsync(id);
            return Ok(leadDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeadAsync([FromBody] LeadCreateDTO leadDTO)
        {
            var leadCreateDTO = await _leadServicce.CreateLeadAsync(leadDTO);
            return CreatedAtAction(nameof(GetLeadByIdAsync), new { id = leadCreateDTO.Id }, leadCreateDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeadAsync(Guid id, [FromBody] LeadUpdateDTO leadDTO)
        {
            var leadUpdateDTO = await _leadServicce.UpdateLeadAsync(id, leadDTO);
            return Ok(leadUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeadAsync(Guid id)
        {
            await _leadServicce.DeleteLeadAsync(id);
            return Ok("Successfully Deleted Lead");
        }

        [HttpPost("{id}/qualify")]
        public async Task<IActionResult> QualifyLeadAsync(Guid id)
        {
            var dealDTO = await _leadServicce.QualifyLeadAsync(id);
            return CreatedAtRoute(new { id = dealDTO.Id, controller = "deal", action = nameof(DealController.GetDealByIdAsync) }, dealDTO);
        }

        [HttpPost("{id}/disqualify")]
        public async Task<IActionResult> DisqualifyLeadAsync(Guid id, [FromBody] DisqualifyDTO disqualifyDTO)
        {
            await _leadServicce.DisqualifyLeadAsync(id, disqualifyDTO);
            return Ok("Successfully Disqualify Lead");
        }
    }
}
