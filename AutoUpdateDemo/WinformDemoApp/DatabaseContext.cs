using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace WinformDemoApp
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AppVersion> AppVersions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }

    public class AppVersion
    {
        public int Id { get; set; }
        public string Version { get; set; } = string.Empty;
    }
} 