using FluentValidation;
using MarketData.Adapter.Deribit;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MarketData.Adapter.Deribit.Host
{
    public class Startup
    {
        
        public IHostBuilder CreateBuilder(string[] args = null)
        {
            return new HostBuilder()
               .ConfigureAppConfiguration((hostingContext, config) => ConfigureAppConfiguration(args, config))
               .ConfigureServices(ConfigureServices)
               .ConfigureLogging(ConfigureLogging);               
        }

        private static void ConfigureLogging(HostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            // Need to resolve the copy appsettings file
            // logging.ClearProviders();
            // logging.AddLog4Net(hostingContext.Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>());
        }

        protected virtual void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ServiceConfig>(hostContext.Configuration.GetSection("ServiceConfiguration"));
            services.AddSingleton<ServiceConfig>(provider => provider.GetRequiredService<IOptions<ServiceConfig>>().Value);
            services.AddSingleton<IHostedService, MarketDataAdapterService>();
            services.AddTransient<InstrumentOfQuery>();
            services.AddTransient<IInstrumentOfQuery>(provider =>
                new InstrumentOfQueryLogger(provider.GetRequiredService<InstrumentOfQuery>(), provider.GetRequiredService<ILogger<InstrumentOfQuery>>()));

            services.AddSingleton<InstrumentConfigurationValidator>()
                    .AddSingleton<IValidator<ServiceConfig>, ServiceConfigValidator>();
            ConfigureServicesHttpClient(services);
        }

        protected virtual void ConfigureServicesHttpClient(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        protected virtual void ConfigureAppConfiguration(string[] args, IConfigurationBuilder config)
        {
            config.AddEnvironmentVariables();
            string configFile = GetConfigurationFileOrDefault(args);
            config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }

        private static string GetConfigurationFileOrDefault(string[] args)
        {
            var cmdOptions = new ConfigurationBuilder().AddCommandLine(args).Build();
            var configFile = cmdOptions.GetValue<string>("config", "appsettings.json");
            return configFile;
        }
    }
}
