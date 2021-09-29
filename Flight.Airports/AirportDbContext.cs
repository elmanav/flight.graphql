using Flight.Airports.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.Airports
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> options) : base(options)
        {
        }

        public DbSet<Airport> Airports { get; private set; }
    }
}