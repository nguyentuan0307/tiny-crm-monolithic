using TinyCRM.API.Models.Deal;
using TinyCRM.API.Models.ProductDeal;

namespace TinyCRM.API.Services.IServices
{
    public interface IDealService
    {
        Task DeleteDealAsync(Guid id);
        Task<DealDTO> GetDealByIdAsync(Guid id);
        Task<IList<DealDTO>> GetDealsAsync(DealSearchDTO search);
        Task<DealStatisticDTO> GetStatisticDealAsync();
        Task<DealDTO> UpdateDealAsync(Guid id, DealUpdateDTO dealDTO);
    }
}
