using TinyCRM.API.Models.Account;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;

namespace TinyCRM.API.Services.IServices
{
    public interface IAccountService
    {
        public Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountDTO);

        public Task DeleteAccountAsync(Guid id);

        public Task<AccountDTO> GetAccountByIdAsync(Guid id);

        public Task<IList<AccountDTO>> GetAccountsAsync(AccountSearchDTO search);

        public Task<AccountDTO> UpdateAccountAsync(Guid id, AccountUpdateDTO accountDTO);
    }
}