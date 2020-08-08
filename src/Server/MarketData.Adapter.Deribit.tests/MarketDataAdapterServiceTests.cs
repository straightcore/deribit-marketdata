using FluentValidation;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using MarketData.Adapter.Deribit.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.tests
{
    [TestFixture]
    [NonParallelizable]
    public class MarketDataAdapterServiceTests
    {
        // [SetUp]
        // public void Setup()
        // {
        // }

        [Test]
        public void Should_does_not_throw_exception_when_start_is_called()
        {
            var serviceProvider = NothingThrowException(GetServiceProvider());
            var service = serviceProvider.GetService<MarketDataAdapterService>();
            Assert.DoesNotThrowAsync(async () => await service.StartAsync(CancellationToken.None));
            // await service.StartAsync(CancellationToken.None);
        }

        [Test]
        public async Task Should_does_not_throw_exception_when_stop_is_called()
        {
            var serviceProvider = NothingThrowException(GetServiceProvider());
            var service = serviceProvider.GetService<MarketDataAdapterService>();
            await service.StartAsync(CancellationToken.None);
            Assert.DoesNotThrowAsync(async () => await service.StopAsync(CancellationToken.None));
        }

        private ServiceProvider NothingThrowException(ServiceProvider provider)
        {
            ServiceSubstituteNotThrowException(provider.GetRequiredService<IValidationConfigurationService>());
            ServiceSubstituteNotThrowException(provider.GetRequiredService<IInstrumentFetcherService>());
            return provider;
        }

        private void ServiceSubstituteNotThrowException(IService service)
        {
            service.StartAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            service.StopAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        }

        private static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                                        .AddLogging(builder => builder.AddConsole())
                                        .AddSingleton<IValidationConfigurationService>(Substitute.For<IValidationConfigurationService>())
                                        .AddSingleton<IInstrumentFetcherService>(Substitute.For<IInstrumentFetcherService>())
                                        .AddTransient<MarketDataAdapterService>()
                                        .BuildServiceProvider();
        }


    }
}