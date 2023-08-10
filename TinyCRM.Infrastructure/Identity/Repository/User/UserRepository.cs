using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Infrastructure.Identity.Specification;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Repository.User;

public class UserRepository : Repository<ApplicationUser, string>, IUserRepository
{
    public UserRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public async Task<List<ApplicationUser>> GetUsersAsync(UserQueryParameters userQueryParameters)
    {
        var specification = new UsersByFilterSpecification(userQueryParameters.KeyWord);
        return await ListAsync(specification,
            userQueryParameters.IncludeTables,
            userQueryParameters.Sorting,
            userQueryParameters.PageIndex,
            userQueryParameters.PageSize
        );
    }
}