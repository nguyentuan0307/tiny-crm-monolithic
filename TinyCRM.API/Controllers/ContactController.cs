﻿using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactsAsync([FromQuery] ContactSearchDTO search)
        {
            var contactDTOs = await _contactService.GetContactsAsync(search);
            return Ok(contactDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetContactByIdAsync))]
        public async Task<IActionResult> GetContactByIdAsync(Guid id)
        {
            var contactDTO = await _contactService.GetContactByIdAsync(id);
            return Ok(contactDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactAsync([FromBody] ContactCreateDTO contactDTO)
        {
            var contactNewDTO = await _contactService.CreateContactAsync(contactDTO);
            return CreatedAtAction(nameof(GetContactByIdAsync), new { id = contactNewDTO.Id }, contactNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactAsync(Guid id, [FromBody] ContactUpdateDTO contactDTO)
        {
            var contactUpdateDTO = await _contactService.UpdateContactAsync(id, contactDTO);
            return Ok(contactUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            await _contactService.DeleteContactAsync(id);
            return Ok("Successfully Deleted Contact");
        }
    }
}