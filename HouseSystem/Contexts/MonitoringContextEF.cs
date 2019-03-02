using Microsoft.EntityFrameworkCore;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace HouseSystem.Contexts
{
    public class MonitoringContextEF : DbContext
    {
        public MonitoringContextEF(DbContextOptions<MonitoringContextEF> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Building>()
                .HasIndex(u => u.Address)
                .IsUnique();
        }
       
        public DbSet<Meter> Meters { get; set; }
        public DbSet<WaterMeter> WaterMeters { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<House> Houses { get; set; }
    }
}