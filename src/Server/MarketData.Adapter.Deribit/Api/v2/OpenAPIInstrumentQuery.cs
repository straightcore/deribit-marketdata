using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using Newtonsoft.Json.Linq;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class OpenAPIInstrumentQuery : IInstrumentQuery
    {
        public async Task<IEnumerable<InstrumentDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken)
        {
            var apiInstance = new MarketDataApi(Org.OpenAPITools.Client.Configuration.Default);
            var currency = configuration.Currency.ToString().ToUpper();  // string | The currency symbol
            var kind = configuration.Kind.ToString().ToLower();  // string | Instrument kind, if not provided instruments of all kinds are considered (optional) 
            var expired = true;  // bool? | Set to true to show expired instruments instead of active ones. (optional)  (default to false)

            // try
            // {
                // Retrieves available trading instruments. This method can be used to see which instruments are available for trading, or which instruments have existed historically.
                var result = await apiInstance.PublicGetInstrumentsGetAsync(currency, kind, expired) as JObject;
                return result.ToObject<JsonRpcEnvelopeDto<InstrumentDto[]>>().result;
                // return Enumerable.Empty<InstrumentDto>();
                
            // }
            // catch (ApiException e)
            // {
            //     this.logger.Er("Exception when calling MarketDataApi.PublicGetInstrumentsGet: " + e.Message );
            //     Debug.Print("Status Code: "+ e.ErrorCode);
            //     Debug.Print(e.StackTrace);
            // }
        }
    }
}