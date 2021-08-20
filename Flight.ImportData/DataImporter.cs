using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Flight.ImportData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flight.ImportData
{
    public class DataImporter
    {
        private readonly FlightDbContext _context;

        public DataImporter(FlightDbContext context)
        {
            _context = context;
        }

        public async Task ImportFlights()
        {
            using var reader = await ImportAsync(new Uri("https://data-live.flightradar24.com/zones/fcgi/feed.json"));
            var data = await JObject.LoadAsync(reader);
            foreach (var item in data)
            {
                if (item.Value is JArray array)
                {
                    var craft = new Aircraft() { RegNumber = array[9].ToString(), ICAOCode = array[8].ToString() };
                    _context.Add(craft);
                }
                
            }

            await _context.SaveChangesAsync();

        }
        
        private async Task<JsonTextReader> ImportAsync(Uri source)
        {
            var httpClient = new HttpClient();
            using var httpResponse = await httpClient.GetAsync(source);

            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return new JsonTextReader(new StringReader(content));
            }

            throw new InvalidDataException();
        }
    }
}