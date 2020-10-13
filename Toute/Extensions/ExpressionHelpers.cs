using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Toute
{
    /// <summary>
    /// Helpers for expressions
    /// </summary>
    public static class ExpressionHelpers
    {
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        public static T GetPropertyValue<In, T>(this Expression<Func<In, T>> lambda, In input)
        {
            return lambda.Compile().Invoke(input);
        }

        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            var expression = (lambda as LambdaExpression).Body as MemberExpression;

            var propertyInfo = (PropertyInfo)expression.Member;
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            propertyInfo.SetValue(target, value);
        }

        public static void SetPropertyValue<In, T>(this Expression<Func<In, T>> lambda, T value, In input)
        {
            var expression = (lambda as LambdaExpression).Body as MemberExpression;

            var propertyInfo = (PropertyInfo)expression.Member;

            propertyInfo.SetValue(input, value);
        }
    }
}
