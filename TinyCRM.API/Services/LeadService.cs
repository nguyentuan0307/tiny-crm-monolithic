using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Helper.QueryParameters;
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

        public async Task<LeadDto> CreateLeadAsync(LeadCreateDto leadDto)
        {
            if (!await IsExistAccount(leadDto.AccountId))
            {
                throw new NotFoundHttpException("Account is not found");
            }

            var lead = _mapper.Map<Lead>(leadDto);
            lead.StatusLead = StatusLead.Prospect;

            await _leadRepository.AddAsync(lead);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<LeadDto>(lead);
        }

        private async Task<bool> IsExistAccount(Guid accountId)
        {
            return await _accountRepository.AnyAsync(accountId);
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await FindLeadAsync(id);
            _leadRepository.Remove(lead);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DisqualifyLeadAsync(Guid id, DisqualifyDto disqualifyDto)
        {
            var existingLead = await FindLeadAsync(id);

            if (existingLead.StatusLead is StatusLead.Disqualified or StatusLead.Qualified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            _mapper.Map(disqualifyDto, existingLead);
            _leadRepository.Update(existingLead);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<LeadDto> GetLeadByIdAsync(Guid id)
        {
            var lead = await FindLeadAsync(id);
            return _mapper.Map<LeadDto>(lead);
        }

        public async Task<IList<LeadDto>> GetLeadsAsync(LeadSearchDto search)
        {
            const string includeTables = "Account";
            var sorting = ConvertSort(search);

            var leadQueryParameters = new LeadQueryParameters
            {
                KeyWord = search.KeyWord,
                Sorting = sorting,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                IncludeTables = includeTables,
            };
            var leads = await _leadRepository.GetLeads(leadQueryParameters).ToListAsync();

            return _mapper.Map<IList<LeadDto>>(leads);
        }

        private static string ConvertSort(LeadSearchDto search)
        {
            if (search.SortFilter == null) return string.Empty;

            var sort = search.SortFilter.ToString() switch
            {
                "Title" => "Title",
                "AccountName" => "Account.Name",
                _ => "Id"
            };
            sort = search.SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }

        public async Task<DealDto> QualifyLeadAsync(Guid id)
        {
            var existingLead = await GetSatisfiedLead(id);

            existingLead.StatusLead = StatusLead.Qualified;
            _leadRepository.Update(existingLead);

            var deal = new Deal
            {
                LeadId = existingLead.Id,
                Title = existingLead.Title,
                StatusDeal = StatusDeal.Open
            };

            await _dealRepository.AddAsync(deal);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<DealDto>(deal);
        }

        private async Task<Lead> GetSatisfiedLead(Guid id)
        {
            var existingLead = await FindLeadAsync(id);
            if (existingLead.StatusLead is StatusLead.Disqualified or StatusLead.Qualified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            if (_dealRepository.IsExistingLead(existingLead.Id))
            {
                throw new BadRequestHttpException("This lead already exists Deal");
            }

            return existingLead;
        }

        public async Task<LeadDto> UpdateLeadAsync(Guid id, LeadUpdateDto leadDto)
        {
            var existingLead = await FindLeadAsync(id);

            if (existingLead.StatusLead is StatusLead.Disqualified or StatusLead.Qualified)
            {
                throw new BadRequestHttpException("Lead is disqualified or qualified");
            }

            await CheckValidateLead(leadDto);

            _mapper.Map(leadDto, existingLead);
            _leadRepository.Update(existingLead);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<LeadDto>(existingLead);
        }

        private async Task CheckValidateLead(LeadUpdateDto leadDto)
        {
            if (!await IsExistAccount(leadDto.AccountId))
            {
                throw new NotFoundHttpException("Account is not found");
            }

            if (leadDto.StatusLead == StatusLead.Disqualified || leadDto.StatusLead == StatusLead.Qualified)
            {
                throw new BadRequestHttpException("Couldn't Update StatusLead to disqualified or qualified");
            }
        }

        private async Task<Lead> FindLeadAsync(Guid id)
        {
            return await _leadRepository.GetAsync(id)
                ?? throw new NotFoundHttpException("Lead is not found");
        }

        public async Task<LeadStatisticDto> GetStatisticLeadAsync()
        {
            var statisticLeads = await _leadRepository.GetLeadStatistics().ToListAsync();

            if (statisticLeads.Count == 0)
            {
                return new LeadStatisticDto();
            }

            var leadStatisticDto = new LeadStatisticDto
            {
                OpenLeads = statisticLeads.Count(x => x.StatusLead == StatusLead.Open),
                ProspectLeads = statisticLeads.Count(x => x.StatusLead == StatusLead.Prospect),
                QualifiedLeads = statisticLeads.Count(x => x.StatusLead == StatusLead.Qualified),
                DisqualifiedLeads = statisticLeads.Count(x => x.StatusLead == StatusLead.Disqualified),
                AvgEstimatedRevenue = statisticLeads.Average(x => x.EstimatedRevenue)
            };
            return leadStatisticDto;
        }

        public async Task<IList<LeadDto>> GetLeadsByAccountIdAsync(Guid accountId, LeadSearchDto search)
        {
            if (!await _accountRepository.AnyAsync(accountId))
            {
                throw new BadRequestHttpException("Account is not found");
            }
            const string includeTables = "Account";
            var sorting = ConvertSort(search);

            var leadQueryParameters = new LeadQueryParameters
            {
                KeyWord = search.KeyWord,
                Sorting = sorting,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                IncludeTables = includeTables,
                AccountId = accountId
            };
            var leads = await _leadRepository.GetLeadsByAccountId(leadQueryParameters).ToListAsync();
            return _mapper.Map<IList<LeadDto>>(leads);
        }
    }
}