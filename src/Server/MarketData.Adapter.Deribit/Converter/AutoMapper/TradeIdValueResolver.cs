using AutoMapper;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Models;

namespace MarketData.Adapter.Deribit.Converter.AutoMapper
{
    public class TradeIdValueResolver : IValueResolver<TradeDto, Trade, string>
    {
        public static readonly EpochTimestampConverter epochConverter = new EpochTimestampConverter();
        
        public string Resolve(TradeDto source, Trade destination, string destMember, ResolutionContext context)
        {
            return $"{source.InstrumentName}@{epochConverter.Convert(source.Timestamp, new System.DateTime(), null).ToString("O")}";
        }
    }
}