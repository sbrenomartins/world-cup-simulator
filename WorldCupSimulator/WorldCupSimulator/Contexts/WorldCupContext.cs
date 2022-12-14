using Microsoft.EntityFrameworkCore;
using WorldCupSimulator.Models;

namespace WorldCupSimulator.Contexts
{
    public class WorldCupContext : DbContext
    {
        public WorldCupContext(DbContextOptions<WorldCupContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ServerConnection"));
        }

        public DbSet<Team> Teams { get; set; }
    }
}
