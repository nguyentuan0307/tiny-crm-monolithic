using TinyCRM.Application.Models.Account;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.Application.Helper.AutoMapper;

public class AccountAutoMapper : TinyCrmAutoMapper
{
    public AccountAutoMapper()
    {
        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<AccountCreateDto, Account>();
        CreateMap<AccountUpdateDto, Account>();
    }
}