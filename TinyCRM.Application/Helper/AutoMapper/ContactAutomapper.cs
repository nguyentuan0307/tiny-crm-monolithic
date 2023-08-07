using TinyCRM.Application.Models.Contact;
using TinyCRM.Domain.Entities.Contacts;

namespace TinyCRM.Application.Helper.AutoMapper;

public class ContactAutoMapper : TinyCrmAutoMapper
{
    public ContactAutoMapper()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<ContactCreateDto, Contact>();
        CreateMap<ContactUpdateDto, Contact>();
    }
}