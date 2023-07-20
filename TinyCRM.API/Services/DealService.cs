using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DealService(IDealRepository dealRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _dealRepository = dealRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task DeleteDealAsync(Guid id)
        {
            var deal = await FindDealAsync(id);
            _dealRepository.Remove(deal);
            await _unitOfWork.SaveChangeAsync();
        }

        private async Task<Deal> FindDealAsync(Guid id)
        {
            return await _dealRepository.GetAsync(p => p.Id == id)
                    ?? throw new NotFoundHttpException("Deal is not found");
        }

        public async Task<DealDTO> GetDealByIdAsync(Guid id)
        {
            var deal = await FindDealAsync(id);
            return _mapper.Map<DealDTO>(deal);
        }

        public async Task<IList<DealDTO>> GetDealsAsync(DealSearchDTO search)
        {
            var query = _dealRepository.List(GetExpression(search));

            var leads = await ApplySortingAndPagination(query, search).ToListAsync();
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

        private static IQueryable<Deal> ApplySortingAndPagination(IQueryable<Deal> query, DealSearchDTO search)
        {
            string sortOrder = search.IsAsc ? "ascending" : "descending";
            query = string.IsNullOrEmpty(search.KeySort)
                    ? query.OrderBy("Id " + sortOrder)
                    : query.OrderBy(search.KeySort + " " + sortOrder);

            query = query.Include(p => p.ProductDeals)
                           .ThenInclude(p => p.Product).Skip(search.PageSize * (search.PageIndex - 1)).Take(search.PageSize);
            return query;
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
    }
}
