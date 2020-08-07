using System;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MarketData.Adapter.Deribit
{
    public class MarketDataAdapterService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly ServiceConfig configuration;
        private bool isDisposed = false;

        public bool IsStart { get; set; }

        public MarketDataAdapterService(ILogger<MarketDataAdapterService> logger, IOptions<ServiceConfig> options)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(IsStart)
            {
                this.logger.LogWarning("Service is already start");
                return Task.CompletedTask;    
            }
            this.logger.LogInformation("MarketData Service start");
            IsStart = true;
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            if(!IsStart)
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