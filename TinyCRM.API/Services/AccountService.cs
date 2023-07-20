using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Interfaces;
using System.Linq.Dynamic.Core;

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

        public async Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountDTO)
        {
            await CheckValidate(accountDTO.Email, accountDTO.Phone);
            var account = _mapper.Map<Account>(accountDTO);
            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var account = await GetExistingAccount(id);

            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<AccountDTO> GetAccountByIdAsync(Guid id)
        {
            var account = await GetExistingAccount(id);

            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<IList<AccountDTO>> GetAccountsAsync(AccountSearchDTO search)
        {
            var query = _accountRepository.List(GetExpression(search));

            var accounts = await ApplySortingAndPagination(query, search).ToListAsync();
            var accountDTOs = _mapper.Map<IList<AccountDTO>>(accounts);

            return accountDTOs;
        }

        private static Expression<Func<Account, bool>> GetExpression(AccountSearchDTO search)
        {
            Expression<Func<Account, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
                || p.Name.Contains(search.KeyWord)
                || p.Email.Contains(search.KeyWord);

            return expression;
        }

        private static IQueryable<Account> ApplySortingAndPagination(IQueryable<Account> query, AccountSearchDTO search)
        {
            string sortOrder = search.IsAsc ? "ascending" : "descending";

            query = string.IsNullOrEmpty(search.KeySort)
                ? query.OrderBy("Id " + sortOrder)
                : query.OrderBy(search.KeySort + " " + sortOrder);

            query = query.Skip(search.PageSize * (search.PageIndex - 1)).Take(search.PageSize);
            return query;
        }

        public async Task<AccountDTO> UpdateAccountAsync(Guid id, AccountUpdateDTO accountDTO)
        {

            var existingAccount = await GetExistingAccount(id);
            await CheckValidate(accountDTO.Email, accountDTO.Phone, existingAccount.Id);

            _mapper.Map(accountDTO, existingAccount);
            _accountRepository.Update(existingAccount);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AccountDTO>(existingAccount);
        }

        public async Task<Account> GetExistingAccount(Guid id)
        {
            return await _accountRepository.GetAsync(p => p.Id == id)
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
