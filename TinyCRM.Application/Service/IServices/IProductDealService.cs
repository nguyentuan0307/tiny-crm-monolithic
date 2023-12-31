﻿using TinyCRM.Application.Models.ProductDeal;

namespace TinyCRM.Application.Service.IServices;

public interface IProductDealService
{
    public Task<ProductDealDto> CreateProductDealAsync(Guid dealId, ProductDealCreateDto productDealDto);

    public Task DeleteProductDealAsync(Guid dealId, Guid productDealId);

    public Task<ProductDealDto> GetProductDealAsync(Guid dealId, Guid productDealId);

    public Task<List<ProductDealDto>> GetProductDealsAsync(Guid dealId, ProductDealSearchDto search);

    public Task<ProductDealDto> UpdateProductDealAsync(Guid dealId, Guid productDealId,
        ProductDealUpdateDto productDealDto);
}