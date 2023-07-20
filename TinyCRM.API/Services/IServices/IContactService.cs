using TinyCRM.API.Models.Contact;

namespace TinyCRM.API.Services.IServices
{
    public interface IContactService
    {
        Task<ContactDTO> CreateContactAsync(ContactCreateDTO contactDTO);

        Task DeleteContactAsync(Guid id);

        Task<ContactDTO> GetContactByIdAsync(Guid id);

        Task<IList<ContactDTO>> GetContactsAsync(ContactSearchDTO search);

        Task<ContactDTO> UpdateContactAsync(Guid id, ContactUpdateDTO contactDTO);
    }
}