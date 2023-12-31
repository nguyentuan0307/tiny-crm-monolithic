﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.Products;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class ProductRepository : Repository<Product, Guid>, IProductRepository
{
    public ProductRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public async Task<List<Product>> GetProductsAsync(ProductQueryParameters productQueryParameters)
    {
        var specification = new ProductsByFilterSpecification(productQueryParameters.KeyWord);
        return await ListAsync(specification,
            productQueryParameters.IncludeTables,
            productQueryParameters.Sorting,
            productQueryParameters.PageIndex,
            productQueryParameters.PageSize);
    }

    public Task<bool> ProductCodeIsExistAsync(string code, Guid id)
    {
        return DbSet.AnyAsync(p => p.Code.Equals(code) && p.Id != id);
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    protected override Expression<Func<Product, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}