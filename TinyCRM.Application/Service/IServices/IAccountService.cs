using TinyCRM.Application.Models.Account;

namespace TinyCRM.Application.Service.IServices;

public interface IAccountService
{
    public Task<AccountDto> CreateAccountAsync(AccountCreateDto accountDto);

    public Task DeleteAccountAsync(Guid id);

    public Task<AccountDto> GetAccountAsync(Guid id);

    public Task<List<AccountDto>> GetAccountsAsync(AccountSearchDto search);

    public Task<AccountDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto);
}