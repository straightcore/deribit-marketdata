using MarketData.Adapter.Deribit.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.tests
{
    public class MarketDataAdapterServiceTests
    {
        // [SetUp]
        // public void Setup()
        // {
        // }

        [Test]
        public void Should_does_not_throw_exception_when_start_is_called()
        {
            var service = new MarketDataAdapterService(Substitute.For<ILogger<MarketDataAdapterService>>(), Substitute.For<IOptions<ServiceConfig>>());
            Assert.DoesNotThrowAsync(async () => await service.StartAsync(CancellationToken.None).ConfigureAwait(true));
        }

        [Test]
        public async Task Should_does_not_throw_exception_when_stop_is_called()
        {
            var service = new MarketDataAdapterService(Substitute.For<ILogger<MarketDataAdapterService>>(), Substitute.For<IOptions<ServiceConfig>>());
            await service.StartAsync(CancellationToken.None).ConfigureAwait(true);
            Assert.DoesNotThrowAsync(async () => await service.StopAsync(CancellationToken.None).ConfigureAwait(true));
        }

        
    }
}