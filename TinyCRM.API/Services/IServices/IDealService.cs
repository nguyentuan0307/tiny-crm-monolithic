using TinyCRM.API.Models.Deal;

namespace TinyCRM.API.Services.IServices
{
    public interface IDealService
    {
        public Task DeleteDealAsync(Guid id);

        public Task<DealDTO> GetDealByIdAsync(Guid id);

        public Task<IList<DealDTO>> GetDealsAsync(DealSearchDTO search);

        public Task<DealStatisticDTO> GetStatisticDealAsync();

        public Task<DealDTO> UpdateDealAsync(Guid id, DealUpdateDTO dealDTO);

        public Task<IList<DealDTO>> GetDealsByAccountIdAsync(Guid accountId);
    }
}