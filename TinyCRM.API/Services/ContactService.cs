using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
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
            if (!await _accountRepository.AnyAsync(p => p.Id == accountId))
            {
                throw new BadRequestHttpException("Account is not exist");
            }
        }

        public async Task DeleteContactAsync(Guid id)
        {
            var contact = await FindContactAsync(id);

            _contactRepository.Remove(contact);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ContactDto> GetContactByIdAsync(Guid id)
        {
            var contact = await FindContactAsync(id);
            return _mapper.Map<ContactDto>(contact);
        }

        public async Task<IList<ContactDto>> GetContactsAsync(ContactSearchDto search)
        {
            const string includeTables = "Account";
            var expression = GetExpression(search.KeyWord);
            var sorting = ConvertSort(search);
            var query = _contactRepository.List(expression, includeTables, sorting, search.PageIndex, search.PageSize);

            var contacts = await query.ToListAsync();
            var contactDtOs = _mapper.Map<IList<ContactDto>>(contacts);

            return contactDtOs;
        }

        private static string ConvertSort(ContactSearchDto search)
        {
            if (search.SortFilter == null) return string.Empty;
            var sort = search.SortFilter.ToString() switch
            {
                "Name" => "Name",
                "Email" => "Email",
                "Phone" => "Phone",
                "AccountName" => "Account.Name",
                _ => "Id"
            };
            sort = search.SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }

        private static Expression<Func<Contact, bool>> GetExpression(string? keyword)
        {
            Expression<Func<Contact, bool>> expression = p => string.IsNullOrEmpty(keyword)
            || p.Name.Contains(keyword)
            || p.Email.Contains(keyword);
            return expression;
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
            var accounts = await _contactRepository.List(p => p.Email == email
            || p.Phone == phone).ToListAsync();
            if (accounts.Any(a => a.Email == email && a.Id != guid))
            {
                throw new BadRequestHttpException("Email is already exist");
            }
            if (accounts.Any(a => a.Phone == email))
            {
                throw new BadRequestHttpException("Phone is already exist");
            }
        }

        private async Task<Contact> FindContactAsync(Guid id)
        {
            return await _contactRepository.GetAsync(p => p.Id == id)
                ?? throw new NotFoundHttpException("Contact is not found");
        }

        public async Task<IList<ContactDto>> GetContactsByAccountIdAsync(Guid accountId)
        {
            if (!await _accountRepository.AnyAsync(p => p.Id == accountId))
            {
                throw new BadRequestHttpException("Account is not exist");
            }
            var contacts = await _contactRepository.List(p => p.AccountId == accountId).ToListAsync();
            return _mapper.Map<IList<ContactDto>>(contacts);
        }
    }
}