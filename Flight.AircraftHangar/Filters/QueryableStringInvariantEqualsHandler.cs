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
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public QueryableStringInvariantEqualsHandler([NotNull] InputParser inputParser) : base(inputParser)
        {
        }
    }
}