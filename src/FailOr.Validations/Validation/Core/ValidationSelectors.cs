using System.Linq.Expressions;
using System.Reflection;

namespace FailOr.Validation.Core;

internal static class ValidationSelectors
{
    internal static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> propertySelector)
    {
        ArgumentNullException.ThrowIfNull(propertySelector);

        var expression = propertySelector.Body;

        if (expression is not MemberExpression memberExpression)
        {
            throw new ArgumentException(
                "Property selector must target a property access.",
                nameof(propertySelector)
            );
        }

        if (memberExpression.Member is not PropertyInfo property)
        {
            throw new ArgumentException(
                "Property selector must target a property access.",
                nameof(propertySelector)
            );
        }

        EnsurePropertyChain(memberExpression, propertySelector);
        return property.Name;
    }

    private static void EnsurePropertyChain<T, TProp>(
        MemberExpression memberExpression,
        Expression<Func<T, TProp>> propertySelector
    )
    {
        Expression? current = memberExpression;

        while (current is MemberExpression currentMemberExpression)
        {
            if (currentMemberExpression.Member.MemberType is not MemberTypes.Property)
            {
                throw new ArgumentException(
                    "Property selector must target a property access.",
                    nameof(propertySelector)
                );
            }

            current = currentMemberExpression.Expression;
        }

        if (current is not ParameterExpression)
        {
            throw new ArgumentException(
                "Property selector must target a property access.",
                nameof(propertySelector)
            );
        }
    }
}
