using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Users;

public interface IUserRepository : IRepository<ApplicationUser, string>
{
    Task<List<ApplicationUser>> GetUsersAsync(UserQueryParameters parameters);
}