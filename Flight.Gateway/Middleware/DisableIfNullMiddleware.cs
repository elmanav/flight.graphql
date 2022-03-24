using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flight.Gateway.Stitching;
using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace Flight.Gateway.Middleware
{
    public class DisableIfNullMiddleware
    {
        private readonly FieldDelegate _next;

        public DisableIfNullMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            var parent = context.Parent<IReadOnlyDictionary<string, object?>?>();

            var shouldDisable = context.Selection.Field
                .Directives[DirectiveNames.DisableIfNull]
                .Select(x => x.ToObject<DisableIfNullDirective>())
                .Any(directive => parent is null ||
                                  !(directive is {Field: { } field} &&
                                    parent.TryGetValue(field, out var value) &&
                                    value is { } and not NullValueNode));

            if (!shouldDisable)
            {
                await _next(context);
            }
        }
    }

}