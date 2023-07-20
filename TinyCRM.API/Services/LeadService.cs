using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LeadService(ILeadRepository leadRepository, IMapper mapper, IUnitOfWork unitOfWork, IDealRepository dealRepository, IAccountRepository accountRepository)
        {
            _leadRepository = leadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dealRepository = dealRepository;
            _accountRepository = accountRepository;
        }
        public async Task<LeadDTO> CreateLeadAsync(LeadCreateDTO leadDTO)
        {
            if (!await IsExistAccount(leadDTO.AccountId))
            {
                throw new NotFoundHttpException("Account is not found");
            }

            var lead = _mapper.Map<Lead>(leadDTO);
            lead.StatusLead = StatusLead.Prospect;

            await _leadRepository.AddAsync(lead);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<LeadDTO>(lead);
        }

        private async Task<bool> IsExistAccount(Guid accountId)
        {
            return await _accountRepository.AnyAsync(p => p.Id == accountId);
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await FindLeadAsync(id);
            _leadRepository.Remove(lead);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DisqualifyLeadAsync(Guid id, DisqualifyDTO disqualifyDTO)
        {
            var existingLead = await FindLeadAsync(id);

            if (existingLead.StatusLead == StatusLead.Disqualified || existingLead.StatusLead == StatusLead.Quanlified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            _mapper.Map(disqualifyDTO, existingLead);
            _leadRepository.Update(existingLead);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<LeadDTO> GetLeadByIdAsync(Guid id)
        {
            var lead = await FindLeadAsync(id);
            return _mapper.Map<LeadDTO>(lead);
        }

        public async Task<IList<LeadDTO>> GetLeadsAsync(LeadSearchDTO search)
        {
            var query = _leadRepository.List(GetExpression(search));

            var leads = await ApplySortingAndPagination(query, search).ToListAsync();
            var leadDTOs = _mapper.Map<IList<LeadDTO>>(leads);
            return leadDTOs;
        }

        private static Expression<Func<Lead, bool>> GetExpression(LeadSearchDTO search)
        {
            Expression<Func<Lead, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
            || p.Title.Contains(search.KeyWord)
            || p.Account.Name.Contains(search.KeyWord);

            return expression;
        }

        private static IQueryable<Lead> ApplySortingAndPagination(IQueryable<Lead> query, LeadSearchDTO search)
        {
            string sortOrder = search.IsAsc ? "ascending" : "descending";
            query = string.IsNullOrEmpty(search.KeySort)
                    ? query.OrderBy("Id " + sortOrder)
                    : query.OrderBy(search.KeySort + " " + sortOrder);

            query = query.Skip(search.PageSize * (search.PageIndex - 1)).Take(search.PageSize);
            return query;
        }

        public async Task<DealDTO> QualifyLeadAsync(Guid id)
        {

            var existingLead = await GetSatisfiedLead(id);

            existingLead.StatusLead = StatusLead.Quanlified;
            _leadRepository.Update(existingLead);

            var deal = new Deal
            {
                LeadId = existingLead.Id,
                Title = existingLead.Title,
                StatusDeal = StatusDeal.Open
            };

            await _dealRepository.AddAsync(deal);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DealDTO>(deal);
        }

        private async Task<Lead> GetSatisfiedLead(Guid id)
        {
            var existingLead = await _leadRepository.GetAsync(l => l.Id == id) ?? throw new NotFoundHttpException("Lead is not found");

            if (existingLead.StatusLead == StatusLead.Disqualified || existingLead.StatusLead == StatusLead.Quanlified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            if (_dealRepository.IsExistingDeal(existingLead.Id))
            {
                throw new BadRequestHttpException("This lead already exists Deal");
            }

            return existingLead;
        }

        public async Task<LeadDTO> UpdateLeadAsync(Guid id, LeadUpdateDTO leadDTO)
        {
            var existingLead = await FindLeadAsync(id);

            if (existingLead.StatusLead == StatusLead.Disqualified || existingLead.StatusLead == StatusLead.Quanlified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            await CheckValidateLead(leadDTO);

            _mapper.Map(leadDTO, existingLead);
            _leadRepository.Update(existingLead);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<LeadDTO>(existingLead);
        }

        private async Task CheckValidateLead(LeadUpdateDTO leadDTO)
        {
            if (!await IsExistAccount(leadDTO.AccountId))
            {
                throw new NotFoundHttpException("Account is not found");
            }

            if (leadDTO.StatusLead == StatusLead.Disqualified || leadDTO.StatusLead == StatusLead.Quanlified)
            {
                throw new BadRequestHttpException("Couldn't Update StatusLead to disqualified or qualified");
            }
        }

        private async Task<Lead> FindLeadAsync(Guid id)
        {
            return await _leadRepository.GetAsync(l => l.Id == id)
                ?? throw new NotFoundHttpException("Lead is not found");
        }
    }
}
