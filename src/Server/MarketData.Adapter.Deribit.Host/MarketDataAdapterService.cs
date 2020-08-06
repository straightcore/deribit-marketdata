using System;
using System.Threading;
using System.Threading.Tasks;
using MarketDataAdapterService.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MarketData.Adapter.Deribit.Host
{
    public class MarketDataAdapterService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly ServiceConfig configuration;
        private bool isDisposed = false;

        public MarketDataAdapterService(ILogger<MarketDataAdapterService> logger, IOptions<ServiceConfig> options)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {   this.logger.LogInformation($"Starting service on {this.configuration.Uri}");
            this.logger.LogInformation($"Test mode: {this.configuration.TestMode}");
            this.logger.LogInformation("MarketData Service start");            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("MarketData Service stop");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if(isDisposed)
            {
                return;
            }
            if(isDisposing)
            {
                
            }
            this.isDisposed = true;
        }
    }
}