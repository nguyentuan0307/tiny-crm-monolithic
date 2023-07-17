using TinyCRM.Domain.Entities.Contacts;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
