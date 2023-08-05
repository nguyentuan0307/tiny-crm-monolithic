using TinyCRM.Application.Models.Contact;
using TinyCRM.Domain.Entities.Contacts;

namespace TinyCRM.Infrastructure.Helper.AutoMapper
{
    public class ContactAutoMapper : TinyCRMAutoMapper
    {
        public ContactAutoMapper()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();
        }
    }
}