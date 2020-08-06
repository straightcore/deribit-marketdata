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
            await new Startup().CreateBuilder(args).RunConsoleAsync();
        }   
    }
}
