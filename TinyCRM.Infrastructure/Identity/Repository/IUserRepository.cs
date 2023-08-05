using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Repository;

public interface IUserRepository : IRepository<ApplicationUser, string>
{
    Task<List<ApplicationUser>> GetUsersAsync(UserQueryParameters parameters);
}