using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification.Base;

public abstract class Specification<TEntity> : ISpecification<TEntity>
{
    public abstract Expression<Func<TEntity, bool>> ToExpression();

    public Specification<TEntity> And(Specification<TEntity> specification)
    {
        return new AndSpecification<TEntity>(this, specification);
    }
}