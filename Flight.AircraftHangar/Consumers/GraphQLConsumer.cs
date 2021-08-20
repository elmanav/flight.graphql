using System.Threading.Tasks;
using Flight.Contracts;
using HotChocolate;
using HotChocolate.Execution;
using JetBrains.Annotations;
using MassTransit;

namespace Flight.AircraftHangar.Consumers
{
    [UsedImplicitly]
    public class GraphQLConsumer : IConsumer<IGraphQLRequest>
    {
        private readonly IRequestExecutor _executor;

        public GraphQLConsumer(IRequestExecutor executor)
        {
            _executor = executor;
        }
        
        /// <inheritdoc />
        public async Task Consume(ConsumeContext<IGraphQLRequest> context)
        {
            var result = await _executor.ExecuteAsync(context.Message.Query);
            await context.RespondAsync<IGraphQLResponse>(new { QueryResult = await result.ToJsonAsync() });
        }
    }
}