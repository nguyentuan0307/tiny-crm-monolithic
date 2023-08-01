﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification;
using TinyCRM.Infrastructure.Helpers.Model;

namespace TinyCRM.Infrastructure.Repositories
{
    public class DealRepository : Repository<Deal, Guid>, IDealRepository
    {
        public DealRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsExistingLead(Guid leadId)
        {
            return DbSet.Any(d => d.LeadId == leadId);
        }

        public IQueryable<Deal> GetDeals(DealQueryParameters dealQueryParameters)
        {
            var filterDealsSpecification = new DealsSpecification(dealQueryParameters.KeyWord);
            return List(specification: filterDealsSpecification,
                includeTables: dealQueryParameters.IncludeTables,
                sorting: dealQueryParameters.Sorting,
                pageIndex: dealQueryParameters.PageIndex,
                pageSize: dealQueryParameters.PageSize);
        }

        public IQueryable<Deal> GetDealsByAccountId(DealQueryParameters dealQueryParameters)
        {
            var filterDealsSpecification = new DealsByAccountIdSpecification(dealQueryParameters.KeyWord, dealQueryParameters.AccountId!.Value);
            return List(specification: filterDealsSpecification,
                includeTables: dealQueryParameters.IncludeTables,
                sorting: dealQueryParameters.Sorting,
                pageIndex: dealQueryParameters.PageIndex,
                pageSize: dealQueryParameters.PageSize);
        }

        public IQueryable<DealStatistic> GetDealStatistics()
        {
            return DbSet.Select(x => new DealStatistic
            {
                StatusDeal = x.StatusDeal,
                ActualRevenue = x.ActualRevenue
            });
        }

        protected override Expression<Func<Deal, bool>> ExpressionForGet(Guid id)
        {
            return p => p.Id == id;
        }

        public override Task<bool> AnyAsync(Guid id)
        {
            return DbSet.AnyAsync(p => p.Id == id);
        }
    }
}