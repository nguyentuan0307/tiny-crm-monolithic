using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.ProductDeal;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Enums;
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

        public async Task<ProductDealDTO> CreateProductDealAsync(Guid id, ProductDealCreateDTO productDealDTO)
        {
            if (id != productDealDTO.DealId)
                throw new BadRequestHttpException("DealId is not match");
            var includeTables = "Lead.Account";
            var deal = await FindDealAsync(productDealDTO.DealId, includeTables);
            await IsExistProduct(productDealDTO.ProductId);

            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");

            var productDeal = _mapper.Map<ProductDeal>(productDealDTO);

            await _productDealRepository.AddAsync(productDeal);
            UpdateRelateCreate(deal, productDeal);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<ProductDealDTO>(productDeal);
        }

        private static void UpdateRelateCreate(Deal deal, ProductDeal productDeal)
        {
            deal.ActualRevenue += productDeal.TotalAmount;
            deal.Lead.Account.TotalSale += productDeal.TotalAmount;
        }

        private async Task<Deal> FindDealAsync(Guid dealId, string? includeTables = default)
        {
            return await _dealRepository.GetAsync(p => p.Id == dealId, includeTables) ??
                throw new NotFoundHttpException("Deal is not found");
        }

        private async Task IsExistProduct(Guid productId)
        {
            if (!await _productRepository.AnyAsync(p => p.Id == productId))
                throw new NotFoundHttpException("Product is not found");
        }

        public async Task DeleteProductDealAsync(Guid id, Guid productDealId)
        {
            var deal = await FindDealAsync(id);
            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");
            var productDeal = await FindProductDealAsync(productDealId);

            if (id != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");

            _productDealRepository.Remove(productDeal);
            UpdateRelateDelete(deal, productDeal);
            await _unitOfWork.SaveChangeAsync();
        }

        private static void UpdateRelateDelete(Deal deal, ProductDeal productDeal)
        {
            deal.ActualRevenue -= productDeal.TotalAmount;
            deal.Lead.Account.TotalSale -= productDeal.TotalAmount;
        }

        private async Task<ProductDeal> FindProductDealAsync(Guid productDealId, string? includeTables = default)
        {
            return await _productDealRepository.GetAsync(p => p.Id == productDealId, includeTables) ??
                throw new NotFoundHttpException("ProductDeal is not found");
        }

        public async Task<ProductDealDTO> GetProductDealByIdAsync(Guid id, Guid productDealId)
        {
            string includeTables = $"{nameof(ProductDeal.Product)}";
            var productDeal = await FindProductDealAsync(productDealId, includeTables);

            if (id != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");
            return _mapper.Map<ProductDealDTO>(productDeal);
        }

        public async Task<IList<ProductDealDTO>> GetProductDealsByDealIdAsync(Guid id)
        {
            var includeTables = "Product";
            var productDeals = await _productDealRepository.List(p => p.DealId == id, includeTables).ToListAsync();
            return _mapper.Map<IList<ProductDealDTO>>(productDeals);
        }

        public async Task<ProductDealDTO> UpdateProductDealAsync(Guid id, Guid productDealId, ProductDealUpdateDTO productDealDTO)
        {
            var productDeal = await FindProductDealAsync(productDealId);
            var totalAmountOld = productDeal.TotalAmount;
            if (id != productDeal.DealId)
                throw new BadRequestHttpException("DealId is not match");

            var deal = await FindDealAsync(productDeal.DealId);
            await IsExistProduct(productDealDTO.ProductId);

            if (deal.StatusDeal != StatusDeal.Open)
                throw new BadRequestHttpException("Deal is Won/Lose");

            _mapper.Map(productDealDTO, productDeal);
            _productDealRepository.Update(productDeal);
            UpdateRelateUpdate(deal, totalAmountOld, productDeal.TotalAmount);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDealDTO>(productDeal);
        }

        private static void UpdateRelateUpdate(Deal deal, decimal totalAmountOld, decimal totalAmountNew)
        {
            deal.Lead.Account.TotalSale -= deal.ActualRevenue;
            deal.ActualRevenue = deal.ActualRevenue - totalAmountOld + totalAmountNew;
            deal.Lead.Account.TotalSale += deal.ActualRevenue;
        }
    }
}