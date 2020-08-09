using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using MarketData.Adapter.Deribit;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Configuration.Validators;
using MarketData.Adapter.Deribit.Converter;
using MarketData.Adapter.Deribit.Converter.AutoMapper;
using MarketData.Adapter.Deribit.EntityFramework;
using MarketData.Adapter.Deribit.Job;
using MarketData.Adapter.Deribit.Models;
using MarketData.Adapter.Deribit.Repositories;
using MarketData.Adapter.Deribit.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Impl;

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
            services.Configure<List<InstrumentConfig>>(hostContext.Configuration.GetSection("Instruments"));
            services.AddSingleton<ServiceConfig>(provider => provider.GetRequiredService<IOptions<ServiceConfig>>().Value);
            services.AddSingleton<IEnumerable<InstrumentConfig>>(provider => provider.GetRequiredService<IOptions<List<InstrumentConfig>>>().Value);
            services.AddSingleton<IHostedService, MarketDataAdapterService>();
            RequestConfigureServices(services);
            services.AddTransient<IMarketDataFetcherService, MarketDataFetcherService>();
            services.AddTransient<IValidationConfigurationService, ValidationConfigurationService>();
            services.AddSingleton<IValidator<InstrumentConfig>, InstrumentConfigurationValidator>()
                    .AddSingleton<IValidator<ServiceConfig>, ServiceConfigValidator>();
            ConfigureServicesHttpClient(services);
            services.AddSingleton<StdSchedulerFactory, StdSchedulerFactory>();
            services.AddAutoMapper(typeof(TradeProfile));
            services.AddTransient<IConverter<Trade, TradeDto>, AutoMapperTradeConverter>();
            services.AddTransient<ITradeRepository<TradeDto>, TradeDtoRepository>()
                    .AddTransient<ITradeRepository<Trade>, EntityFrameworkTradeRepository>();
            services.AddTransient<DownloadMarketDataJobDetailBuilder>();
            services.AddDbContext<MarketDataDbContext>();
        }

        protected virtual void RequestConfigureServices(IServiceCollection services)
        {
            services.AddTransient<OpenAPIInstrumentQuery>();
            services.AddTransient<IInstrumentQuery>(provider =>
                new InstrumentQueryLogger(provider.GetRequiredService<OpenAPIInstrumentQuery>(), provider.GetRequiredService<ILogger<OpenAPIInstrumentQuery>>()));
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
