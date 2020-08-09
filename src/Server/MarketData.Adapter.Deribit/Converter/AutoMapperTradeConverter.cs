using AutoMapper;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Models;

namespace MarketData.Adapter.Deribit.Converter
{
    public class AutoMapperTradeConverter : IConverter<Trade, TradeDto>
    {
        private readonly IMapper mapper;

        public AutoMapperTradeConverter(IMapper mapper)
        {
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            
        }

        public TDestination Convert<TDestination>(object source)
        {
            return this.mapper.Map<TDestination>(source);
        }

        public Trade Convert(TradeDto source)
        {
            return this.mapper.Map<Trade>(source);
        }
    }
}