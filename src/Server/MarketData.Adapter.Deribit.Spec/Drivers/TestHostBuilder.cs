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
        private ServiceConfig serviceConfiguration = new ServiceConfig();
        

        public TestHostBuilder SetInstrumentConfiguration(IEnumerable<MarketData.Adapter.Deribit.Configuration.InstrumentConfig> instruments)
        {
            this.serviceConfiguration.Instruments = instruments?.ToArray();
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
            return new StartupBdd(new TestAppSettings() { ServiceConfiguration = this.serviceConfiguration })
                        .CreateBuilder()
                        .Build();
        }
    }
}