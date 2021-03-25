using Microsoft.EntityFrameworkCore;
using Stonks.API.Models;

namespace Stonks.API.Data
{
    public class StonksContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<TimeSeries> TimeSeries {get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=4002;Database=Stonks;Username=yorrick;Password=123;timeout=1000;");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeries>()
                .HasKey(ts => new { ts.TimeStamp, ts.Symbol });
        }
    }
}