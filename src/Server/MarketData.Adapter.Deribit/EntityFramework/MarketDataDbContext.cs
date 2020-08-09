using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Deribit.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Adapter.Deribit.EntityFramework
{
    public class MarketDataDbContext : DbContext
    {
        public DbSet<Trade> Trades { get; set; }
        
        // public MarketDataDbContext()
        // {
            
        // }

        public MarketDataDbContext(DbContextOptions<MarketDataDbContext> options)
            : base(options)
        {
            // if (string.IsNullOrEmpty(connectionString))
            // {
            //     throw new System.ArgumentException($"'{nameof(connectionString)}' cannot be null or empty", nameof(connectionString));
            // }

            // this.connectionString = connectionString;
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //     => options.UseSqlite("Data Source=marketData.db");
    }
}