using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Helper.Filters;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [SortFilterAttributeQuery(Filters = "Name,Email")]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountSearchDTO search)
        {
            var accountDTOs = await _accountService.GetAccountsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Accounts: {JsonSerializer.Serialize(accountDTOs)}");
            return Ok(accountDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid id)
        {
            var accountDTO = await _accountService.GetAccountByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Account: {JsonSerializer.Serialize(accountDTO)}");
            return Ok(accountDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreateDTO accountDTO)
        {
            var accountNewDTO = await _accountService.CreateAccountAsync(accountDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Account: {JsonSerializer.Serialize(accountNewDTO)}");
            return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = accountNewDTO.Id }, accountNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountAsync(Guid id, [FromBody] AccountUpdateDTO accountDTO)
        {
            var accountUpdateDTO = await _accountService.UpdateAccountAsync(id, accountDTO);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Updated Account: {JsonSerializer.Serialize(accountUpdateDTO)}");
            return Ok(accountUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountAsync(Guid id)
        {
            await _accountService.DeleteAccountAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Account: {id}");
            return Ok("Successfully Deleted Account");
        }

        [HttpGet("{id}/contacts")]
        public async Task<IActionResult> GetContactsByAccountIdAsync(Guid id)
        {
            var contactDTOs = await _contactService.GetContactsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Contacts: {JsonSerializer.Serialize(contactDTOs)}");
            return Ok(contactDTOs);
        }

        [HttpGet("{id}/leads")]
        public async Task<IActionResult> GetLeadsByAccountIdAsync(Guid id)
        {
            var leadDTOs = await _leadService.GetLeadsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Leads: {JsonSerializer.Serialize(leadDTOs)}");
            return Ok(leadDTOs);
        }

        [HttpGet("{id}/deals")]
        public async Task<IActionResult> GetDealsByAccountIdAsync(Guid id)
        {
            var dealDTOs = await _dealService.GetDealsByAccountIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Deals: {JsonSerializer.Serialize(dealDTOs)}");
            return Ok(dealDTOs);
        }
    }
}