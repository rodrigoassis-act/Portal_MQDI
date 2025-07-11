using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ONS.PortalMQDI.Shared.Utils
{
    /// <summary>  
    /// Classe para construção de predicados para consultas LINQ  
    /// </summary> 
    public static class PredicateBuilder
    {
        /// <summary>  
        /// Cria um predicado que avalia como verdadeiro.  
        /// </summary>  
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>  
        /// Cria um predicado que avalia como falso.  
        /// </summary>  
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>  
        /// Cria uma expressão de predicado a partir da expressão lambda especificada.  
        /// </summary>  
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>  
        /// Combina o primeiro predicado com o segundo usando o operador lógico "e".  
        /// </summary>  
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>  
        /// Combina o primeiro predicado com o segundo usando o operador lógico "ou".  
        /// </summary>  
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>  
        /// Nega o predicado.  
        /// </summary>  
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>  
        /// Combina a primeira expressão com a segunda usando a função de mesclagem especificada.  
        /// </summary>  
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (map.TryGetValue(p, out ParameterExpression replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
