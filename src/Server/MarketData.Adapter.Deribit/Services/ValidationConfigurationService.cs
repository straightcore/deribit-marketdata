using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MarketData.Adapter.Deribit.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit.Services
{
    public interface IValidationConfigurationService : IService, IDisposable
    {
    }

    public class ValidationConfigurationService : IService, IDisposable, IValidationConfigurationService
    {
        private bool isDisposed;
        private readonly ILogger<ValidationConfigurationService> logger;
        private readonly ServiceConfig serviceConfig;
        private readonly IValidator<ServiceConfig> serviceConfigValidator;
        private readonly IEnumerable<InstrumentConfig> instruments;
        private readonly IValidator<InstrumentConfig> instrumentValidator;

        public ValidationConfigurationService(ILogger<ValidationConfigurationService> logger,
                                              ServiceConfig serviceConfig,
                                              IValidator<ServiceConfig> serviceConfigValidator,
                                              IEnumerable<InstrumentConfig> instruments,
                                              IValidator<InstrumentConfig> instrumentValidator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceConfig = serviceConfig ?? throw new ArgumentNullException(nameof(serviceConfig));
            this.serviceConfigValidator = serviceConfigValidator ?? throw new ArgumentNullException(nameof(serviceConfigValidator));
            this.instruments = instruments ?? throw new ArgumentNullException(nameof(instruments));
            this.instrumentValidator = instrumentValidator ?? throw new ArgumentNullException(nameof(instrumentValidator));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var taskServiceValidation = ValidateServiceConfiguration(cancellationToken);
            var taskInstrumentsValidation = ValidateInstrumentsConfiguration(cancellationToken);
            var serviceValidation = await taskServiceValidation;
            var results = (await taskInstrumentsValidation).ToList();
            results.Add(serviceValidation);
            if (results.Any(result => !result.IsValid))
            {
                throw new ValidationException($"Configuration invalid has been detected.", results.OfType<ValidationFailure>());
            }
        }

        private async Task<IEnumerable<ValidationResult>> ValidateInstrumentsConfiguration(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Validating instruments configuration...");
            var results = await Task.WhenAll(this.instruments.Select(instr => this.instrumentValidator.ValidateAsync(instr, cancellationToken)));
            var errorResults = results.Where(result => !result.IsValid);
            if (errorResults.Any())
            {
                errorResults = errorResults.ToList();
                this.logger.LogCritical($"{errorResults.Count()} instrument configuration is not valid. Please check them.");
                foreach (var message in errorResults.SelectMany(result => result.Errors).Select(error => $"{error.ErrorCode}: {error.ErrorMessage}").Distinct())
                {
                    this.logger.LogError(message);
                }
            }
            return results;
        }

        private async Task<ValidationResult> ValidateServiceConfiguration(CancellationToken cancellationToken)
        {
            CheckIfDisposed();
            this.logger.LogInformation("Validating service http requester configuration...");
            this.logger.LogDebug("configuration: {0}", JsonConvert.SerializeObject(this.serviceConfig));
            var result = await this.serviceConfigValidator.ValidateAsync(this.serviceConfig, cancellationToken);
            if (result.IsValid)
            {
                return result;
            }
            this.logger.LogCritical("Http Request configuration is not valid. Please check it.");
            foreach (var error in result.Errors)
            {
                this.logger.LogError($"{error.ErrorCode}: {error.ErrorMessage}");
            }
            return result;
        }

        public Task StopAsync(CancellationToken cancellation)
        {
            CheckIfDisposed();
            return Task.CompletedTask;
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
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        private void CheckIfDisposed()        
        {
            if(isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ValidationConfigurationService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

    }
}