using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Helper.Filters;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetContactsAsync([FromQuery] ContactSearchDTO search)
        {
            var contactDTOs = await _contactService.GetContactsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDTOs)}");
            return Ok(contactDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetContactByIdAsync))]
        public async Task<IActionResult> GetContactByIdAsync(Guid id)
        {
            var contactDTO = await _contactService.GetContactByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contact: {JsonSerializer.Serialize(contactDTO)}");
            return Ok(contactDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactAsync([FromBody] ContactCreateDTO contactDTO)
        {
            var contactNewDTO = await _contactService.CreateContactAsync(contactDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Contact: {JsonSerializer.Serialize(contactNewDTO)}");
            return CreatedAtAction(nameof(GetContactByIdAsync), new { id = contactNewDTO.Id }, contactNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactAsync(Guid id, [FromBody] ContactUpdateDTO contactDTO)
        {
            var contactUpdateDTO = await _contactService.UpdateContactAsync(id, contactDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Contact: {JsonSerializer.Serialize(contactUpdateDTO)}");
            return Ok(contactUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            await _contactService.DeleteContactAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Contact: {id}");
            return Ok("Successfully Deleted Contact");
        }
    }
}