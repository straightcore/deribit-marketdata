using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class InstrumentResponseDto
    {
        [JsonProperty("leverage")]
        public int? Leverage { get; set; }
        [JsonProperty("contract_size")]
        public int? ContractSize { get; set; }
        [JsonProperty("creation_timestamp")]
        public long? CreationTimestamp { get; set; }
        [JsonProperty("expiration_timestamp")]
        public long? ExpirationTimestamp { get; set; }
        [JsonProperty("strike")]
        public decimal? Strike { get; set; }
        [JsonProperty("taker_commission")]
        public decimal? TakerCommission { get; set; }
        [JsonProperty("tick_size")]
        public decimal? TickSize { get; set; }
        [JsonProperty("maker_commission")]
        public decimal? MakerCommission { get; set; }
        [JsonProperty("min_trade_amount")]
        public decimal? MinTradeAmount { get; set; }
        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }
        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }
        [JsonProperty("instrument_name")]
        public string Name { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("option_type")]
        public string OptionType { get; set; }
        [JsonProperty("quote_currency")]
        public string QuoteCurrency { get; set; }
        [JsonProperty("settlement_period")]
        public string SettlementPeriod { get; set; }

    }
}