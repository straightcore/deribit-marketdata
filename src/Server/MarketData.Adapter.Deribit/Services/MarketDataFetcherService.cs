using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Job;
using MarketData.Adapter.Deribit.Repositories;
using Org.OpenAPITools.Api;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace MarketData.Adapter.Deribit.Services
{
    public interface IMarketDataFetcherService : IService
    {

    }

    public class MarketDataFetcherService : IDisposable, IService, IMarketDataFetcherService
    {
        private readonly IEnumerable<InstrumentConfig> instruments;
        private readonly DownloadMarketDataJobDetailBuilder jobDetailBuilder;
        private readonly StdSchedulerFactory schedulerFactory;
        private readonly ITradeRepository<TradeDto> repository;

        // private readonly TaskScheduler taskScheduler;
        private readonly ServiceConfig serviceConfiguration;
        private readonly IInstrumentQuery requester;
        private bool isDisposed;
        private IScheduler scheduler;

        public MarketDataFetcherService(ServiceConfig serviceConfiguration,
                                        IInstrumentQuery requester,
                                        IEnumerable<InstrumentConfig> instruments,
                                        DownloadMarketDataJobDetailBuilder jobDetailBuilder,
                                        StdSchedulerFactory schedulerFactory,
                                        ITradeRepository<TradeDto> repository)
        {
            this.serviceConfiguration = serviceConfiguration ?? throw new ArgumentNullException(nameof(serviceConfiguration));
            this.requester = requester ?? throw new System.ArgumentNullException(nameof(requester));
            this.instruments = instruments ?? throw new System.ArgumentNullException(nameof(instruments));
            this.jobDetailBuilder = jobDetailBuilder ?? throw new ArgumentNullException(nameof(jobDetailBuilder));
            this.schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            // this.taskScheduler = taskScheduler ?? throw new ArgumentNullException(nameof(taskScheduler));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            CheckIfDIsposed();
            var instrumentsPerRequestTask = await Task.WhenAll(this.instruments.Select(async instr => await this.requester.GetInstrumentsAsync(instr, cancellationToken)).ToList());
            cancellationToken.ThrowIfCancellationRequested();
            this.scheduler = await schedulerFactory.GetScheduler(cancellationToken);
            await this.scheduler.Start();
            cancellationToken.ThrowIfCancellationRequested();
            var trigger = TriggerBuilder.Create()
                    .WithIdentity("downloadInstrumentTrigger", "marketData")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(this.serviceConfiguration.FetchInterval)
                        .RepeatForever())
                    .Build();
            var jobDetail = this.jobDetailBuilder
                                .SetInstruments((instrumentsPerRequestTask).SelectMany(item => item))
                                .SetMarketDataApi(new MarketDataApi(Org.OpenAPITools.Client.Configuration.Default))
                                .SetCancellationToken(cancellationToken)
                                .SetRepository(this.repository)
                                .Build();
            cancellationToken.ThrowIfCancellationRequested();
            await this.scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellation)
        {
            CheckIfDIsposed();
            cancellation.ThrowIfCancellationRequested();
            return Task.WhenAll(new Task[]{ Task.WhenAll(this.scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellation)
                                                .ContinueWith(task => this.scheduler.DeleteJobs(task.Result), TaskContinuationOptions.OnlyOnRanToCompletion))
                                                .ContinueWith(task => this.scheduler.Shutdown(cancellation), TaskContinuationOptions.ExecuteSynchronously),
                                            this.repository.Save()
                                            });
        }

        private void CheckIfDIsposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (this.scheduler.IsStarted)
                    {
                        this.scheduler.Shutdown()
                                    .ConfigureAwait(false)
                                    .GetAwaiter()
                                    .GetResult();
                    }
                }
                this.scheduler = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~InstrumentFetcherService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

    }
}