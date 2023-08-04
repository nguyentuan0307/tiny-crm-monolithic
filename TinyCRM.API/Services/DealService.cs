using AutoMapper;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DealService(IDealRepository dealRepository, IMapper mapper, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _dealRepository = dealRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteDealAsync(Guid id)
        {
            var deal = await FindDealAsync(id);
            _dealRepository.Remove(deal);
            await _unitOfWork.SaveChangeAsync();
        }

        private async Task<Deal> FindDealAsync(Guid id, string? includeTables = default)
        {
            return await _dealRepository.GetAsync(id, includeTables)
                    ?? throw new NotFoundHttpException("Deal is not found");
        }

        public async Task<DealDto> GetDealByIdAsync(Guid id)
        {
            const string includeTables = "Lead,ProductDeals.Product";
            var deal = await FindDealAsync(id, includeTables);
            return _mapper.Map<DealDto>(deal);
        }

        public async Task<IList<DealDto>> GetDealsAsync(DealSearchDto search)
        {
            const string includeTables = "Lead,ProductDeals.Product";

            var dealQueryParameters = new DealQueryParameters
            {
                KeyWord = search.KeyWord,
                IncludeTables = includeTables,
                Sorting = search.ConvertSort(),
                PageIndex = search.PageIndex,
                PageSize = search.PageSize
            };

            var leads = await _dealRepository.GetDealsAsync(dealQueryParameters);
            var leadDtOs = _mapper.Map<IList<DealDto>>(leads);

            return leadDtOs;
        }

        public async Task<DealDto> UpdateDealAsync(Guid id, DealUpdateDto dealDto)
        {
            var existingDeal = await FindDealAsync(id);

            if (existingDeal.StatusDeal is StatusDeal.Won or StatusDeal.Lost)
            {
                throw new BadRequestHttpException("Deal is Win/Lose Status");
            }

            existingDeal.ActualRevenue = existingDeal.ProductDeals.Sum(p => p.TotalAmount);
            _mapper.Map(dealDto, existingDeal);

            _dealRepository.Update(existingDeal);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DealDto>(existingDeal);
        }

        public async Task<DealStatisticDto> GetStatisticDealAsync()
        {
            var statistic = await _dealRepository.GetDealStatisticsAsync();

            if (statistic.Count == 0)
            {
                return new DealStatisticDto();
            }

            var dealStatisticDto = new DealStatisticDto
            {
                OpenDeals = statistic.Count(x => x.StatusDeal == StatusDeal.Open),
                WonDeals = statistic.Count(x => x.StatusDeal == StatusDeal.Won),
                LostDeals = statistic.Count(x => x.StatusDeal == StatusDeal.Lost),
                TotalRevenue = statistic.Sum(x => x.ActualRevenue),
                AvgRevenue = statistic.Average(x => x.ActualRevenue)
            };
            return dealStatisticDto;
        }

        public async Task<IList<DealDto>> GetDealsByAccountIdAsync(Guid accountId, DealSearchDto search)
        {
            if (!await _accountRepository.AnyAsync(accountId))
            {
                throw new BadRequestHttpException("Account is not found");
            }

            const string includeTables = "Lead.Account";

            var dealQueryParameters = new DealQueryParameters
            {
                KeyWord = search.KeyWord,
                Sorting = search.ConvertSort(),
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                IncludeTables = includeTables,
                AccountId = accountId
            };
            var dealAccounts = await _dealRepository.GetDealsByAccountIdAsync(dealQueryParameters);

            return _mapper.Map<IList<DealDto>>(dealAccounts);
        }
    }
}