using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Seventh.DGuard.Database
{
    public class SeventhDGuardContext : DbContext
    {
        public SeventhDGuardContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public SeventhDGuardContext(DbContextOptions<SeventhDGuardContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Adicionar indice para coluna EnderecoIp para agilizar consultas de servidores
            modelBuilder.Entity<Server>().HasIndex(e => e.Ip);

            modelBuilder.Entity<Video>().Property(b => b.AddDate).HasDefaultValueSql("getdate()");
        }

        public DbSet<Server> Server { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<RecyclerStatus> RecyclerStatus { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SeventhDGuardContext>
    {
        public SeventhDGuardContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Seventh.DGuard/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<SeventhDGuardContext>();
            var connectionString = configuration.GetConnectionString("cs_seventh_dguard");
            builder.UseSqlServer(connectionString);
            return new SeventhDGuardContext(builder.Options);
        }
    }
}
