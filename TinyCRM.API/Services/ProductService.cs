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

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto productDto)
        {
            await CheckValidate(productDto.Code);
            var product = _mapper.Map<Product>(productDto);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDto>(product);
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

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await GetProductAsync(id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IList<ProductDto>> GetProductsAsync(ProductSearchDto search)
        {
            var includeTables = string.Empty;
            var expression = GetExpression(search);
            var sorting = ConvertSort(search);
            var query = _productRepository.List(expression, includeTables, sorting, search.PageIndex, search.PageSize);

            var products = await query.ToListAsync();
            var productDtOs = _mapper.Map<IList<ProductDto>>(products);

            return productDtOs;
        }

        private static string ConvertSort(ProductSearchDto search)
        {
            var sort = search.SortFilter.ToString() switch
            {
                "Id" => "Id",
                "Code" => "Code",
                "Name" => "Name",
                "Price" => "Price",
                _ => "Code"
            };
            sort = search.SortDirection ? $"{sort} asc" : $"{sort} desc";
            return sort;
        }

        private static Expression<Func<Product, bool>> GetExpression(ProductSearchDto search)
        {
            Expression<Func<Product, bool>> expression = p => string.IsNullOrEmpty(search.KeyWord)
            || p.Code.Contains(search.KeyWord)
            || p.Name.Contains(search.KeyWord);
            return expression;
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto productDto)
        {
            await CheckValidate(productDto.Code, id);

            var existingProduct = await GetProductAsync(id);

            _mapper.Map(productDto, existingProduct);
            _productRepository.Update(existingProduct);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDto>(existingProduct);
        }
    }
}