using TinyCRM.API.Models.Account;

namespace TinyCRM.API.Services.IServices
{
    public interface IAccountService
    {
        Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountDTO);
        Task DeleteAccountAsync(Guid id);
        Task<AccountDTO> GetAccountByIdAsync(Guid id);
        Task<List<AccountDTO>> GetAccountsAsync(AccountSearchDTO search);
        Task<AccountDTO> UpdateAccountAsync(Guid id, AccountUpdateDTO accountDTO);
    }
}
