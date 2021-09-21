using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddFlightClient()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5100/graphql"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            IFlightClient client = services.GetRequiredService<IFlightClient>();
            var res = await client.GetFlights.ExecuteAsync();
            foreach (var flight in res.Data.Flights)
            {
                Console.WriteLine($"{flight.Id} : {flight.Number} from {flight.From} on {flight.Aircraft} with {flight.Airline}.");
            }
        }
    }
}