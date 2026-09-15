using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models;
using System.Linq.Expressions;

namespace Calenderapp.MVC
{

    public class SearchEngine() : ISearchEngine
    {

        public IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder) where T : class
        {
            if (string.IsNullOrEmpty(sortOrder))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortOrder.Replace("_desc", ""));
            //var arrow = Expression.Property(parameter, sortOrder.Replace("_desc", ""));
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            return sortOrder.EndsWith("_desc") ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
        }

        public IQueryable<T> ApplySearch<T>(IQueryable<T> query, string searchString, string searchCategory) where T : class
        {
            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchCategory))
                return query;

            searchString = searchString.ToLower();

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, searchCategory);
            var propertyToString = Expression.Call(property, "ToString", null);
            var propertyToLower = Expression.Call(propertyToString, "ToLower", null);
            var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
            var searchStringExpression = Expression.Constant(searchString);
            var containsExpression = Expression.Call(propertyToLower, containsMethod!, searchStringExpression);
            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

            return query.Where(lambda);
        }
    }
}
