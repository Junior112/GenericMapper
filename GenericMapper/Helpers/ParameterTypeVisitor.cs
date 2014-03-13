using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GenericMapper.Classes;

namespace GenericMapper.Helpers
{
    internal class ParameterTypeVisitor<TFrom, TTo> : ExpressionVisitor
    {

        private readonly Dictionary<string, ParameterExpression> _convertedParameters = new Dictionary<string, ParameterExpression>();
        private readonly Expression<Func<TFrom, bool>> _expression;
        public ParameterTypeVisitor(Expression<Func<TFrom, bool>> expresionToConvert)
        {
            //for each parameter in the original expression creates a new parameter with the same name but with changed type 
            _convertedParameters = expresionToConvert.Parameters
                .ToDictionary(
                    x => x.Name,
                    x => Expression.Parameter(typeof(TTo), x.Name)
                );
            _expression = expresionToConvert;
        }

        public Expression<Func<TTo, bool>> Convert()
        {
            return (Expression<Func<TTo, bool>>)Visit(_expression);
        }
        
        private string nameParameter;
        private string nameProperty;
        //handles Properties and Fields accessors 
        protected override Expression VisitMember(MemberExpression node)
        {
            nameProperty = node.Member.Name;
            //we want to replace only the nodes of type TFrom
            //so we can handle expressions of the form x=> x.Property.SubProperty
            //in the expression x=> x.Property1 == 6 && x.Property2 == 3
            //this replaces         ^^^^^^^^^^^         ^^^^^^^^^^^            
            if (node.Member.DeclaringType != typeof(TFrom))
            {
                return base.VisitMember(node);
            }
            else
            {
                nameParameter = _convertedParameters.ToList()[_convertedParameters.Count - 1].Key;
                
                var attribute = node.Member.GetCustomAttribute<MapAttribute>();
                var name = attribute == null ? node.Member.Name : attribute.NameProperty;
                var names = name.Split(".".ToArray());

                if (names.Length > 1)
                {
                    var dynamicExpression = System.Linq.Dynamic.DynamicExpression.ParseLambda<TTo, bool>(attribute.NameProperty + "!=null");
                    var left = (MemberExpression)((BinaryExpression)dynamicExpression.Body).Left;

                    return base.VisitMember(left);
                }
                var memeberInfo = typeof(TTo).GetMember(name).First();

                //this will actually call the VisitParameter method in this class
                var newExp = Visit(node.Expression);
                return Expression.MakeMemberAccess(newExp, memeberInfo);
            }
        }

        // this will be called where ever we have a reference to a parameter in the expression
        // for ex. in the expression x=> x.Property1 == 6 && x.Property2 == 3
        // this will be called twice     ^                   ^
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return string.IsNullOrEmpty(node.Name) ? _convertedParameters[nameParameter] : _convertedParameters[node.Name];
        }

        //this will be the first Visit method to be called
        //since we're converting LamdaExpressions
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            //for each parameter in the original expression creates a new parameter with the same name but with changed type 
            if (_convertedParameters.All(x => node.Parameters.All(y => y.Name != x.Key)))
            {
                var dic = node.Parameters.ToDictionary(x => x.Name, x => Expression.Parameter(typeof(T), x.Name));
                foreach (var parameterExpression in dic)
                {
                    _convertedParameters.Add(parameterExpression.Key, parameterExpression.Value);
                }
            }

            if (!string.IsNullOrEmpty(nameProperty))
            {
                var attribute = typeof (TFrom).GetProperty(nameProperty).GetCustomAttribute<MapAttribute>();
                var name = attribute == null ? node.Name : attribute.NameProperty;
                if (attribute != null)
                {
                    if (attribute.IsDto)
                    {
                        throw new Exception("This is not valid expression.");
                        /*
                        var exp = this.InvokeGeneric("GetExpression", 
                                                    typeof(TTo).GetProperty(attribute.NameProperty).PropertyType, 
                                                    typeof(TFrom).GetProperty(nameProperty).PropertyType, 
                                                    Expression.Lambda(node.Body, node.Parameters));
                        */
                    }
                }
            }

            //visit the body of the lambda, this will Traverse the ExpressionTree 
            //and recursively replace parts of the expresion we for witch we have matching Visit methods 
            var newExp = Visit(node.Body);

            //this will create the new expression            
            return Expression.Lambda(newExp, _convertedParameters.Select(x => x.Value));
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            var operand = this.Visit(node.Operand);
            return operand != node.Operand ? Expression.MakeUnary(node.NodeType, operand, node.Type, node.Method) : node;
        }

        private Expression<Func<T2, bool>> GetExpression<T, T2>(LambdaExpression expression)
        {
            var visitor = new ParameterTypeVisitor<T, T2>((Expression<Func<T,bool>>)expression);
            return visitor.Convert();
        }
    }
}