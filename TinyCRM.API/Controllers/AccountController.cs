using Microsoft.AspNetCore.Mvc;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;

namespace TinyCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsAsync([FromQuery] AccountSearchDTO search)
        {
            var accountDTOs = await _accountService.GetAccountsAsync(search);
            return Ok(accountDTOs);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAccountByIdAsync))]
        public async Task<IActionResult> GetAccountByIdAsync(Guid id)
        {
            var accountDTO = await _accountService.GetAccountByIdAsync(id);
            return Ok(accountDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreateDTO accountDTO)
        {
            var accountNewDTO = await _accountService.CreateAccountAsync(accountDTO);
            return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = accountNewDTO.Id }, accountNewDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountAsync(Guid id, [FromBody] AccountUpdateDTO accountDTO)
        {
            var accountUpdateDTO = await _accountService.UpdateAccountAsync(id, accountDTO);
            return Ok(accountUpdateDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountAsync(Guid id)
        {
            await _accountService.DeleteAccountAsync(id);
            return Ok("Successfully Deleted Account");
        }
    }
}
