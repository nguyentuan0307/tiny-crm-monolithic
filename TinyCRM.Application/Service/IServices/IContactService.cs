using TinyCRM.Application.Models.Contact;

namespace TinyCRM.Application.Service.IServices;

public interface IContactService
{
    public Task<ContactDto> CreateContactAsync(ContactCreateDto contactDto);

    public Task DeleteContactAsync(Guid id);

    public Task<ContactDto> GetContactAsync(Guid id);

    public Task<List<ContactDto>> GetContactsAsync(ContactSearchDto search);

    public Task<ContactDto> UpdateContactAsync(Guid id, ContactUpdateDto contactDto);

    public Task<List<ContactDto>> GetContactsAsync(Guid accountId, ContactSearchDto search);
}