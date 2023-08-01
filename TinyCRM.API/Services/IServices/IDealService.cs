using TinyCRM.API.Models.Deal;

namespace TinyCRM.API.Services.IServices
{
    public interface IDealService
    {
        public Task DeleteDealAsync(Guid id);

        public Task<DealDto> GetDealByIdAsync(Guid id);

        public Task<IList<DealDto>> GetDealsAsync(DealSearchDto search);

        public Task<DealStatisticDto> GetStatisticDealAsync();

        public Task<DealDto> UpdateDealAsync(Guid id, DealUpdateDto dealDto);

        public Task<IList<DealDto>> GetDealsByAccountIdAsync(Guid accountId, DealSearchDto search);
    }
}