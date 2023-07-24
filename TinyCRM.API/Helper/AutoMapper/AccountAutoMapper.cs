using AutoMapper;
using TinyCRM.API.Models.Account;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.API.Helper.AutoMapper
{
    public class AccountAutoMapper : Profile
    {
        public AccountAutoMapper()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<AccountCreateDto, Account>();
            CreateMap<AccountUpdateDto, Account>();
        }
    }
}