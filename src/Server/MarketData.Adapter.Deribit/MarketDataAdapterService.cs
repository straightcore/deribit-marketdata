using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit
{
    public class MarketDataAdapterService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly ServiceConfig configuration;
        private bool isDisposed = false;
        private readonly IValidator<ServiceConfig> configValidator;

        public bool IsStart { get; set; }
        private readonly IInstrumentOfQuery instrumentQuery;

        public MarketDataAdapterService(ILogger<MarketDataAdapterService> logger, ServiceConfig configuration, IValidator<ServiceConfig> configValidator, IInstrumentOfQuery instrumentQuery)
        {
            this.instrumentQuery = instrumentQuery ?? throw new ArgumentNullException(nameof(instrumentQuery));
            this.configValidator = configValidator ?? throw new ArgumentNullException(nameof(configValidator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = (configuration ?? throw new ArgumentNullException(nameof(configuration)));
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsStart)
            {
                this.logger.LogWarning("Service is already start");
                return;
            }
            await ValidateConfiguration(cancellationToken);
            if(this.configuration.Instruments?.Any() ?? false)
            {
                var instrumentsJsonRpc = await Task.WhenAll(this.configuration.Instruments?.Select(instrument => this.instrumentQuery.GetInstrumentsAsync(instrument, cancellationToken)));
                foreach(var instrument in instrumentsJsonRpc.SelectMany(jsonRpc => jsonRpc.result))
                {
                    this.logger.LogInformation(JsonConvert.SerializeObject(instrument));
                }
            }
            this.logger.LogInformation("MarketData Service is started");
            IsStart = true;
        }

        private async Task ValidateConfiguration(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Validating configuration...");
            this.logger.LogDebug($"configuration: {JsonConvert.SerializeObject(this.configuration)}");
            var result = await this.configValidator.ValidateAsync(this.configuration, cancellationToken);
            if (!result.IsValid)
            {
                this.logger.LogCritical("Configuration of service is not valid");
                foreach (var error in result.Errors)
                {
                    this.logger.LogError($"[{error.ErrorCode}] {error.ErrorMessage}");
                }
                throw new AggregateException("Configuration of service is not valid", result.Errors.Select(error => new Exception($"{error.ErrorCode}: {error.ErrorMessage}")));
            }
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