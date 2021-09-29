using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Flight.ImportData
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new DbContextOptionsBuilder<FlightDbContext>();
            builder.UseSqlite(@"Data Source=flights.db");
            await using var context = new FlightDbContext(builder.Options);
            if (await context.Database.EnsureCreatedAsync())
            {
                var importer = new DataImporter(context);
                await importer.ImportFlights();
                await importer.ImportAirports();
                await importer.ImportAirlines();
                await importer.AddCactus1549HudsonRiverAsync();
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Import complete.");
        }
    }
}