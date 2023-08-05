using TinyCRM.Application.Models.Account;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.Infrastructure.Helper.AutoMapper
{
    public class AccountAutoMapper : TinyCRMAutoMapper
    {
        public AccountAutoMapper()
        {
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<AccountCreateDto, Account>();
            CreateMap<AccountUpdateDto, Account>();
        }
    }
}