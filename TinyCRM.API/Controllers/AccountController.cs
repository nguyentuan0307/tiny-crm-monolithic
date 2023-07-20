using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountSearchDTO search)
        {
            var accountDTOs = await _accountService.GetAccountsAsync(search);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Accounts");
            return Ok(accountDTOs);

        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid id)
        {
            var accountDTO = await _accountService.GetAccountByIdAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Retrieved Account");
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
            string accountJson = JsonSerializer.Serialize(accountUpdateDTO);

            _logger.LogInformation($"[{DateTime.Now}]Successfully Created Account: {accountJson}");
            return Ok(accountUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountAsync(Guid id)
        {
            await _accountService.DeleteAccountAsync(id);
            _logger.LogInformation($"[{DateTime.Now}]Successfully Deleted Account: {id}");
            return Ok("Successfully Deleted Account");
        }
    }
}