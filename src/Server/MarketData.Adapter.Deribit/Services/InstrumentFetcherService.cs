using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;

namespace MarketData.Adapter.Deribit.Services
{
    public interface IInstrumentFetcherService : IService
    {
        
    }

    public class InstrumentFetcherService : IDisposable, IService, IInstrumentFetcherService
    {
        private readonly IEnumerable<InstrumentConfig> instruments;
        private readonly IInstrumentQuery requester;
        private bool isDisposed;

        public InstrumentFetcherService(IInstrumentQuery requester, IEnumerable<InstrumentConfig> instruments)
        {
            this.requester = requester ?? throw new System.ArgumentNullException(nameof(requester));
            this.instruments = instruments ?? throw new System.ArgumentNullException(nameof(instruments));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var instrumentsPerRequest = await Task.WhenAll(this.instruments.Select(async instr => await this.requester.GetInstrumentsAsync(instr, cancellationToken)));
            var instruments = instrumentsPerRequest.SelectMany(item => item).ToList();
        }

        public Task StopAsync(CancellationToken cancellation)
        {
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

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~InstrumentFetcherService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

    }
}