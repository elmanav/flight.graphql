using Flight.ImportData.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.ImportData
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }

        public DbSet<Aircraft> Aircrafts { get; private set; }

        public DbSet<Airport> Airports { get; private set; }
        public DbSet<Airline> Airlines { get; private set; }
        public DbSet<LiveFlight> Flights { get; private set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Airport>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Airport>().HasKey("Id");

            modelBuilder.Entity<Airline>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Airline>().HasKey("Id");

            modelBuilder.Entity<Aircraft>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Aircraft>().HasKey("Id");

            modelBuilder.Entity<LiveFlight>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<LiveFlight>().HasKey("Id");
        }
    }
}