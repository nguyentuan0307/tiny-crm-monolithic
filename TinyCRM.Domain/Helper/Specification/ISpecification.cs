using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> IsSatisfiedBy();
}