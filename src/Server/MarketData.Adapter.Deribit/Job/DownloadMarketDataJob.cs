using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Json.rpc;
using MarketData.Adapter.Deribit.Repositories;
using Newtonsoft.Json.Linq;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Quartz;

namespace MarketData.Adapter.Deribit.Job
{
    public class DownloadMarketDataJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var instruments = context.JobDetail.JobDataMap.Get("instruments") as IEnumerable<InstrumentResponseDto>;
            var taskScheduler = context.JobDetail.JobDataMap.Get("taskScheduler") as TaskScheduler;
            var apiInstance = context.JobDetail.JobDataMap.Get("marketDataApi") as MarketDataApi;
            var cancellationToken = context.JobDetail.JobDataMap.Get("cancellationToken") is CancellationToken casted ? casted : CancellationToken.None;
            var repository = context.JobDetail.JobDataMap.Get("tradeRepository") as ITradeRepository<TradeDto>;
            // var apiInstance = new MarketDataApi(Org.OpenAPITools.Client.Configuration.Default);
            return Task.WhenAll(ExecuteDownload(instruments, apiInstance, taskScheduler, cancellationToken))
                        .ContinueWith(new TradePusher(repository, cancellationToken, taskScheduler).Push,
                                      cancellationToken,
                                      TaskContinuationOptions.AttachedToParent,
                                      taskScheduler)
                        ;
        }

        private IEnumerable<Task<TradeDto>> ExecuteDownload(IEnumerable<InstrumentResponseDto> instruments,
                                                  MarketDataApi apiInstance,
                                                  TaskScheduler taskScheduler,
                                                  CancellationToken cancellationToken)
        {
            return instruments.Select(instru => GetLastTradesByInstruments(apiInstance, taskScheduler, instru, cancellationToken));
        }

        private static async Task<TradeDto> GetLastTradesByInstruments(MarketDataApi apiInstance, TaskScheduler taskScheduler, InstrumentResponseDto instru, CancellationToken cancellationToken)
        {
            // return apiInstance.PublicGetLastTradesByInstrumentGetAsync(instru.Name, null, null, 1, false, null)
            //                                         .ContinueWith(GetLastTrade, cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, taskScheduler)
            //                                         .ContinueWith(ReturnNull, cancellationToken, TaskContinuationOptions.OnlyOnFaulted, taskScheduler);
            try
            {
                var dataApi = await apiInstance.PublicGetLastTradesByInstrumentGetAsync(instru.Name, null, null, 1, false, null);
                return GetLastTrade(dataApi as JObject);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private static TradeDto ReturnNull(Task<TradeDto> task)
        {
            return (TradeDto)null;
        }

        private static TradeDto GetLastTrade(Task<object> task)
        {
            return (task.Result as JObject).ToObject<JsonRpcEnvelopeDto<TradeResponseDto>>()?.result?.Trades?.Last();
        }
        private static TradeDto GetLastTrade(JObject jObj)
        {
            return (jObj).ToObject<JsonRpcEnvelopeDto<TradeResponseDto>>()?.result?.Trades?.Last();
        }

        private class TradePusher
        {
            private readonly ITradeRepository<TradeDto> repository;
            private readonly CancellationToken cancellationToken;
            private readonly TaskScheduler taskScheduler;
            public TradePusher(ITradeRepository<TradeDto> repository, CancellationToken cancellationToken, TaskScheduler taskScheduler)
            {
                this.taskScheduler = taskScheduler;
                this.cancellationToken = cancellationToken;
                this.repository = repository;

            }

            public Task Push(Task<TradeDto[]> trades)
            {
                var result = trades.Result?.Where(trade => trade != null);
                return repository.Insert(result)
                                 .ContinueWith(task => repository.Save(), cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, taskScheduler);
            }
        }
    }

}