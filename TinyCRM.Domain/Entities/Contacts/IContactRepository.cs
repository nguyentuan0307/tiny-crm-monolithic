using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Contacts
{
    public interface IContactRepository : IRepository<Contact, Guid>
    {
        Task<bool> IsPhoneExistAsync(string phone, Guid guid);

        Task<bool> IsEmailExistAsync(string email, Guid guid);

        IQueryable<Contact> GetContacts(ContactQueryParameters contactQueryParameters);

        IQueryable<Contact> GetContactsByAccountId(ContactQueryParameters contactQueryParameters);
    }
}