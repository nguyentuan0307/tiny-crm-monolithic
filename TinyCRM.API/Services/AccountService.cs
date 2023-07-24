using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDto> CreateAccountAsync(AccountCreateDto accountDto)
        {
            await CheckValidate(accountDto.Email, accountDto.Phone);
            var account = _mapper.Map<Account>(accountDto);
            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<AccountDto>(account);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var account = await GetExistingAccount(id);

            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            var account = await GetExistingAccount(id);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<IList<AccountDto>> GetAccountsAsync(AccountSearchDto search)
        {
            var includeTables = string.Empty;
            var expression = GetExpression(search.KeyWord);
            var sorting = ConvertSort(search);

            var query = _accountRepository.List(expression, includeTables, sorting, search.PageIndex, search.PageSize);

            var accounts = await query.ToListAsync();
            var accountDtOs = _mapper.Map<IList<AccountDto>>(accounts);

            return accountDtOs;
        }

        private static string ConvertSort(AccountSearchDto search)
        {
            var sort = search.SortFilter.ToString() switch
            {
                "Id" => "Id",
                "Name" => "Name",
                "Email" => "Email",
                "Phone" => "Phone",
                "Address" => "Address",
                _ => "Id"
            };
            sort = search.SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }

        private static Expression<Func<Account, bool>> GetExpression(string? keyWord)
        {
            Expression<Func<Account, bool>> expression = p => string.IsNullOrEmpty(keyWord)
                || p.Name.Contains(keyWord)
                || p.Email.Contains(keyWord);

            return expression;
        }

        public async Task<AccountDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto)
        {
            var existingAccount = await GetExistingAccount(id);
            await CheckValidate(accountDto.Email, accountDto.Phone, existingAccount.Id);

            _mapper.Map(accountDto, existingAccount);
            _accountRepository.Update(existingAccount);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AccountDto>(existingAccount);
        }

        public async Task<Account> GetExistingAccount(Guid id, string? includeTables = default)
        {
            return await _accountRepository.GetAsync(p => p.Id == id, includeTables)
                ?? throw new NotFoundHttpException("Account is not found");
        }

        private async Task CheckValidate(string email, string phone, Guid guid = default)
        {
            var accounts = await _accountRepository.List(p => p.Email == email
            || p.Phone == phone).ToListAsync();
            if (accounts.Any(a => a.Email == email && a.Id != guid))
            {
                throw new BadRequestHttpException("Email is already exist");
            }
            if (accounts.Any(a => a.Phone == email))
            {
                throw new BadRequestHttpException("Phone is already exist");
            }
        }
    }
}