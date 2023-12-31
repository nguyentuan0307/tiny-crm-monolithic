﻿using System.Linq.Expressions;
using TinyCRM.Domain.Helper.Specification.Base;
using TinyCRM.Infrastructure.Identity.Users;

namespace TinyCRM.Infrastructure.Identity.Specification;

public class UsersByFilterSpecification : Specification<ApplicationUser>, ISpecification<ApplicationUser>
{
    private readonly string? _keyWord;

    public UsersByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<ApplicationUser, bool>> ToExpression()
    {
        Expression<Func<ApplicationUser, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
            expression = p => p.Name.Contains(_keyWord)
                              || p.Email!.Contains(_keyWord);

        return expression;
    }
}