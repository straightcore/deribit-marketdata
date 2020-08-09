using System;
using AutoMapper;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Models;

namespace MarketData.Adapter.Deribit.Converter.AutoMapper
{
    public class TradeProfile : Profile 
    {
        public TradeProfile()
        {
            CreateMap<TradeDto, Trade>()
                .ForMember(dest => dest.Date, opt => opt.ConvertUsing(new EpochTimestampConverter(), source => source.Timestamp))
                .ForMember(dest => dest.Id, opt => opt.MapFrom<TradeIdValueResolver>())
                ;
        }
    }
}