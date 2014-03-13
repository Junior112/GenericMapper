using System;
using System.Linq.Expressions;
using GenericMapper.Helpers;

namespace GenericMapper.Extensions
{
    public static class Expressions
    {
        public static Expression<Func<TEntity, bool>> ConvertToExpressionEntity<TDto, TEntity>(this Expression<Func<TDto, bool>> expression)
            where TDto : class, new()
            where TEntity : class
        {
            var visitor = new ParameterTypeVisitor<TDto, TEntity>(expression);
            return visitor.Convert();
        }
    }
}
