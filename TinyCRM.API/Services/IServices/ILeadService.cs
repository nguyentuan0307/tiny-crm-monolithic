using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;

namespace TinyCRM.API.Services.IServices
{
    public interface ILeadService
    {
        public Task<LeadDto> CreateLeadAsync(LeadCreateDto leadDto);

        public Task DeleteLeadAsync(Guid id);

        public Task DisqualifyLeadAsync(Guid id, DisqualifyDto disqualifyDto);

        public Task<LeadDto> GetLeadByIdAsync(Guid id);

        public Task<IList<LeadDto>> GetLeadsAsync(LeadSearchDto search);

        public Task<IList<LeadDto>> GetLeadsByAccountIdAsync(Guid accountId, LeadSearchDto search);

        public Task<LeadStatisticDto> GetStatisticLeadAsync();

        public Task<DealDto> QualifyLeadAsync(Guid id);

        public Task<LeadDto> UpdateLeadAsync(Guid id, LeadUpdateDto leadDto);
    }
}