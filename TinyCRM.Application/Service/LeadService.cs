using AutoMapper;
using TinyCRM.Application.Models.Deal;
using TinyCRM.Application.Models.Lead;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Application.Service;

public class LeadService : ILeadService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IDealRepository _dealRepository;
    private readonly ILeadRepository _leadRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LeadService(ILeadRepository leadRepository, IMapper mapper, IUnitOfWork unitOfWork,
        IDealRepository dealRepository, IAccountRepository accountRepository)
    {
        _leadRepository = leadRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _dealRepository = dealRepository;
        _accountRepository = accountRepository;
    }

    public async Task<LeadDto> CreateLeadAsync(LeadCreateDto leadDto)
    {
        await CheckValidateAccount(leadDto.AccountId);

        var lead = _mapper.Map<Lead>(leadDto);
        lead.StatusLead = StatusLead.Prospect;

        await _leadRepository.AddAsync(lead);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<LeadDto>(lead);
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
            throw new InvalidUpdateException($"Lead[{existingLead.Title}] is disqualified or qualified");

        _mapper.Map(disqualifyDto, existingLead);
        _leadRepository.Update(existingLead);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<LeadDto> GetLeadAsync(Guid id)
    {
        var lead = await FindLeadAsync(id);
        return _mapper.Map<LeadDto>(lead);
    }

    public async Task<List<LeadDto>> GetLeadsAsync(LeadSearchDto search)
    {
        const string includeTables = "Account";
        var leadQueryParameters = new LeadQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize,
            IncludeTables = includeTables
        };
        var leads = await _leadRepository.GetLeadsAsync(leadQueryParameters);

        return _mapper.Map<List<LeadDto>>(leads);
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

    public async Task<LeadDto> UpdateLeadAsync(Guid id, LeadUpdateDto leadDto)
    {
        var existingLead = await FindLeadAsync(id);

        CheckValidateStatusLead(existingLead.StatusLead);
        await CheckValidateAccount(leadDto.AccountId);
        CheckValidateStatusLead(leadDto.StatusLead);

        _mapper.Map(leadDto, existingLead);
        _leadRepository.Update(existingLead);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<LeadDto>(existingLead);
    }

    public async Task<LeadStatisticDto> GetStatisticLeadAsync()
    {
        var statisticLeads = await _leadRepository.GetLeadStatisticsAsync();

        if (statisticLeads.Count == 0) return new LeadStatisticDto();

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

    public async Task<List<LeadDto>> GetLeadsAsync(Guid accountId, LeadSearchDto search)
    {
        await CheckValidateAccount(accountId);
        const string includeTables = "Account";

        var leadQueryParameters = new LeadQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize,
            IncludeTables = includeTables,
            AccountId = accountId
        };
        var leads = await _leadRepository.GetLeadsByAccountIdAsync(leadQueryParameters);
        return _mapper.Map<List<LeadDto>>(leads);
    }

    private async Task CheckValidateAccount(Guid accountId)
    {
        if (!await _accountRepository.AnyAsync(accountId))
            throw new EntityNotFoundException($"Account with Id[{accountId} is not found");
    }

    private async Task<Lead> GetSatisfiedLead(Guid id)
    {
        var existingLead = await FindLeadAsync(id);
        if (existingLead.StatusLead is StatusLead.Disqualified or StatusLead.Qualified)
            throw new InvalidUpdateException($"Lead[{existingLead.Title}] is disqualified or qualified");

        if (_dealRepository.IsExistingLead(existingLead.Id))
            throw new InvalidUpdateException($"Lead[{existingLead.Title}] already exists Deal");

        return existingLead;
    }

    private static void CheckValidateStatusLead(StatusLead status)
    {
        if (status is StatusLead.Disqualified or StatusLead.Qualified)
            throw new InvalidUpdateException("Couldn't Update StatusLead to disqualified or qualified");
    }

    private async Task<Lead> FindLeadAsync(Guid id)
    {
        return await _leadRepository.GetAsync(id)
               ?? throw new EntityNotFoundException($"Lead with Id[{id}] is not found");
    }
}