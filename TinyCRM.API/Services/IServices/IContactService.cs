using TinyCRM.API.Models.Contact;

namespace TinyCRM.API.Services.IServices
{
    public interface IContactService
    {
        public Task<ContactDto> CreateContactAsync(ContactCreateDto contactDto);

        public Task DeleteContactAsync(Guid id);

        public Task<ContactDto> GetContactByIdAsync(Guid id);

        public Task<IList<ContactDto>> GetContactsAsync(ContactSearchDto search);

        public Task<ContactDto> UpdateContactAsync(Guid id, ContactUpdateDto contactDto);

        public Task<IList<ContactDto>> GetContactsByAccountIdAsync(Guid accountId);
    }
}