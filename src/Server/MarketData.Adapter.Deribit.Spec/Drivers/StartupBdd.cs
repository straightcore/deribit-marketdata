using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentValidation;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Host;
using MarketData.Adapter.Deribit.Spec.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MarketData.Adapter.Deribit.Spec.Drivers
{
    public class StartupBdd : Startup
    {
        private string configuration;

        public StartupBdd(TestAppSettings configuration)
        {
            var config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.configuration = Newtonsoft.Json.JsonConvert.SerializeObject(config);
        }

        protected override void ConfigureAppConfiguration(string[] args, IConfigurationBuilder config)
        {
            config.AddEnvironmentVariables();
            // config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
            config.AddJsonStream(TransformStringToStream(this.configuration));
        }

        protected override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            base.ConfigureServices(hostContext, services);
            services.Remove(services.First(descriptor => descriptor.ImplementationType == typeof(MarketDataAdapterService)));
            services.AddSingleton<IHostedService>(CreateHostedServiceSubstitute);
        }

        private IHostedService CreateHostedServiceSubstitute(IServiceProvider serviceProvider)
        {
            return Substitute.ForPartsOf<MarketDataAdapterService>(serviceProvider.GetRequiredService<ILogger<MarketDataAdapterService>>(),
                 serviceProvider.GetRequiredService<ServiceConfig>(),
                 serviceProvider.GetRequiredService<IValidator<ServiceConfig>>(),
                 serviceProvider.GetRequiredService<IInstrumentOfQuery>());
        }

        protected override void ConfigureServicesHttpClient(IServiceCollection services)
        {     
            services.AddSingleton<IHttpClientFactory, TestHttpClientFactory>();
        }

        private static MemoryStream TransformStringToStream(string configuration)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(configuration);
            writer.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}