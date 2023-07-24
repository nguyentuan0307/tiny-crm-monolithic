using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.API.Exceptions;
using TinyCRM.API.Models.Product;
using TinyCRM.API.Services.IServices;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDTO> CreateProductAsync(ProductCreateDTO productDTO)
        {
            await CheckValidate(productDTO.Code);
            var product = _mapper.Map<Product>(productDTO);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        private async Task CheckValidate(string code, Guid id = default)
        {
            var product = await _productRepository.GetAsync(p => p.Code == code);
            if (product != null && product.Id != id)
            {
                throw new BadRequestHttpException("Product code is existed");
            }
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await GetProductAsync(id);
            _productRepository.Remove(product);
            await _unitOfWork.SaveChangeAsync();
        }

        private async Task<Product> GetProductAsync(Guid id)
        {
            return await _productRepository.GetAsync(p => p.Id == id)
                ?? throw new NotFoundHttpException("Product is not found");
        }

        public async Task<ProductDTO> GetProductByIdAsync(Guid id)
        {
            var product = await GetProductAsync(id);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IList<ProductDTO>> GetProductsAsync(ProductSearchDTO search)
        {
            var includeTables = string.Empty;
            var query = _productRepository.List(GetExpression(search), includeTables, search.Sorting, search.PageIndex, search.PageSize);

            var products = await query.ToListAsync();
            var productDTOs = _mapper.Map<IList<ProductDTO>>(products);

            return productDTOs;
        }

        private static Expression<Func<Product, bool>> GetExpression(ProductSearchDTO search)
        {
            Expression<Func<Product, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
            || p.Code.Contains(search.KeyWord)
            || p.Name.Contains(search.KeyWord);
            return expression;
        }

        public async Task<ProductDTO> UpdateProductAsync(Guid id, ProductUpdateDTO productDTO)
        {
            await CheckValidate(productDTO.Code, id);

            var existingProduct = await GetProductAsync(id);

            _mapper.Map(productDTO, existingProduct);
            _productRepository.Update(existingProduct);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDTO>(existingProduct);
        }
    }
}