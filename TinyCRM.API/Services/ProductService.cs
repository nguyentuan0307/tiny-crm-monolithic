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
            var product = _mapper.Map<Product>(productDTO);
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();
            await Console.Out.WriteLineAsync($"{product.Id}");
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == id;
            var product = await _productRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Product is not found");
            _productRepository.Remove(product);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == id;
            var product = await _productRepository.GetAsync(expression) ?? throw new NotFoundHttpException("Product is not found");
            return _mapper.Map<ProductDTO>(product);
        }
        public async Task<List<ProductDTO>> GetProductsAsync(ProductSearchDTO search)
        {
            Expression<Func<Product, bool>> expression = p => string.IsNullOrEmpty(search.Filter) || p.Name.Contains(search.Filter);
            var query = _productRepository.List(expression)
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount);

            List<Product> products = await query.ToListAsync();
            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(products);

            return productDTOs;
        }

        public async Task<ProductDTO> UpdateProductAsync(string id, ProductUpdateDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                throw new BadRequestHttpException("ID provided does not match the ID in the Product");
            }

            Product existingProduct = await _productRepository.GetAsync(p => p.Id == id) ?? throw new NotFoundHttpException("Product not found");

            existingProduct.Name = productDTO.Name;
            existingProduct.Price = productDTO.Price;
            existingProduct.TypeProduct = productDTO.TypeProduct;
            existingProduct.Status = productDTO.Status;
            _productRepository.Update(existingProduct);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductDTO>(existingProduct);
        }
    }
}
