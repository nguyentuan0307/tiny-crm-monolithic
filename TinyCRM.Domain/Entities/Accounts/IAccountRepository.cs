using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Accounts;

public interface IAccountRepository : IRepository<Account, Guid>
{
    Task<List<Account>> GetAccountsAsync(AccountQueryParameters accountQueryParameters);

    Task<bool> EmailIsExitsAsync(string email, Guid id);

    Task<bool> PhoneIsExistAsync(string phone, Guid id);
}