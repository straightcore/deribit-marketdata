using TechTalk.SpecFlow;
using MarketData.Adapter.Deribit.Spec.Drivers;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow.Assist;
using MarketData.Adapter.Deribit.Configuration;
using System;
using System.Net.Http;
using MarketData.Adapter.Deribit.Spec.Mock;
using System.Net;
using System.Text;

namespace MarketData.Adapter.Deribit.Spec.StepDefinitions
{
    [Binding]
    public class Steps
    {
        private readonly ScenarioContext scenarioContext;

        public Steps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext ?? throw new System.ArgumentNullException(nameof(scenarioContext));
        }

        [Given(@"a configuration file")]
        public void GivenAConfigurationFile()
        {
            var hosterBuilder = this.scenarioContext.Get<TestHostBuilder>("hostBuilder");
            hosterBuilder.SetServiceConfiguration("http://localhost/api/v2/public/", 1, false);
        }

        [Given(@"instrument configuration section")]
        public void GivenInstrumentConfigurationSection()
        {
        }


        [Given(@"instrument configuration section with:")]
        public void GivenInstrumentConfigurationSectionWith(Table table)
        {
            var instruments = table.CreateSet<Instrument>();
            var hosterBuilder = this.scenarioContext.Get<TestHostBuilder>("hostBuilder");
            hosterBuilder.SetInstrumentConfiguration(instruments);
        }

        [When(@"the service is started")]
        [Given(@"the service is started")]
        [When(@"the service is starting")]
        public Task WhenServiceIsStarted()
        {
            var builder = this.scenarioContext.Get<TestHostBuilder>("hostBuilder");
            var host = builder.Build();
            this.scenarioContext.Add("host", host);
            this.scenarioContext.Add("serviceProvider", host.Services);

            var factory = host.Services.GetService<IHttpClientFactory>() as TestHttpClientFactory;
            // factory.NextHttpResponseMessage = new HttpResponseMessage()
            // {
            //     StatusCode = HttpStatusCode.OK,
            //     Content = new StringContent(HttpResponseContentBTCFuture, Encoding.UTF8, "application/json")
            // };
            return host.StartAsync(CancellationToken.None);
        }

        [When("the service is stopped")]
        public Task WhenTheServiceIsStopped()
        {
            var host = this.scenarioContext.Get<IHost>("host");
            return host.StopAsync(CancellationToken.None);
        }

        [Then(@"the service close all subcsriptions")]
        public void ThenTheServiceCloseAllSubscription()
        {
            var factory = this.scenarioContext.Get<IServiceProvider>("serviceProvider")
                                              .GetService<IHttpClientFactory>() as TestHttpClientFactory;
            // factory.HttpMessagesHandlerByName; TestHttpClientFactory
        }

        [Then(@"the service fetchs all instruments")]
        public void ThenTheServiceFetchAllInstruments()
        {
            var factory = this.scenarioContext.Get<IServiceProvider>("serviceProvider")
                                            .GetService<IHttpClientFactory>() as TestHttpClientFactory;

        }

        [Then(@"the service is available")]
        public void ThenTheServiceIsAbailable()
        {

        }
    }
}
