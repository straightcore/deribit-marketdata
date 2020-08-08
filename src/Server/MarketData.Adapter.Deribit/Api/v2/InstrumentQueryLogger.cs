using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using Microsoft.Extensions.Logging;
using Org.OpenAPITools.Client;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class InstrumentQueryLogger : IInstrumentQuery
    {
        private readonly IInstrumentQuery decorated;
        private readonly ILogger logger;
        public InstrumentQueryLogger(IInstrumentQuery decorated, ILogger logger)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.decorated = decorated ?? throw new System.ArgumentNullException(nameof(decorated));

        }
        public Task<IEnumerable<InstrumentDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("Fetch instrument Currency '{0}' and Kind '{1}'", configuration.Currency, configuration.Kind);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var task = decorated.GetInstrumentsAsync(configuration, cancellationToken);
                task.ConfigureAwait(true);
                return task;
            }
            catch(ApiException ex)
            {
                this.logger.LogError("Exception when calling MarketDataApi.PublicGetInstrumentsGet: " + ex.Message );
                this.logger.LogDebug("Status Code: "+ ex.ErrorCode);
                this.logger.LogDebug(ex.StackTrace);
                throw ex;
            }
            catch(Exception exception)
            {
                this.logger.LogError(exception, "En error occured while executing http request");
                throw exception;
            }
        }
    }
}