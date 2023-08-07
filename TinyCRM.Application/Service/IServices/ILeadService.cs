using TinyCRM.Application.Models.Deal;
using TinyCRM.Application.Models.Lead;

namespace TinyCRM.Application.Service.IServices;

public interface ILeadService
{
    public Task<LeadDto> CreateLeadAsync(LeadCreateDto leadDto);

    public Task DeleteLeadAsync(Guid id);

    public Task DisqualifyLeadAsync(Guid id, DisqualifyDto disqualifyDto);

    public Task<LeadDto> GetLeadAsync(Guid id);

    public Task<List<LeadDto>> GetLeadsAsync(LeadSearchDto search);

    public Task<List<LeadDto>> GetLeadsAsync(Guid accountId, LeadSearchDto search);

    public Task<LeadStatisticDto> GetStatisticLeadAsync();

    public Task<DealDto> QualifyLeadAsync(Guid id);

    public Task<LeadDto> UpdateLeadAsync(Guid id, LeadUpdateDto leadDto);
}