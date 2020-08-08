using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace MarketData.Adapter.Deribit.Configuration
{
    public class InstrumentConfig
    {
        /// <summary>
        /// Gets or sets the currency for this intrument
        /// supported values: BTC, ETH
        /// </summary>        
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency? Currency { get; set; }
        /// <summary>
        /// Gets or sets the kind for this intrument (Type of instrument)
        /// supported values: Future, Options
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Kind? Kind { get; set; }
        /// <summary>
        /// Gets or sets if the query includes the expired instruments
        /// By default not
        /// </summary>  
        public bool Expired { get; set; }
    }

    public enum Kind
    {
        Future,
        Option
    }
    public enum Currency
    {
        BTC,
        ETH
    }
}