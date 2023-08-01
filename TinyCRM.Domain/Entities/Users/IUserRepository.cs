using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Users
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        public IQueryable<ApplicationUser> GetUsers(UserQueryParameters parameters);
    }
}