using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.GraphQL
{
    public static class ObjectFieldDescriptorExtensions
    {
        public static IObjectFieldDescriptor UseDbContext<TDbContext>(
            this IObjectFieldDescriptor descriptor)
            where TDbContext : DbContext
        {
            return descriptor.UseScopedService(
                s => s.GetRequiredService<IDbContextFactory<TDbContext>>().CreateDbContext(),
                disposeAsync: (s, c) => c.DisposeAsync());
        }

        public static IObjectFieldDescriptor UseUpperCase(
            this IObjectFieldDescriptor descriptor)
        {
            // TODO : we need a better API for the user.
            descriptor.Extend().Definition.ResultConverters.Add(
                new ResultConverterDefinition((context, result) =>
                {
                    if (result is string s) return s.ToUpperInvariant();
                    return result;
                }));

            return descriptor;
        }
    }
}