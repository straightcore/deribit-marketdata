using System.Collections.Generic;
using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class TradeResponseDto
    {
        [JsonProperty("trades")]
        public IEnumerable<TradeDto> Trades { get; set; }
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }
}