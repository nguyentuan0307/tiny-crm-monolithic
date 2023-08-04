using AutoMapper;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Account;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Helper.QueryParameters;
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
            var account = await FindAccountAsync(id);

            _accountRepository.Remove(account);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid id)
        {
            var account = await FindAccountAsync(id);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<IList<AccountDto>> GetAccountsAsync(AccountSearchDto search)
        {
            var includeTables = string.Empty;
            var accountQueryParameters = new AccountQueryParameters
            {
                KeyWord = search.KeyWord,
                IncludeTables = includeTables,
                Sorting = search.ConvertSort(),
                PageIndex = search.PageIndex,
                PageSize = search.PageSize
            };

            var accounts = await _accountRepository.GetAccountsAsync(accountQueryParameters);
            var accountDtOs = _mapper.Map<IList<AccountDto>>(accounts);

            return accountDtOs;
        }

        public async Task<AccountDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto)
        {
            var existingAccount = await FindAccountAsync(id);
            await CheckValidate(accountDto.Email, accountDto.Phone, existingAccount.Id);

            _mapper.Map(accountDto, existingAccount);
            _accountRepository.Update(existingAccount);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AccountDto>(existingAccount);
        }

        private async Task<Account> FindAccountAsync(Guid id)
        {
            return await _accountRepository.GetAsync(id)
                ?? throw new NotFoundHttpException("Account is not found");
        }

        private async Task CheckValidate(string email, string phone, Guid guid = default)
        {
            if (await _accountRepository.EmailIsExitsAsync(email, guid))
            {
                throw new BadRequestHttpException("Email is already exist");
            }

            if (await _accountRepository.PhoneIsExistAsync(phone, guid))
            {
                throw new BadRequestHttpException("Phone is already exist");
            }
        }
    }
}