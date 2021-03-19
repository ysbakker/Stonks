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
            optionsBuilder.UseNpgsql("jdbc:postgresql://localhost:4002/Stonks");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeries>()
                .HasKey(ts => new { ts.TimeStamp, ts.Symbol });
        }
    }
}