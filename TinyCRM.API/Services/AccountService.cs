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

        public async Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountDTO)
        {
            var account = _mapper.Map<Account>(accountDTO);
            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            await Console.Out.WriteLineAsync($"{account.Id}");
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            Expression<Func<Account, bool>> expression = p => p.Id == id;
            var account = await _accountRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Account is not found");
            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<AccountDTO> GetAccountByIdAsync(Guid id)
        {
            Expression<Func<Account, bool>> expression = p => p.Id == id;
            var account = await _accountRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Account is not found");
            return _mapper.Map<AccountDTO>(account);
        }
        public async Task<List<AccountDTO>> GetAccountsAsync(AccountSearchDTO search)
        {
            Expression<Func<Account, bool>> expression = p => string.IsNullOrEmpty(search.Filter) || p.Name.Contains(search.Filter);
            var query = _accountRepository.List(expression)
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount);

            List<Account> accounts = await query.ToListAsync();
            List<AccountDTO> accountDTOs = _mapper.Map<List<AccountDTO>>(accounts);

            return accountDTOs;
        }

        public async Task<AccountDTO> UpdateAccountAsync(Guid id, AccountUpdateDTO accountDTO)
        {
            if (id != accountDTO.Id)
            {
                throw new BadRequestHttpException("ID provided does not match the ID in the Account");
            }

            Account existingAccount = await _accountRepository.GetAsync(p => p.Id == id) ?? throw new NotFoundHttpException("Account not found");

            existingAccount.Name = accountDTO.Name;
            existingAccount.Email = accountDTO.Email;
            existingAccount.Phone = accountDTO.Phone;
            existingAccount.Address = accountDTO.Address;

            _accountRepository.Update(existingAccount);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<AccountDTO>(existingAccount);
        }
    }
}
