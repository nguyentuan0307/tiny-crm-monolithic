using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification.Base;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();

    Specification<T> And(Specification<T> specification);
}