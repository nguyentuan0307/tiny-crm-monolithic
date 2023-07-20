using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}