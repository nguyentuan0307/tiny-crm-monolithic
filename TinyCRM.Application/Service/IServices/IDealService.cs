using TinyCRM.Application.Models.Deal;

namespace TinyCRM.Application.Service.IServices;

public interface IDealService
{
    public Task DeleteDealAsync(Guid id);

    public Task<DealDto> GetDealAsync(Guid id);

    public Task<List<DealDto>> GetDealsAsync(DealSearchDto search);

    public Task<DealStatisticDto> GetStatisticDealAsync();

    public Task<DealDto> UpdateDealAsync(Guid id, DealUpdateDto dealDto);

    public Task<List<DealDto>> GetDealsAsync(Guid accountId, DealSearchDto search);
}