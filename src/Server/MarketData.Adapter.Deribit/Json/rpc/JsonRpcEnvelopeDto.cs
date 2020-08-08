using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit.Json.rpc
{
    public class JsonRpcEnvelopeDto<T>
    {
        [JsonProperty("id")]
        public int Id {get;set;}
        [JsonProperty("jsonrpc")]
        public string JsonRPCVersion { get; set; }
        
        [JsonProperty("result")]
        public T result { get; set; }
        
    }
}