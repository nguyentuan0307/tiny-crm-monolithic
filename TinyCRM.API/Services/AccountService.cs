using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Models.Contact;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Leads;
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
            var includeTables = string.Empty;
            var query = _accountRepository.List(GetExpression(search.KeyWord), includeTables, search.Sorting, search.PageIndex, search.PageSize);

            var accounts = await query.ToListAsync();
            var accountDTOs = _mapper.Map<IList<AccountDTO>>(accounts);

            return accountDTOs;
        }

        private static Expression<Func<Account, bool>> GetExpression(string? keyWord)
        {
            Expression<Func<Account, bool>> expression = p => string.IsNullOrEmpty(keyWord)
                || p.Name.Contains(keyWord)
                || p.Email.Contains(keyWord);

            return expression;
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