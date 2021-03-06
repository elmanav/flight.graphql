using ConferencePlanner.GraphQL.Data;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.GraphQL.Imports
{
    public static class ImportRequestExecutorBuilderExtensions
    {
        public static IRequestExecutorBuilder EnsureDatabaseIsCreated(
            this IRequestExecutorBuilder builder)
        {
            return builder.ConfigureSchemaAsync(async (services, builder, ct) =>
            {
                IDbContextFactory<ApplicationDbContext> factory =
                    services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                using ApplicationDbContext dbContext = factory.CreateDbContext();

                if (await dbContext.Database.EnsureCreatedAsync(ct))
                {
                    var importer = new DataImporter();
                    await importer.LoadDataAsync(dbContext);
                }
            });
        }
    }
}