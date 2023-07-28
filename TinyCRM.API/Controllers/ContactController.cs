using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactsAsync([FromQuery] ContactSearchDto search)
        {
            var contactDtOs = await _contactService.GetContactsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDtOs)}");
            return Ok(contactDtOs);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetContactByIdAsync))]
        public async Task<IActionResult> GetContactByIdAsync(Guid id)
        {
            var contactDto = await _contactService.GetContactByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contact: {JsonSerializer.Serialize(contactDto)}");
            return Ok(contactDto);
        }

        [HttpPost]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> CreateContactAsync([FromBody] ContactCreateDto contactDto)
        {
            var contactNewDto = await _contactService.CreateContactAsync(contactDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Contact: {JsonSerializer.Serialize(contactNewDto)}");
            return CreatedAtAction(nameof(GetContactByIdAsync), new { id = contactNewDto.Id }, contactNewDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> UpdateContactAsync(Guid id, [FromBody] ContactUpdateDto contactDto)
        {
            var contactUpdateDto = await _contactService.UpdateContactAsync(id, contactDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Contact: {JsonSerializer.Serialize(contactUpdateDto)}");
            return Ok(contactUpdateDto);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            await _contactService.DeleteContactAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Contact: {id}");
            return Ok("Successfully Deleted Contact");
        }
    }
}