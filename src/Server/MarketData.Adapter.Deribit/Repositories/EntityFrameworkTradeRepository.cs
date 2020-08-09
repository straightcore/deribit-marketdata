using System.Collections.Generic;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Converter;
using MarketData.Adapter.Deribit.EntityFramework;
using MarketData.Adapter.Deribit.Models;

namespace MarketData.Adapter.Deribit.Repositories
{
    public class EntityFrameworkTradeRepository : ITradeRepository<Trade>
    {
        private readonly MarketDataDbContext dbContext;
        public EntityFrameworkTradeRepository(MarketDataDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));

        }

        
        public Task Insert(Trade trade)
        {
            return this.dbContext.AddAsync(trade).AsTask();
        }

        public Task Insert(IEnumerable<Trade> trades)
        {
            return this.dbContext.AddRangeAsync(trades);
        }

        public Task Save()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}