using System.Linq.Expressions;

namespace TinyCRM.Infrastructure.Helpers.Extensions;

internal static class ExpressionsExtensions
{
    public static Expression<Func<T, bool>> JoinWithOr<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftBody = left.Body;
        var rightBody = right.Body;

        var joinedExpression = Expression.OrElse(leftBody, rightBody);

        return Expression.Lambda<Func<T, bool>>(joinedExpression, parameter);
    }

    public static Expression<Func<T, bool>> JoinWithAnd<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftBody = left.Body;
        var rightBody = right.Body;

        var joinedExpression = Expression.AndAlso(leftBody, rightBody);

        return Expression.Lambda<Func<T, bool>>(joinedExpression, parameter);
    }
}