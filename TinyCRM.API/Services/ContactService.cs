using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IContactRepository contactRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactDTO> CreateContactAsync(ContactCreateDTO ContactDTO)
        {
            var Contact = _mapper.Map<Contact>(ContactDTO);
            await _contactRepository.AddAsync(Contact);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ContactDTO>(Contact);
        }

        public async Task DeleteContactAsync(Guid id)
        {
            Expression<Func<Contact, bool>> expression = p => p.Id == id;
            var Contact = await _contactRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Contact is not found");
            _contactRepository.Remove(Contact);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ContactDTO> GetContactByIdAsync(Guid id)
        {
            Expression<Func<Contact, bool>> expression = p => p.Id == id;
            var Contact = await _contactRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Contact is not found");
            return _mapper.Map<ContactDTO>(Contact);
        }
        public async Task<List<ContactDTO>> GetContactsAsync(ContactSearchDTO search)
        {
            Expression<Func<Contact, bool>> expression = p => string.IsNullOrEmpty(search.Filter) || p.Name.Contains(search.Filter);
            var query = _contactRepository.List(expression)
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount);

            List<Contact> Contacts = await query.ToListAsync();
            List<ContactDTO> ContactDTOs = _mapper.Map<List<ContactDTO>>(Contacts);

            return ContactDTOs;
        }

        public async Task<ContactDTO> UpdateContactAsync(Guid id, ContactUpdateDTO ContactDTO)
        {
            if (id != ContactDTO.Id)
            {
                throw new BadRequestHttpException("ID provided does not match the ID in the Contact");
            }

            Contact existingContact = await _contactRepository.GetAsync(p => p.Id == id) ?? throw new NotFoundHttpException("Contact not found");

            existingContact.Name = ContactDTO.Name;
            existingContact.Email = ContactDTO.Email;
            existingContact.Phone = ContactDTO.Phone;

            _contactRepository.Update(existingContact);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ContactDTO>(existingContact);
        }
    }
}
