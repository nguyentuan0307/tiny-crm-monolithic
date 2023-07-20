using TinyCRM.API.Models.Account;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;

namespace TinyCRM.API.Services.IServices
{
    public interface IAccountService
    {
        Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountDTO);

        Task DeleteAccountAsync(Guid id);

        Task<AccountDTO> GetAccountByIdAsync(Guid id);

        Task<IList<AccountDTO>> GetAccountsAsync(AccountSearchDTO search);

        Task<List<ContactDTO>> GetContactsByAccountIdAsync(Guid id);

        Task<List<DealDTO>> GetDealsByAccountIdAsync(Guid id);

        Task<List<LeadDTO>> GetLeadsByAccountIdAsync(Guid id);

        Task<AccountDTO> UpdateAccountAsync(Guid id, AccountUpdateDTO accountDTO);
    }
}