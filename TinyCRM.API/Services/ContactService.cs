using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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

        public async Task<ContactDTO> CreateContactAsync(ContactCreateDTO contactDTO)
        {
            await CheckValidateAccount(contactDTO.AccountId);
            await CheckValidate(contactDTO.Email, contactDTO.Phone);

            var contact = _mapper.Map<Contact>(contactDTO);
            await _contactRepository.AddAsync(contact);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ContactDTO>(contact);
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

        public async Task<ContactDTO> GetContactByIdAsync(Guid id)
        {
            var contact = await FindContactAsync(id);
            return _mapper.Map<ContactDTO>(contact);
        }
        public async Task<IList<ContactDTO>> GetContactsAsync(ContactSearchDTO search)
        {
            var query = _contactRepository.List(GetExpression(search));

            var contacts = await ApplySortingAndPagination(query, search).ToListAsync();
            var contactDTOs = _mapper.Map<IList<ContactDTO>>(contacts);

            return contactDTOs;
        }

        private static Expression<Func<Contact, bool>> GetExpression(ContactSearchDTO search)
        {
            Expression<Func<Contact, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
            || p.Name.Contains(search.KeyWord)
            || p.Email.Contains(search.KeyWord);
            return expression;
        }

        private static IQueryable<Contact> ApplySortingAndPagination(IQueryable<Contact> query, ContactSearchDTO search)
        {
            string sortOrder = search.IsAsc ? "ascending" : "descending";

            query = string.IsNullOrEmpty(search.KeySort)
                    ? query.OrderBy("Id " + sortOrder)
                    : query.OrderBy(search.KeySort + " " + sortOrder);

            query = query.Skip(search.PageSize * (search.PageIndex - 1)).Take(search.PageSize);
            return query;
        }

        public async Task<ContactDTO> UpdateContactAsync(Guid id, ContactUpdateDTO contactDTO)
        {
            var existingContact = await FindContactAsync(id);
            await CheckValidate(contactDTO.Email, contactDTO.Phone, existingContact.Id);
            _mapper.Map(contactDTO, existingContact);

            _contactRepository.Update(existingContact);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ContactDTO>(existingContact);
        }

        private async Task CheckValidate(string email, string phone, Guid guid = default)
        {
            var accounts = await _contactRepository.List(p => p.Email == email
            || p.Phone == phone).ToListAsync();
            if (accounts.Any(a => a.Email == email && a.Id != guid))
            {
                throw new BadHttpRequestException("Email is already exist");
            }
            if (accounts.Any(a => a.Phone == email))
            {
                throw new BadHttpRequestException("Phone is already exist");
            }
        }

        private async Task<Contact> FindContactAsync(Guid id)
        {
            return await _contactRepository.GetAsync(p => p.Id == id)
                ?? throw new NotFoundHttpException("Contact is not found");
        }
    }
}
