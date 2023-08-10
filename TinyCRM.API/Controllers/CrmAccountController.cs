using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyCRM.Application.Models.Account;
using TinyCRM.Application.Models.Permissions;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Infrastructure.Logger;

namespace TinyCRM.API.Controllers;

[ApiController]
[Route("api/crm-accounts")]
public class CrmAccountController : Controller
{
    private readonly IAccountService _accountService;

    public CrmAccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Authorize(Policy = TinyCrmPermissions.Accounts.Read)]
    public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountSearchDto search)
    {
        var accountDtOs = await _accountService.GetAccountsAsync(search);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Retrieved Accounts: {JsonSerializer.Serialize(accountDtOs)}");
        return Ok(accountDtOs);
    }

    [HttpGet("{id:guid}")]
    [ActionName(nameof(GetAccountAsync))]
    [Authorize(Policy = TinyCrmPermissions.Accounts.Read)]
    public async Task<IActionResult> GetAccountAsync(Guid id)
    {
        var accountDto = await _accountService.GetAccountAsync(id);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Retrieved Account: {JsonSerializer.Serialize(accountDto)}");
        return Ok(accountDto);
    }

    [HttpPost]
    [Authorize(Policy = TinyCrmPermissions.Accounts.Create)]
    public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreateDto accountDto)
    {
        var accountNewDto = await _accountService.CreateAccountAsync(accountDto);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Created Account: {JsonSerializer.Serialize(accountNewDto)}");
        return CreatedAtAction(nameof(GetAccountAsync), new { id = accountNewDto.Id }, accountNewDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Accounts.Edit)]
    public async Task<IActionResult> UpdateAccountAsync(Guid id, [FromBody] AccountUpdateDto accountDto)
    {
        var accountUpdateDto = await _accountService.UpdateAccountAsync(id, accountDto);
        LoggerService.LogInformation(
            $"[{DateTime.Now}]Successfully Updated Account: {JsonSerializer.Serialize(accountUpdateDto)}");
        return Ok(accountUpdateDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = TinyCrmPermissions.Accounts.Delete)]
    public async Task<IActionResult> DeleteAccountAsync(Guid id)
    {
        await _accountService.DeleteAccountAsync(id);
        LoggerService.LogInformation($"[{DateTime.Now}]Successfully Deleted Account: {id}");
        return Ok("Successfully Deleted Account");
    }
}