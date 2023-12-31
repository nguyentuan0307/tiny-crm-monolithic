﻿using AutoMapper;
using TinyCRM.Application.Models.Product;
using TinyCRM.Application.Service.IServices;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Exceptions;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Application.Service;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
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

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await FindProductAsync(id);
        _productRepository.Remove(product);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<ProductDto> GetProductAsync(Guid id)
    {
        var product = await FindProductAsync(id);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<List<ProductDto>> GetProductsAsync(ProductSearchDto search)
    {
        var includeTables = string.Empty;
        var productQueryParameter = new ProductQueryParameters
        {
            KeyWord = search.KeyWord,
            Sorting = search.ConvertSort(),
            PageIndex = search.PageIndex,
            PageSize = search.PageSize,
            IncludeTables = includeTables
        };

        var products = await _productRepository.GetProductsAsync(productQueryParameter);
        var productDtOs = _mapper.Map<List<ProductDto>>(products);

        return productDtOs;
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto productDto)
    {
        await CheckValidate(productDto.Code, id);

        var existingProduct = await FindProductAsync(id);

        _mapper.Map(productDto, existingProduct);
        _productRepository.Update(existingProduct);
        await _unitOfWork.SaveChangeAsync();
        return _mapper.Map<ProductDto>(existingProduct);
    }

    private async Task CheckValidate(string code, Guid id = default)
    {
        if (await _productRepository.ProductCodeIsExistAsync(code, id))
            throw new InvalidUpdateException($"Product Code[{code}] is exist");
    }

    private async Task<Product> FindProductAsync(Guid id)
    {
        return await _productRepository.GetAsync(id)
               ?? throw new EntityNotFoundException($"Product with Id[{id}]is not found");
    }
}