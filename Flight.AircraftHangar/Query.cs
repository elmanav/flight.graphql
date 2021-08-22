using System.Linq;
using Flight.AircraftHangar.Models;
using HotChocolate;
using HotChocolate.Data;

namespace Flight.AircraftHangar
{
    public class Query
    {
        #region Public

        public IQueryable<Aircraft> GetAircrafts([Service] AircraftDbContext context)
        {
            return context.Aircrafts;
        }

        public Aircraft? GetAircraft(string regNumber, [Service] AircraftDbContext context)
        {
            return context.Aircrafts.FirstOrDefault(aircraft => aircraft.RegNumber == regNumber);
        }

        #endregion
    }
}