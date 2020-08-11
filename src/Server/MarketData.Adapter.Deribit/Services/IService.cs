using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.Services
{
    public interface IService : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellation);
    }

}