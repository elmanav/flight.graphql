using System.Linq;
using Flight.Airports.Models;
using HotChocolate;

namespace Flight.Airports
{
    public class Query
    {
        #region Public

        public IQueryable<Airport> GetAirports([Service] AirportDbContext context)
        {
            return context.Airports;
        }

        public Airport? GetAirport(string iata, [Service] AirportDbContext context)
        {
            return context.Airports.SingleOrDefault(airport => airport.Iata == iata);
        }
        #endregion
    }
}