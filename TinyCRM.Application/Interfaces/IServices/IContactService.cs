using TinyCRM.Application.Models.Contact;

namespace TinyCRM.Application.Interfaces.IServices
{
    public interface IContactService
    {
        public Task<ContactDto> CreateContactAsync(ContactCreateDto contactDto);

        public Task DeleteContactAsync(Guid id);

        public Task<ContactDto> GetContactByIdAsync(Guid id);

        public Task<List<ContactDto>> GetContactsAsync(ContactSearchDto search);

        public Task<ContactDto> UpdateContactAsync(Guid id, ContactUpdateDto contactDto);

        public Task<List<ContactDto>> GetContactsByAccountIdAsync(Guid accountId, ContactSearchDto search);
    }
}