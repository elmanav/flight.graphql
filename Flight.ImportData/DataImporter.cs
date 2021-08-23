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
                if (item.Value is JArray array)
                {
                    var craft = new Aircraft { RegNumber = array[9].ToString(), ICAOCode = array[8].ToString() };
                    var flight = new LiveFlight(array[13].ToString(), array[11].ToString(), array[12].ToString(),
                        array[18].ToString(), array[9].ToString());
                    _context.Aircrafts.Add(craft);
                    _context.Flights.Add(flight);
                }
        }

        public async Task AddCactus1549HudsonRiverAsync()
        {
            await _context.Flights.AddAsync(new LiveFlight("UA1549", "LGA", "CLT", "UAL", "N106US"));
            await _context.Aircrafts.AddAsync(new Aircraft { RegNumber = "N106US", ICAOCode = "A320" });
        }

        public async Task ImportAirports()
        {
            using var reader = await ImportAsync(new Uri("https://www.flightradar24.com/_json/airports.php"));
            var data = await JObject.LoadAsync(reader);
            var airportsData = data["rows"] as JArray;
            foreach (var item in airportsData)
            {
                var airport = item.ToObject<Airport>();
                _context.Airports.Add(airport);
            }
        }

        public async Task ImportAirlines()
        {
            using var reader = await ImportAsync(new Uri("https://www.flightradar24.com/_json/airlines.php"));
            var data = await JObject.LoadAsync(reader);
            var airlinesData = data["rows"] as JArray;
            foreach (var item in airlinesData)
            {
                var airline = item.ToObject<Airline>();
                _context.Airlines.Add(airline);
            }
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