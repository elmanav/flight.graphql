using Flight.Airlines.Models;
using Microsoft.EntityFrameworkCore;

namespace Flight.Airlines
{
    public class AirlinesDbContext : DbContext
    {
        public AirlinesDbContext(DbContextOptions<AirlinesDbContext> options) : base(options)
        {
        }

        public DbSet<Airline> Airlines { get; private set; }
    }
}