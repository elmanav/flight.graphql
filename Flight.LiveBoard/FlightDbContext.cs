using Flight.LiveBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.LiveBoard
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }

        public DbSet<LiveFlight> Flights { get; private set; }
    }
}