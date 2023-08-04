using AutoMapper;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Enums;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class ProductDealService : IProductDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductDealRepository _productDealRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDealService(IDealRepository dealRepository, IProductRepository productRepository,
            IProductDealRepository productDealRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _dealRepository = dealRepository;
            _productRepository = productRepository;
            _productDealRepository = productDealRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDealDto> CreateProductDealAsync(Guid productDealId, ProductDealCreateDto productDealDto)
        {
            if (productDealId != productDealDto.DealId)
                throw new BadRequestHttpException("DealId is not match");
            const string includeTables = "Lead.Account";
            var deal = await FindDealAsync(productDealDto.DealId, includeTables);
            await IsExistProduct(productDealDto.ProductId);

            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");

            var productDeal = _mapper.Map<ProductDeal>(productDealDto);

            await _productDealRepository.AddAsync(productDeal);
            UpdateRelateCreate(deal, productDeal);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<ProductDealDto>(productDeal);
        }

        private static void UpdateRelateCreate(Deal deal, ProductDeal productDeal)
        {
            deal.ActualRevenue += productDeal.TotalAmount;
            deal.Lead.Account.TotalSales += productDeal.TotalAmount;
        }

        private async Task<Deal> FindDealAsync(Guid dealId, string? includeTables = default)
        {
            return await _dealRepository.GetAsync(dealId, includeTables) ??
                throw new NotFoundHttpException("Deal is not found");
        }

        private async Task IsExistProduct(Guid productId)
        {
            if (!await _productRepository.AnyAsync(productId))
                throw new NotFoundHttpException("Product is not found");
        }

        public async Task DeleteProductDealAsync(Guid dealId, Guid productDealId)
        {
            var deal = await FindDealAsync(dealId);

            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");

            var productDeal = await FindProductDealAsync(productDealId);

            if (dealId != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");

            _productDealRepository.Remove(productDeal);
            UpdateRelateDelete(deal, productDeal);
            await _unitOfWork.SaveChangeAsync();
        }

        private static void UpdateRelateDelete(Deal deal, ProductDeal productDeal)
        {
            deal.ActualRevenue -= productDeal.TotalAmount;
            deal.Lead.Account.TotalSales -= productDeal.TotalAmount;
        }

        private async Task<ProductDeal> FindProductDealAsync(Guid productDealId, string? includeTables = default)
        {
            return await _productDealRepository.GetAsync(productDealId, includeTables) ??
                throw new NotFoundHttpException("ProductDeal is not found");
        }

        public async Task<ProductDealDto> GetProductDealByIdAsync(Guid dealId, Guid productDealId)
        {
            const string includeTables = $"{nameof(ProductDeal.Product)}";
            var productDeal = await FindProductDealAsync(productDealId, includeTables);

            if (dealId != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");
            return _mapper.Map<ProductDealDto>(productDeal);
        }

        public async Task<IList<ProductDealDto>> GetProductDealsByDealIdAsync(Guid dealId, ProductDealSearchDto search)
        {
            const string includeTables = "Product";
            var productDealsQueryParameter = new ProductDealQueryParameters
            {
                KeyWord = search.KeyWord,
                Sorting = search.ConvertSort(),
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                IncludeTables = includeTables,
                DealId = dealId
            };
            var productDeals = await _productDealRepository.GetProductDealsByDealIdAsync(productDealsQueryParameter);
            return _mapper.Map<IList<ProductDealDto>>(productDeals);
        }

        public async Task<ProductDealDto> UpdateProductDealAsync(Guid dealId, Guid productDealId, ProductDealUpdateDto productDealDto)
        {
            var productDeal = await FindProductDealAsync(productDealId);
            var totalAmountOld = productDeal.TotalAmount;
            if (dealId != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");

            var deal = await FindDealAsync(productDeal.DealId);
            await IsExistProduct(productDealDto.ProductId);

            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");

            _mapper.Map(productDealDto, productDeal);
            _productDealRepository.Update(productDeal);
            UpdateRelateUpdate(deal, totalAmountOld, productDeal.TotalAmount);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDealDto>(productDeal);
        }

        private static void UpdateRelateUpdate(Deal deal, decimal totalAmountOld, decimal totalAmountNew)
        {
            deal.Lead.Account.TotalSales -= deal.ActualRevenue;
            deal.ActualRevenue = deal.ActualRevenue - totalAmountOld + totalAmountNew;
            deal.Lead.Account.TotalSales += deal.ActualRevenue;
        }
    }
}