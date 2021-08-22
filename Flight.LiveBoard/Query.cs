using System.Linq;
using Flight.LiveBoard.Models;
using HotChocolate;
using HotChocolate.Data;

namespace Flight.LiveBoard
{
    public class Query
    {
        #region Public

        [UseFiltering()]
        public IQueryable<LiveFlight> GetFlights([Service] FlightDbContext context)
        {
            return context.Flights;
        }

        public LiveFlight GetFlight(string number, [Service] FlightDbContext context)
        {
            return context.Flights.Single(flight => flight.Number == number);
        }

        #endregion
    }
}