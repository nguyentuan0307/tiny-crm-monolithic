using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.Accounts;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class AccountRepository : Repository<Account, Guid>, IAccountRepository
{
    public AccountRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public async Task<List<Account>> GetAccountsAsync(AccountQueryParameters accountQueryParameters)
    {
        var specification = new AccountsSpecification(accountQueryParameters.KeyWord);

        return await ListAsync(specification,
            accountQueryParameters.IncludeTables,
            accountQueryParameters.Sorting,
            accountQueryParameters.PageIndex,
            accountQueryParameters.PageSize
        );
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    public Task<bool> EmailIsExitsAsync(string email, Guid id)
    {
        return DbSet.AnyAsync(p => p.Email == email && p.Id != id);
    }

    public Task<bool> PhoneIsExistAsync(string phone, Guid id)
    {
        return DbSet.AnyAsync(p => p.Phone == phone && p.Id != id);
    }

    protected override Expression<Func<Account, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}