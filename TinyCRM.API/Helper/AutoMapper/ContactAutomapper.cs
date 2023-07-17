using AutoMapper;
using TinyCRM.API.Models.Contact;
using TinyCRM.Domain.Entities.Contacts;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ContactAutomapper : Profile
    {
        public ContactAutomapper()
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<ContactCreateDTO, Contact>();
            CreateMap<ContactUpdateDTO, Contact>();
        }
    }
}
