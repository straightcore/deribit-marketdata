using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using Newtonsoft.Json.Linq;
using Org.OpenAPITools.Api;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class OpenAPIInstrumentQuery : IInstrumentQuery
    {
        public async Task<IEnumerable<InstrumentResponseDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken)
        {
            var apiInstance = new MarketDataApi(Org.OpenAPITools.Client.Configuration.Default);
            var currency = configuration.Currency.ToString().ToUpper();  // string | The currency symbol
            var kind = configuration.Kind.ToString().ToLower();  // string | Instrument kind, if not provided instruments of all kinds are considered (optional) 
            var expired = configuration.Expired;  // bool? | Set to true to show expired instruments instead of active ones. (optional)  (default to false)
            var result = await apiInstance.PublicGetInstrumentsGetAsync(currency, kind, expired) as JObject;
            return result.ToObject<JsonRpcEnvelopeDto<InstrumentResponseDto[]>>().result;
        }
    }
}