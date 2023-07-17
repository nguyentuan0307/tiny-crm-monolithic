using AutoMapper;
using TinyCRM.API.Models.Account;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.API.Helper.AccountMapper
{
    public class AccountAutoMapper : Profile
    {
        public AccountAutoMapper()
        {
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountCreateDTO, Account>();
            CreateMap<AccountUpdateDTO, Account>();
        }
    }
}
