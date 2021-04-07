using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stonks.API.Models;

namespace Stonks.API.Data
{
    public class StonksContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<TimeSeries> TimeSeries {get; set; }
        public DbSet<Quote> Quotes {get; set; }
        private readonly IConfiguration _configuration;

        public StonksContext(DbContextOptions<StonksContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeries>()
                .HasKey(ts => new { ts.TimeStamp, ts.Symbol });
        }
    }
}