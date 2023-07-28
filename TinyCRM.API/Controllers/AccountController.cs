using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Roles;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IDealService _dealService;
        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger, IDealService dealService, ILeadService leadService, IContactService contactService)
        {
            _accountService = accountService;
            _logger = logger;
            _dealService = dealService;
            _leadService = leadService;
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountSearchDto search)
        {
            var accountDtOs = await _accountService.GetAccountsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Accounts: {JsonSerializer.Serialize(accountDtOs)}");
            return Ok(accountDtOs);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid id)
        {
            var accountDto = await _accountService.GetAccountByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Account: {JsonSerializer.Serialize(accountDto)}");
            return Ok(accountDto);
        }

        [HttpPost]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreateDto accountDto)
        {
            var accountNewDto = await _accountService.CreateAccountAsync(accountDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Account: {JsonSerializer.Serialize(accountNewDto)}");
            return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = accountNewDto.Id }, accountNewDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> UpdateAccountAsync(Guid id, [FromBody] AccountUpdateDto accountDto)
        {
            var accountUpdateDto = await _accountService.UpdateAccountAsync(id, accountDto);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Account: {JsonSerializer.Serialize(accountUpdateDto)}");
            return Ok(accountUpdateDto);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Policy.AdminPolicy)]
        public async Task<IActionResult> DeleteAccountAsync(Guid id)
        {
            await _accountService.DeleteAccountAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Account: {id}");
            return Ok("Successfully Deleted Account");
        }

        [HttpGet("{id:guid}/contacts")]
        public async Task<IActionResult> GetContactsByAccountIdAsync(Guid id)
        {
            var contactDtOs = await _contactService.GetContactsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDtOs)}");
            return Ok(contactDtOs);
        }

        [HttpGet("{id:guid}/leads")]
        public async Task<IActionResult> GetLeadsByAccountIdAsync(Guid id)
        {
            var leadDtOs = await _leadService.GetLeadsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDtOs)}");
            return Ok(leadDtOs);
        }

        [HttpGet("{id:guid}/deals")]
        public async Task<IActionResult> GetDealsByAccountIdAsync(Guid id)
        {
            var dealDtOs = await _dealService.GetDealsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDtOs)}");
            return Ok(dealDtOs);
        }
    }
}