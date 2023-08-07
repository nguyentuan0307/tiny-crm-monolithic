using AutoMapper;
using TinyCRM.Application.Models.Contact;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Application.Service;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(IContactRepository contactRepository, IMapper mapper, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public async Task<ContactDto> CreateContactAsync(ContactCreateDto contactDto)
    {
        await CheckValidateAccount(contactDto.AccountId);
        await CheckValidate(contactDto.Email, contactDto.Phone);

        var contact = _mapper.Map<Contact>(contactDto);
        await _contactRepository.AddAsync(contact);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<ContactDto>(contact);
    }

    private async Task CheckValidateAccount(Guid accountId)
    {
        if (!await _accountRepository.AnyAsync(accountId))
        {
            throw new EntityNotFoundException($"Account with Id[{accountId} is not found");
        }
    }

    public async Task DeleteContactAsync(Guid id)
    {
        var contact = await FindContactAsync(id);

        _contactRepository.Remove(contact);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<ContactDto> GetContactAsync(Guid id)
    {
        var contact = await FindContactAsync(id);
        return _mapper.Map<ContactDto>(contact);
    }

    public async Task<List<ContactDto>> GetContactsAsync(ContactSearchDto search)
    {
        const string includeTables = "Account";
        var contactQueryParameters = new ContactQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize,
            IncludeTables = includeTables
        };

        var contacts = await _contactRepository
            .GetContactsAsync(contactQueryParameters);
        var contactDtOs = _mapper.Map<List<ContactDto>>(contacts);

        return contactDtOs;
    }

    public async Task<ContactDto> UpdateContactAsync(Guid id, ContactUpdateDto contactDto)
    {
        var existingContact = await FindContactAsync(id);
        await CheckValidate(contactDto.Email, contactDto.Phone, existingContact.Id);
        _mapper.Map(contactDto, existingContact);

        _contactRepository.Update(existingContact);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<ContactDto>(existingContact);
    }

    private async Task CheckValidate(string email, string phone, Guid guid = default)
    {
        if (await _contactRepository.IsEmailExistAsync(email, guid))
        {
            throw new InvalidUpdateException($"Email[{email}] is already exist");
        }

        if (await _contactRepository.IsPhoneExistAsync(phone, guid))
        {
            throw new InvalidUpdateException($"Phone[{phone}] is already exist");
        }
    }

    private async Task<Contact> FindContactAsync(Guid id)
    {
        return await _contactRepository.GetAsync(id)
               ?? throw new EntityNotFoundException($"Contact with Id[{id}] is not found");
    }

    public async Task<List<ContactDto>> GetContactsAsync(Guid accountId, ContactSearchDto search)
    {
        await CheckValidateAccount(accountId);

        const string includeTables = "Account";

        var contactQueryParameters = new ContactQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize,
            IncludeTables = includeTables,
            AccountId = accountId
        };

        var contacts = await _contactRepository
            .GetContactsByAccountIdAsync(contactQueryParameters);

        return _mapper.Map<List<ContactDto>>(contacts);
    }
}