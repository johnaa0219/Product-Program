using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            else
            {
                return s;
            }
        }
        public static IQueryable<T> Col<T>(this IQueryable<T> source,
        string column)
        {
            //取得 T.column 屬性
            var property = typeof(T).GetProperty(column);

            //產生 it.a 的 it
            var itParameter = Expression.Parameter(typeof(T), "it");

            if (itParameter == null)
            {
                property = typeof(T).GetProperty("Noa");
                itParameter = Expression.Parameter(typeof(T), "it");
            }

            //產生 it.a
            Expression expressionProperty = Expression.Property(
                itParameter, property.Name);

            //產生 .OrderBy(x=>x.column);
            return source.OrderBy(Expression.Lambda<Func<T, string>>(Expression.Property(itParameter, column), itParameter));
        }
        public static IQueryable<T> WhereContains<T>(this IQueryable<T> source,
        string column, string value)
        {
            if (column != null)
            {
                //取得 T.column 屬性
                var property = typeof(T).GetProperty(column);

                //產生 it.a 的 it
                var itParameter = Expression.Parameter(typeof(T), "it");

                //產生 it.a
                Expression expressionProperty = Expression.Property(
                    itParameter, property.Name);

                //產生 string.Contains 函數
                var containsMethod = typeof(string)
                    .GetMethod("Contains", new[] { typeof(string) });

                //產生 it.a.Contains(value)
                var selector = Expression.Call(
                    expressionProperty,
                    containsMethod,
                    Expression.Constant(value));

                //產生 .Where(it => it.a.Contains(value))
                return source.Where(Expression.Lambda<Func<T, bool>>(selector, itParameter));
            }
            return source;
        }
        public static IQueryable<T> Between<T>(this IQueryable<T> source,
       string column, string value1,string value2)
        {
            if (column != null)
            {
                //取得 T.column 屬性
                var property = typeof(T).GetProperty(column);


                //產生 it.a 的 it
                var itParameter = Expression.Parameter(typeof(T), "it");

                //產生 it.a
                Expression fromExpression = Expression.Property(itParameter, property.Name);
                Expression toExpression = Expression.Property(itParameter, property.Name);
                var before = Expression.LessThanOrEqual(toExpression,
                    Expression.Constant(value2, property.GetType()));

                var after = Expression.GreaterThanOrEqual(fromExpression,
                    Expression.Constant(value1, property.GetType()));

                Expression body = Expression.And(after, before);

                return source.Where(Expression.Lambda<Func<T, bool>>(body,itParameter));
            }
            return source;
        }
        public static IQueryable<T> OrderByCol<T>(this IQueryable<T> source,
        string column)
        {
            //取得 T.column 屬性
            var property = typeof(T).GetProperty(column);

            //產生 it.a 的 it
            var itParameter = Expression.Parameter(typeof(T), "it");

            if (itParameter == null)
            {
                property = typeof(T).GetProperty("Noa");
                itParameter = Expression.Parameter(typeof(T), "it");
            }

            //產生 it.a
            Expression expressionProperty = Expression.Property(
                itParameter, property.Name);
                        
            //產生 .OrderBy(x=>x.column);
            return source.OrderBy(Expression.Lambda<Func<T, string>>(Expression.Property(itParameter, column), itParameter));
        }

    }

}
