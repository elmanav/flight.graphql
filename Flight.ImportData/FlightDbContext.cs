using Flight.ImportData.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.ImportData
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
            
        }

        public DbSet<Aircraft> Aircrafts
        {
            get;
            private set;
        }
    }
}