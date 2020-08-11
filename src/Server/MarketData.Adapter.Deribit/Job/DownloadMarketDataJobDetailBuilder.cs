using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Repositories;
using Org.OpenAPITools.Api;
using Quartz;

namespace MarketData.Adapter.Deribit.Job
{
    public class DownloadMarketDataJobDetailBuilder
    {
        private Dictionary<string, object> jobDataMap = new Dictionary<string, object>()
        {
            { "cancellationToken",  CancellationToken.None },
            { "taskScheduler",  TaskScheduler.Current },
        };

        private JobKey jobKey = new JobKey("downloadInstrumentsJobDetail", "marketData");
        private readonly ITradeRepository<TradeDto> repository;

        public DownloadMarketDataJobDetailBuilder SetRepository(ITradeRepository<TradeDto> repository)
        {
            jobDataMap.Add("tradeRepository", repository);
            return this;
        }

        public DownloadMarketDataJobDetailBuilder SetJobIdentity(string name, string group = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }
            this.jobKey.Name = name;
            this.jobKey.Group = group;
            return this;
        }

        public DownloadMarketDataJobDetailBuilder SetInstruments(IEnumerable<InstrumentResponseDto> instruments)
        {
            var list = instruments?.ToList() ?? Enumerable.Empty<InstrumentResponseDto>();
            if (!list.Any() && jobDataMap.ContainsKey("instruments"))
            {
                jobDataMap.Remove("instruments");
                return this;
            }
            jobDataMap["instruments"] = instruments;
            return this;
        }

        public DownloadMarketDataJobDetailBuilder SetCancellationToken(CancellationToken cancellationToken)
        {
            jobDataMap["cancellationToken"] = cancellationToken;
            return this;
        }

        public DownloadMarketDataJobDetailBuilder SetTaskScheduler(TaskScheduler taskScheduler)
        {
            jobDataMap["taskScheduler"] = taskScheduler;
            return this;
        }

        public DownloadMarketDataJobDetailBuilder SetMarketDataApi(MarketDataApi marketDataApi)
        {
            jobDataMap["marketDataApi"] = marketDataApi;
            return this;
        }

        public IJobDetail Build()
        {

            var builder = JobBuilder.Create<DownloadMarketDataJob>();
            builder.SetJobData(new JobDataMap((IDictionary<string, object>)new Dictionary<string, object>(jobDataMap)));
            builder.WithIdentity(new JobKey(this.jobKey.Name, this.jobKey.Group));
            return builder.Build();
        }
    }
}