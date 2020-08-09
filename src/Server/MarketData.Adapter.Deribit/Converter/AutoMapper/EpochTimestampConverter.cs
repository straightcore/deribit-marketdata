using System;
using AutoMapper;

namespace MarketData.Adapter.Deribit.Converter.AutoMapper
{
    public class EpochTimestampConverter : IValueConverter<long, DateTime>
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DateTime Convert(long source, DateTime destination, ResolutionContext context)
        {
            return epoch.AddMilliseconds(source);
        }

        public DateTime Convert(long sourceMember, ResolutionContext context)
        {
            return epoch.AddMilliseconds(sourceMember);
        }
    }
}