using System.Linq;
using Flight.Airlines.Models;
using HotChocolate;

namespace Flight.Airlines
{
    public class Query
    {
        #region Public

        public IQueryable<Airline> GetAirlines([Service] AirlinesDbContext context)
        {
            return context.Airlines;
        }

        public Airline? GetAirline(string icao, [Service] AirlinesDbContext context)
        {
            return context.Airlines.SingleOrDefault(airport => airport.Icao == icao);
        }

        #endregion
    }
}