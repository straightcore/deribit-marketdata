using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using Microsoft.Extensions.Hosting;

namespace MarketData.Adapter.Deribit.Spec.Drivers
{
    public class TestHostBuilder 
    {
        private const string LoggingConfigurationPart= "{ \"LogLevel\": { \"Default\": \"Debug\", \"Microsoft\": \"Warning\" }, \"Debug\": { \"LogLevel\": { \"Default\": \"Debug\", \"Microsoft.Hosting\": \"Trace\" } }, \"Console\": { \"IncludeScopes\": true, \"LogLevel\": { \"Default\": \"Debug\", \"Microsoft\": \"Warning\" } } }";
        private ServiceConfig serviceConfiguration = new ServiceConfig();
        
        private IEnumerable<InstrumentConfig> instruments = null;

        public TestHostBuilder SetInstrumentConfiguration(IEnumerable<InstrumentConfig> instruments)
        {
            this.instruments = instruments;
            return this;
        }

        public TestHostBuilder SetServiceConfiguration(string url, int fetchIntervalSeconds, bool testMode = true)
        {
            this.serviceConfiguration.Url = url;
            this.serviceConfiguration.FetchInterval = fetchIntervalSeconds;
            this.serviceConfiguration.TestMode = testMode;
            return this;
        }

        public TestHostBuilder SetAuthentificationConfiguration(string clientId, string clientSecret)
        {
            this.serviceConfiguration.Authentification = new AuthentificationConfig()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
            return this;
        }

        public IHost Build()
        {
            return new StartupBdd(new TestAppSettings() { ServiceConfiguration = this.serviceConfiguration, Instruments = this.instruments?.ToArray(), Logging = LoggingConfigurationPart })
                        .CreateBuilder()
                        .Build();
        }
    }
}