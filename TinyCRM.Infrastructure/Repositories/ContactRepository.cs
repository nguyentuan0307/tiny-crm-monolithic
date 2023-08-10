using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.Contacts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class ContactRepository : Repository<Contact, Guid>, IContactRepository
{
    public ContactRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public async Task<List<Contact>> GetContactsAsync(ContactQueryParameters contactQueryParameters)
    {
        var specification = new ContactsByFilterSpecification(contactQueryParameters.KeyWord);
        return await ListAsync(specification,
            contactQueryParameters.IncludeTables,
            contactQueryParameters.Sorting,
            contactQueryParameters.PageIndex,
            contactQueryParameters.PageSize);
    }

    public async Task<List<Contact>> GetContactsByAccountIdAsync(ContactQueryParameters contactQueryParameters)
    {
        var contactsByAccountIdSpecification =
            new ContactsByAccountIdSpecification(contactQueryParameters.AccountId!.Value);
        var specification =
            contactsByAccountIdSpecification.And(new ContactsByFilterSpecification(contactQueryParameters.KeyWord));

        return await ListAsync(specification,
            contactQueryParameters.IncludeTables,
            contactQueryParameters.Sorting,
            contactQueryParameters.PageIndex,
            contactQueryParameters.PageSize);
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

    protected override Expression<Func<Contact, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}