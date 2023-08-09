using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.Application.Models.Contact;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Infrastructure.Logger;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Read)]
    public async Task<IActionResult> GetContactsAsync([FromQuery] ContactSearchDto search)
    {
        var contactDtOs = await _contactService.GetContactsAsync(search);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDtOs)}");
        return Ok(contactDtOs);
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetContactAsync))]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Read)]
    public async Task<IActionResult> GetContactAsync(Guid id)
    {
        var contactDto = await _contactService.GetContactAsync(id);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contact: {JsonSerializer.Serialize(contactDto)}");
        return Ok(contactDto);
    }

    [HttpPost]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Create)]
    public async Task<IActionResult> CreateContactAsync([FromBody] ContactCreateDto contactDto)
    {
        var contactNewDto = await _contactService.CreateContactAsync(contactDto);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Created Contact: {JsonSerializer.Serialize(contactNewDto)}");
        return CreatedAtAction(nameof(GetContactAsync), new { id = contactNewDto.Id }, contactNewDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Edit)]
    public async Task<IActionResult> UpdateContactAsync(Guid id, [FromBody] ContactUpdateDto contactDto)
    {
        var contactUpdateDto = await _contactService.UpdateContactAsync(id, contactDto);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Updated Contact: {JsonSerializer.Serialize(contactUpdateDto)}");
        return Ok(contactUpdateDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Delete)]
    public async Task<IActionResult> DeleteContactAsync(Guid id)
    {
        await _contactService.DeleteContactAsync(id);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Deleted Contact: {id}");
        return Ok("Successfully Deleted Contact");
    }

    [HttpGet("account/{accountId:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Contacts.Read)]
    public async Task<IActionResult> GetContactsAsync(Guid accountId, [FromQuery] ContactSearchDto search)
    {
        var contactDtOs = await _contactService.GetContactsAsync(accountId, search);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDtOs)}");
        return Ok(contactDtOs);
    }
}