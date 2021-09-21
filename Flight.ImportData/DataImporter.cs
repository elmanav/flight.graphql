using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Flight.ImportData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flight.ImportData
{
    public class DataImporter
    {
        private const string APIKey = "e7ed6e04b261d1418bb5c3f9957894b8";
        private readonly FlightDbContext _context;

        public DataImporter(FlightDbContext context)
        {
            _context = context;
        }

        private void StartLog([CallerMemberName] string method = null)
        {
            Console.WriteLine($"Import {method} started...");
        }

        private void CompleteLog([CallerMemberName] string method = null)
        {
            Console.WriteLine($"Import {method} completed.");
        }

        public async Task ImportCountries()
        {
            StartLog();
            using var reader =
                await ImportAsync(new Uri($"http://api.aviationstack.com/v1/countries?access_key={APIKey}&limit=1000"));
            var data = await JObject.LoadAsync(reader);
            var countries = data["data"] as JArray;
            foreach (var token in countries)
            {
                var country = new Country(token["country_iso2"].ToString(), token["country_name"].ToString(),
                    token["capital"].ToString(), token["continent"].ToString());
                _context.Countries.Add(country);
            }

            CompleteLog();
        }

        public async Task ImportCities()
        {
            StartLog();
            using var reader =
                await ImportAsync(new Uri($"http://api.aviationstack.com/v1/cities?access_key={APIKey}&limit=10000"));
            var data = await JObject.LoadAsync(reader);
            var countries = data["data"] as JArray;
            foreach (var token in countries)
            {
                var city = new City(token["iata_code"].ToString(), token["city_name"].ToString(),
                    token["country_iso2"].ToString(), token["timezone"].ToString());
                _context.Cities.Add(city);
            }

            CompleteLog();
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
            StartLog();
            using var reader =
                await ImportAsync(new Uri($"http://api.aviationstack.com/v1/airports?access_key={APIKey}&limit=10000"));
            var data = await JObject.LoadAsync(reader);
            var airports = data["data"] as JArray;
            foreach (var token in airports)
            {
                var airport = new Airport(token["iata_code"].Value<string>(), token["airport_name"].Value<string>(),
                    token["city_iata_code"]?.Value<string?>(), token["icao_code"].Value<string>(),
                    token["country_iso2"]?.Value<string?>());
                _context.Airports.Add(airport);
            }

            CompleteLog();
        }
        
        public async Task ImportAircraftModels()
        {
            StartLog();
            using var reader =
                await ImportAsync(new Uri($"http://api.aviationstack.com/v1/aircraft_types?access_key={APIKey}&limit=1000"));
            var data = await JObject.LoadAsync(reader);
            var models = data["data"] as JArray;
            foreach (var token in models)
            {
                var model = new AircraftModel(token["iata_code"].Value<string>(), token["aircraft_name"].Value<string>());
                _context.AircraftModels.Add(model);
            }

            CompleteLog();
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