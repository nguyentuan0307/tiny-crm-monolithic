using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification;

public class AndSpecifications<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecifications(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public Expression<Func<T, bool>> IsSatisfiedBy()
    {
        var leftExpression = _left.IsSatisfiedBy();
        var rightExpression = _right.IsSatisfiedBy();

        var body = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
        var lambda = Expression.Lambda<Func<T, bool>>(body, leftExpression.Parameters);

        return lambda;
    }
}

public class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public Expression<Func<T, bool>> IsSatisfiedBy()
    {
        var leftExpression = _left.IsSatisfiedBy();
        var rightExpression = _right.IsSatisfiedBy();

        var body = Expression.OrElse(leftExpression.Body, rightExpression.Body);
        var lambda = Expression.Lambda<Func<T, bool>>(body, leftExpression.Parameters);

        return lambda;
    }
}