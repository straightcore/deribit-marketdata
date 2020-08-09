using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Converter;
using MarketData.Adapter.Deribit.Models;

namespace MarketData.Adapter.Deribit.Repositories
{
    public class TradeDtoRepository : ITradeRepository<TradeDto>
    {
        private readonly ITradeRepository<Trade> decoratedTradeRepo;
        private readonly IConverter<Trade, TradeDto> converter;

        public TradeDtoRepository(ITradeRepository<Trade> decoratedTradeRepo, IConverter<Trade, TradeDto> converter)
        {
            this.decoratedTradeRepo = decoratedTradeRepo ?? throw new System.ArgumentNullException(nameof(decoratedTradeRepo));
            this.converter = converter ?? throw new System.ArgumentNullException(nameof(converter));
        }
        public Task Insert(TradeDto trade)
        {
            return this.decoratedTradeRepo.Insert(this.converter.Convert(trade));
        }

        public Task Insert(IEnumerable<TradeDto> trades)
        {
            return this.decoratedTradeRepo.Insert(trades.Select(this.converter.Convert));
        }

        public Task Save()
        {
            return this.decoratedTradeRepo.Save();
        }
    }
}