using Flight.AircraftHangar.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.AircraftHangar
{
    public class AircraftDbContext : DbContext
    {
        public AircraftDbContext(DbContextOptions<AircraftDbContext> options) : base(options)
        {
            
        }

        public DbSet<Aircraft> Aircrafts
        {
            get;
            private set;
        }
    }
}