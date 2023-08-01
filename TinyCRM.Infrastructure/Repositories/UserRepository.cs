using TinyCRM.Domain.Entities.Users;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification;

namespace TinyCRM.Infrastructure.Repositories;

public class UserRepository : Repository<ApplicationUser, string>, IUserRepository
{
    public UserRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public IQueryable<ApplicationUser> GetUsers(UserQueryParameters userQueryParameters)
    {
        var specification = new UsersSpecification(userQueryParameters.KeyWord);
        return List(specification: specification,
            includeTables: userQueryParameters.IncludeTables,
            sorting: userQueryParameters.Sorting,
            pageIndex: userQueryParameters.PageIndex,
            pageSize: userQueryParameters.PageSize
        );
    }
}