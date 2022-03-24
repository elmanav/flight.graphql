using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flight.AircraftHangar.Models;
using GreenDonut;
using HotChocolate;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Flight.AircraftHangar
{
    public class Query
    {
        #region Public

        public IQueryable<Aircraft> GetAircrafts([Service] AircraftDbContext context)
        {
            return context.Aircrafts;
        }

        public async Task<Aircraft?> GetAircraft(string regNumber, AircraftBatchDataLoader dataLoader)
        {
            return await dataLoader.LoadAsync(regNumber);
        }

        
        
        #endregion
    }

    public class AircraftBatchDataLoader : BatchDataLoader<string, Aircraft>
    {
        private readonly AircraftDbContext _context;

        /// <inheritdoc />
        public AircraftBatchDataLoader(AircraftDbContext context, [NotNull] IBatchScheduler batchScheduler, [CanBeNull] DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            _context = context;
        }

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<string, Aircraft>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var crafts = await _context.Aircrafts.Where(aircraft => keys.Contains(aircraft.RegNumber)).ToArrayAsync(cancellationToken: cancellationToken);
            return crafts.ToDictionary(aircraft => aircraft.RegNumber);
        }
    }
}