using System;
using System.Threading.Tasks;
using MarketDataAdapterService.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace MarketData.Adapter.Deribit.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddEnvironmentVariables();
                   string configFile = GetConfigurationFileOrDefault(args);
                   config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
                   if (args != null)
                   {
                       config.AddCommandLine(args);
                   }
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddOptions();
                   services.Configure<ServiceConfig>(hostContext.Configuration.GetSection("Deribit"));

                   services.AddSingleton<IHostedService, MarketDataAdapterService>();
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   logging.AddConsole();
                   // Need to resolve the copy appsettings file
                // logging.ClearProviders();
                // logging.AddLog4Net(hostingContext.Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>());
               });

            await builder.RunConsoleAsync();
        }

        private static string GetConfigurationFileOrDefault(string[] args)
        {
            var cmdOptions = new ConfigurationBuilder().AddCommandLine(args).Build();
            var configFile = cmdOptions.GetValue<string>("config", "appsettings.json");
            return configFile;
        }
    }
}
