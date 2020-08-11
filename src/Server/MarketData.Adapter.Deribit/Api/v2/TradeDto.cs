using Newtonsoft.Json;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public class TradeDto
    {
        /// <summary>
        /// 	Trade amount. For perpetual and futures - in USD units, for options it is amount of corresponding cryptocurrency contracts, e.g., BTC or ETH.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 	Block trade id - when trade was part of block trade
        /// </summary>
        /// <value></value>
        [JsonProperty("block_trade_id")]
        public string BlockTradeId { get; set; }
        /// <summary>
        /// Direction: buy, or sell
        /// </summary>
        /// <value></value>
        [JsonProperty("direction")]
        public string Direction { get; set; }
        /// <summary>
        /// 	Index Price at the moment of trade
        /// </summary>
        /// <value></value>
        [JsonProperty("index_price")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// 	Unique instrument identifier
        /// </summary>
        /// <param name="only"></param>
        /// <returns></returns>
        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; }
        /// <summary>
        /// 	Option implied volatility for the price (Option only)
        /// </summary>
        /// <param name="liquidation"></param>
        /// <returns></returns>
        [JsonProperty("iv")]
        public decimal? Volatility { get; set; }
        /// <summary>
        /// Optional field (only for trades caused by liquidation): "M" when maker side of trade was under liquidation, "T" when taker side was under liquidation, "MT" when both sides of trade were under liquidation
        /// </summary>
        /// <value></value>
        [JsonProperty("liquidation")]
        public string Liquidation { get; set; }
        /// <summary>
        /// Mark Price at the moment of trade
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>	
        [JsonProperty("mark_price")]
        public decimal MarkPrice { get; set; }
        /// <summary>
        /// Price in base currency
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>	
        [JsonProperty("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Direction of the "tick" (0 = Plus Tick, 1 = Zero-Plus Tick, 2 = Minus Tick, 3 = Zero-Minus Tick).
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        [JsonProperty("tick_direction")]
        public int TickDirection { get; set; }
        /// <summary>
        /// The timestamp of the trade
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        /// <summary>
        /// Unique (per currency) trade identifier
        /// </summary>
        /// <value></value>
        [JsonProperty("trade_id")]
        public string TradeId { get; set; }
        /// <summary>
        /// The sequence number of the trade within instrument
        /// </summary>
        /// <value></value>	
        [JsonProperty("trade_seq")]
        public int TradeSequence { get; set; }
    }
}