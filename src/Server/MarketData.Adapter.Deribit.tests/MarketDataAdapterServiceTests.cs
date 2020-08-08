using FluentValidation;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using Microsoft.Extensions.DependencyInjection;
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
            var serviceProvider = GetServiceProvider();
            var service = serviceProvider.GetService<MarketDataAdapterService>();
            Assert.DoesNotThrowAsync(async () => await service.StartAsync(CancellationToken.None).ConfigureAwait(true));
        }

        [Test]
        public async Task Should_does_not_throw_exception_when_stop_is_called()
        {
            var serviceProvider = GetServiceProvider();
            var service = serviceProvider.GetService<MarketDataAdapterService>();
            await service.StartAsync(CancellationToken.None).ConfigureAwait(true);
            Assert.DoesNotThrowAsync(async () => await service.StopAsync(CancellationToken.None).ConfigureAwait(true));
        }

        private static ServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                                        .AddLogging(builder => builder.AddConsole())
                                        .AddSingleton<IValidator<ServiceConfig>, ServiceConfigValidator>()
                                        .AddSingleton<InstrumentConfigurationValidator>()
                                        .AddTransient<MarketDataAdapterService>()
                                        .AddSingleton<ServiceConfig>(provider => new ServiceConfig()
                                        {
                                            Url = "https://test.deribit.com/api/v2/public/",
                                            FetchInterval = 1,
                                            TestMode = true,
                                        })
                                        .AddSingleton<IInstrumentOfQuery>(Substitute.For<IInstrumentOfQuery>())
                                        .BuildServiceProvider();
        }


    }
}