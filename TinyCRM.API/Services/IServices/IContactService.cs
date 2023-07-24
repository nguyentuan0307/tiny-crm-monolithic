using TinyCRM.API.Models.Contact;

namespace TinyCRM.API.Services.IServices
{
    public interface IContactService
    {
        public Task<ContactDTO> CreateContactAsync(ContactCreateDTO contactDTO);

        public Task DeleteContactAsync(Guid id);

        public Task<ContactDTO> GetContactByIdAsync(Guid id);

        public Task<IList<ContactDTO>> GetContactsAsync(ContactSearchDTO search);

        public Task<ContactDTO> UpdateContactAsync(Guid id, ContactUpdateDTO contactDTO);
        public Task<IList<ContactDTO>> GetContactsByAccountIdAsync(Guid accountId);
    }
}