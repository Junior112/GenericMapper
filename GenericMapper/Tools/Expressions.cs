using GenericMapper.Helpers;
using System;
using System.Linq.Expressions;

namespace GenericMapper.Tools
{
    public class Expressions
    {
        public Expression<Func<TEntity, bool>> ConvertToExpressionEntity<TDto, TEntity>(Expression<Func<TDto, bool>> expression)
        {
            var visitor = new ParameterTypeVisitor<TDto, TEntity>(expression);
            return visitor.Convert();
        }
    }
}
