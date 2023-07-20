using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.Lead;

namespace TinyCRM.API.Services.IServices
{
    public interface ILeadService
    {
        public Task<LeadDTO> CreateLeadAsync(LeadCreateDTO leadDTO);

        public Task DeleteLeadAsync(Guid id);

        Task DisqualifyLeadAsync(Guid id, DisqualifyDTO disqualifyDTO);

        public Task<LeadDTO> GetLeadByIdAsync(Guid id);

        public Task<IList<LeadDTO>> GetLeadsAsync(LeadSearchDTO search);

        Task<LeadStatisticDTO> GetStatisticLeadAsync();

        Task<DealDTO> QualifyLeadAsync(Guid id);

        public Task<LeadDTO> UpdateLeadAsync(Guid id, LeadUpdateDTO leadDTO);
    }
}