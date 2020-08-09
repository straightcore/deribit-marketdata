using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketData.Adapter.Deribit.EntityFramework
{
    public class MarketDataDbContext : DbContext
    {
        public DbSet<Trade> Instruments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=marketData.db");
    }
}