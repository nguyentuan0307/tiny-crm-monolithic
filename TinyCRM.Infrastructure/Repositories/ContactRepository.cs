using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification;

namespace TinyCRM.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact, Guid>, IContactRepository
    {
        public ContactRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<Contact> GetContacts(ContactQueryParameters contactQueryParameters)
        {
            var specification = new ContactsSpecification(contactQueryParameters.KeyWord);
            return List(specification: specification,
                includeTables: contactQueryParameters.IncludeTables,
                sorting: contactQueryParameters.Sorting,
                pageIndex: contactQueryParameters.PageIndex,
                pageSize: contactQueryParameters.PageSize);
        }

        public IQueryable<Contact> GetContactsByAccountId(ContactQueryParameters contactQueryParameters)
        {
            var specification = new ContactsByAccountIdSpecification(contactQueryParameters.KeyWord, contactQueryParameters.AccountId!.Value);
            return List(specification: specification,
                includeTables: contactQueryParameters.IncludeTables,
                sorting: contactQueryParameters.Sorting,
                pageIndex: contactQueryParameters.PageIndex,
                pageSize: contactQueryParameters.PageSize);
        }

        protected override Expression<Func<Contact, bool>> ExpressionForGet(Guid id)
        {
            return p => p.Id == id;
        }

        public override Task<bool> AnyAsync(Guid id)
        {
            return DbSet.AnyAsync(p => p.Id == id);
        }

        public Task<bool> IsPhoneExistAsync(string phone, Guid guid)
        {
            return DbSet.AnyAsync(p => p.Phone == phone && p.Id != guid);
        }

        public Task<bool> IsEmailExistAsync(string email, Guid guid)
        {
            return DbSet.AnyAsync(p => p.Email == email && p.Id != guid);
        }
    }
}