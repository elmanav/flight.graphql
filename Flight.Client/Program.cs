using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StrawberryShake.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Flight.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSerializer<AnyValueJsonSerializer>()
                .AddFlightClient()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5051/graphql"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            IFlightClient client = services.GetRequiredService<IFlightClient>();
            var value = new MoneyValue(new decimal(12.345), "LAT");
            // var dataJson = JsonDocument.Parse(JsonSerializer.Serialize(value));
            // var element = dataJson.RootElement.GetRawText();
            
            var res = await client.GetAny.ExecuteAsync(new ValueInput(){TypeName = "Type", Value = value});
             var craft = res.Data!.AircraftByAny;
             Console.WriteLine($"{craft.Id} : {craft.Id}.");
            
        }
    }

   record MoneyValue(decimal Value, string Currency) : Value;

   public record Value
   {
       
   }

   public class AnyValueJsonSerializer : ILeafValueParser<JsonElement, object>
       , IInputValueFormatter
   {
       public AnyValueJsonSerializer(string typeName = BuiltInScalarNames.Any)
       {
           TypeName = typeName;
       }


       /// <inheritdoc />
       public object Parse(JsonElement serializedValue)
       {
           throw new NotImplementedException();
       }

       /// <inheritdoc />
       public string TypeName { get; }

       /// <inheritdoc />
       public object? Format(object? runtimeValue)
       {
           if (runtimeValue is null)
           {
               return null;
           }

           var rootElement = JsonDocument.Parse(JsonSerializer.Serialize<object>(runtimeValue)).RootElement;
           var s = rootElement.GetRawText();
           return rootElement;
       }
   }
}