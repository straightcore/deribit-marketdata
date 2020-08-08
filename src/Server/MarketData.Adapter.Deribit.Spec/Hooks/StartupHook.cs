using System;
using System.Threading;
using MarketData.Adapter.Deribit.Spec.Drivers;
using Microsoft.Extensions.Hosting;
using TechTalk.SpecFlow;

namespace MarketData.Adapter.Deribit.Spec.Hooks
{
    [Binding]
    public class StartupHook
    {
        private readonly ScenarioContext context;

        public StartupHook(ScenarioContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));

        [BeforeScenario]
        public void BeforeScenario()
        {
            context.Clear();
            context.Add("hostBuilder", new TestHostBuilder());
        }
        [AfterScenario]
        public void AfterScenario()
        {
            var host =  this.context.Get<IHost>("host");
            host?.StopAsync(CancellationToken.None)
                 .ConfigureAwait(false)
                 .GetAwaiter()
                 .GetResult();
            host?.Dispose();
            context.Remove("hostBuilder");
            context.Clear();
        }
    }
}