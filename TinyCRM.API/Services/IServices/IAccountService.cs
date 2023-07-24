using TinyCRM.API.Models.Account;

namespace TinyCRM.API.Services.IServices
{
    public interface IAccountService
    {
        public Task<AccountDto> CreateAccountAsync(AccountCreateDto accountDto);

        public Task DeleteAccountAsync(Guid id);

        public Task<AccountDto> GetAccountByIdAsync(Guid id);

        public Task<IList<AccountDto>> GetAccountsAsync(AccountSearchDto search);

        public Task<AccountDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto);
    }
}