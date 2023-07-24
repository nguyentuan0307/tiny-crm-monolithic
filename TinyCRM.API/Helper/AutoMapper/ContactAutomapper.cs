using AutoMapper;
using TinyCRM.API.Models.Contact;
using TinyCRM.Domain.Entities.Contacts;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class ContactAutoMapper : Profile
    {
        public ContactAutoMapper()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();
        }
    }
}