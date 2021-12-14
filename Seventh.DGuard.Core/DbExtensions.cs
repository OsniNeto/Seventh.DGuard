using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Seventh.DGuard.Core
{
    public static class DbExtensions
    {
        #region OrderBy

        private static readonly MethodInfo OrderByMethod =
        typeof(Queryable).GetMethods().Single(method =>
        method.Name == "OrderBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod =
            typeof(Queryable).GetMethods().Single(method =>
            method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        public static bool PropertyExists<T>(this IQueryable<T> source, string propertyName)
        {
            return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) != null;
        }

        public static IQueryable<T> OrderByProperty<T>(
            this IQueryable<T> source, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return null;
            }
            ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            MethodInfo genericMethod =
                OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }

        public static IQueryable<T> OrderByPropertyDescending<T>(
            this IQueryable<T> source, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return null;
            }
            ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            MethodInfo genericMethod =
                OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }
        #endregion

        #region Reflection filter

        public static Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity, TFilter>(TFilter filtro)
        {
            var paramterExpression = Expression.Parameter(typeof(TEntity), "s");

            var expressions = new List<Expression>();

            foreach (var propertyInfo in filtro.GetType().GetProperties())
                if (IsProp(propertyInfo.PropertyType))
                {
                    var expression = CreateExpresion(propertyInfo, filtro, paramterExpression);

                    if (expression != null)
                        expressions.Add(expression);
                }

            if (!expressions.Any())
                return null;

            return Expression.Lambda<Func<TEntity, bool>>(expressions.Aggregate(Expression.AndAlso), paramterExpression);
        }

        private static bool IsProp(Type type)
        {
            return type.Assembly.GetName().Name.ToUpper().Contains("CORELIB")
                && !type.IsInterface
                && !type.Name.StartsWith("List")
                && !type.Name.StartsWith("Pagination")
                && !type.Name.StartsWith("Sort");
        }

        private static Expression CreateExpresion<TFilter>(PropertyInfo propertyInfo, TFilter item, ParameterExpression paramterExpression)
        {
            var value = propertyInfo.GetValue(item);

            // Valores nulos
            if (value == null)
                return null;

            var valor = value.ToString().ToUpper() == "NULL" ? null : value;

            //String
            if (propertyInfo.PropertyType == typeof(string))
            {
                var propertyExp = Expression.Property(paramterExpression, propertyInfo.Name);

                var methodType = "Equals";
                if (valor.ToString().StartsWith("%") && valor.ToString().EndsWith("%"))
                    methodType = "Contains";
                else if (valor.ToString().StartsWith("%"))
                    methodType = "EndsWith";
                else if (valor.ToString().EndsWith("%"))
                    methodType = "StartsWith";

                var method = typeof(string).GetMethod(methodType, new[] { typeof(string) });
                var someValue = Expression.Constant(valor.ToString().Trim('%'), typeof(string));
                var containsMethodExp = Expression.Call(propertyExp, method, someValue);
                return containsMethodExp;
            }

            // DateTime
            else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {

                if (propertyInfo.Name.EndsWith("_Starts"))
                {
                    var ce = Expression.Constant(value);
                    var propriedade = propertyInfo.Name.Replace("_Starts", "");
                    var me = Expression.Property(paramterExpression, propriedade);
                    return Expression.GreaterThanOrEqual(me, ce);
                }
                else if (propertyInfo.Name.EndsWith("_Ends"))
                {
                    var ce = Expression.Constant(value);
                    var propriedade = propertyInfo.Name.Replace("_Ends", "");
                    var me = Expression.Property(paramterExpression, propriedade);
                    return Expression.LessThanOrEqual(me, ce);
                }
                else if (propertyInfo.Name.EndsWith("_Exact"))
                {
                    var ce = Expression.Constant(value);
                    var propriedade = propertyInfo.Name.Replace("_Exact", "");
                    var me = Expression.Property(paramterExpression, propriedade);
                    return Expression.LessThan(me, ce);
                }
                else
                {
                    var data = Convert.ToDateTime(value);
                    var dataInicio = data.Date;
                    var dataFim = data.Date.AddDays(1).AddSeconds(-1);
                    var me = Expression.Property(paramterExpression, propertyInfo.Name);

                    var ceGreater = Expression.Constant(dataInicio);
                    var expGreater = Expression.GreaterThanOrEqual(me, ceGreater);
                    var ceLess = Expression.Constant(dataFim);
                    var expLess = Expression.LessThanOrEqual(me, ceLess);

                    return Expression.AndAlso(expGreater, expLess);
                }
            }

            // Outros tipos
            else
            {
                var me = Expression.Property(paramterExpression, propertyInfo.Name);
                var ce = Expression.Constant(valor);
                return Expression.Equal(me, ce);
            }
        }

        #endregion
    }
}