using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using JetBrains.Annotations;

namespace Flight.AircraftHangar.Filters
{
	public class QueryableStringInvariantEqualsHandler : QueryableStringOperationHandler
	{
		private static readonly MethodInfo _toLower = typeof(string)
			.GetMethods()
			.Single(
				x => x.Name == nameof(string.ToLower) &&
				     x.GetParameters().Length == 0);

		protected override int Operation => DefaultFilterOperations.Contains;

		public override Expression HandleOperation(
			QueryableFilterContext context,
			IFilterOperationField field,
			IValueNode value,
			object parsedValue)
		{
			Expression property = context.GetInstance();
			if (parsedValue is string str)
			{
				MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				return Expression.Call(Expression.Call(property, _toLower), method, Expression.Constant(str.ToLower()));

				
				
				// return Expression.Equal(
				// 	Expression.Call(property, _toLower),
				// 	Expression.Constant(str.ToLower()));
			}

			throw new InvalidOperationException();
		}

		static Expression<Func<T, bool>> GetExpression<T>(string propertyName, string propertyValue)
		{
			var parameterExp = Expression.Parameter(typeof(T), "type");
			var propertyExp = Expression.Property(parameterExp, propertyName);
			MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
			var someValue = Expression.Constant(propertyValue, typeof(string));
			var containsMethodExp = Expression.Call(propertyExp, method, someValue);

			return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
		}


	}
}