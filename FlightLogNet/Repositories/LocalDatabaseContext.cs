namespace FlightLogNet.Repositories
{
    using System.Reflection;
    using Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class LocalDatabaseContext(IConfiguration configuration) : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Airplane> Airplanes { get; set; }

        public DbSet<ClubAirplane> ClubAirplanes { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<FlightStart> FlightStarts { get; set; }

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string sqliteConnectionString = configuration.GetValue<string>("SqliteConnectionString");
            string npgsqlConnectionString = configuration.GetValue<string>("NpgsqlConnectionString");

            if (sqliteConnectionString is not null)
            {
                optionsBuilder.UseSqlite(sqliteConnectionString);
            }
            else if (npgsqlConnectionString is not null)
            {
                optionsBuilder.UseNpgsql(npgsqlConnectionString);
            }
            else
            {
                string _database = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "data\\local.db");
                optionsBuilder.UseSqlite($"Data Source={_database}");
            }
        }
    }
}
