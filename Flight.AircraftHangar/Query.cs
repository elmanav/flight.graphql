using System.Linq;
using Flight.AircraftHangar.Models;
using HotChocolate.Data;

namespace Flight.AircraftHangar
{
	public class Query
	{
		private readonly AircraftDbContext _context;

		public Query(AircraftDbContext context)
		{
			_context = context;
		}

		#region Public

		[UseFiltering()]
		public IQueryable<Aircraft> GetAircrafts()
		{
			return _context.Aircrafts;
		}

		#endregion
	}
}