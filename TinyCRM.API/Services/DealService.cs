using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Deals;
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
            return await _dealRepository.GetAsync(p => p.Id == id, includeTables)
                    ?? throw new NotFoundHttpException("Deal is not found");
        }

        public async Task<DealDTO> GetDealByIdAsync(Guid id)
        {
            var includeTables = "Lead,ProductDeals.Product";
            var deal = await FindDealAsync(id, includeTables);
            return _mapper.Map<DealDTO>(deal);
        }

        public async Task<IList<DealDTO>> GetDealsAsync(DealSearchDTO search)
        {
            var includeTables = "Lead,ProductDeals.Product";
            var query = _dealRepository.List(GetExpression(search), includeTables, search.Sorting, search.PageIndex, search.PageSize);

            var leads = await query.ToListAsync();
            var leadDTOs = _mapper.Map<IList<DealDTO>>(leads);

            return leadDTOs;
        }

        private static Expression<Func<Deal, bool>>? GetExpression(DealSearchDTO search)
        {
            Expression<Func<Deal, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
            || p.Title.Contains(search.KeyWord)
            || p.Lead.Account.Name.Contains(search.KeyWord);
            return expression;
        }

        public async Task<DealDTO> UpdateDealAsync(Guid id, DealUpdateDTO dealDTO)
        {
            var existingDeal = await FindDealAsync(id);

            if (existingDeal.StatusDeal == Domain.Enums.StatusDeal.Won || existingDeal.StatusDeal == Domain.Enums.StatusDeal.Lost)
            {
                throw new BadRequestHttpException("Deal is Win/Lose Status");
            }

            existingDeal.ActualRevenue = existingDeal.ProductDeals.Sum(p => p.TotalAmount);
            _mapper.Map(dealDTO, existingDeal);

            _dealRepository.Update(existingDeal);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DealDTO>(existingDeal);
        }

        public async Task<DealStatisticDTO> GetStatisticDealAsync()
        {
            var statistic = await _dealRepository.List().Select(x => new
            {
                x.StatusDeal,
                x.ActualRevenue
            }).ToListAsync();

            if (statistic.Count == 0)
            {
                return new DealStatisticDTO();
            }

            var dealStatisticDTO = new DealStatisticDTO
            {
                OpenDeals = statistic.Where(x => x.StatusDeal == Domain.Enums.StatusDeal.Open).Count(),
                WonDeals = statistic.Where(x => x.StatusDeal == Domain.Enums.StatusDeal.Won).Count(),
                LostDeals = statistic.Where(x => x.StatusDeal == Domain.Enums.StatusDeal.Lost).Count(),
                TotalRevenue = statistic.Sum(x => x.ActualRevenue),
                AvgRevenue = statistic.Average(x => x.ActualRevenue)
            };
            return dealStatisticDTO;
        }

        public async Task<IList<DealDTO>> GetDealsByAccountIdAsync(Guid accountId)
        {
            if (!await _accountRepository.AnyAsync(a => a.Id == accountId))
            {
                throw new BadRequestHttpException("Account is not found");
            }
            var dealAccounts = await _dealRepository.List(p => p.Lead.AccountId == accountId, "Lead").ToListAsync();

            return _mapper.Map<IList<DealDTO>>(dealAccounts);
        }
    }
}