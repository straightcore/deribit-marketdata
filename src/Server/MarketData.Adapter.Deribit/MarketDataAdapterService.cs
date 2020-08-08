using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using MarketData.Adapter.Deribit.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit
{
    public class MarketDataAdapterService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly IInstrumentFetcherService instrumentFetcherService;
        private readonly IValidationConfigurationService validationService;
        private bool isDisposed = false;
        public bool IsStart { get; set; }

        public MarketDataAdapterService(ILogger<MarketDataAdapterService> logger,
                                        IInstrumentFetcherService instrumentFetcherService,
                                        IValidationConfigurationService validationService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.instrumentFetcherService = instrumentFetcherService ?? throw new ArgumentNullException(nameof(instrumentFetcherService));
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsStart)
            {
                this.logger.LogWarning("Service is already start");
                return;
            }
            await validationService.StartAsync(cancellationToken);
            await instrumentFetcherService.StartAsync(cancellationToken);
            this.logger.LogInformation("MarketData Service is started");
            IsStart = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (!IsStart)
            {
                this.logger.LogWarning("Service is already stop");
                return Task.CompletedTask;
            }
            this.logger.LogInformation("MarketData Service stop");
            IsStart = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposed)
            {
                return;
            }
            if (isDisposing)
            {

            }
            this.isDisposed = true;
        }
    }
}