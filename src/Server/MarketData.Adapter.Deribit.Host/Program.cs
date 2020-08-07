using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


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
