using System;

namespace MarketData.Adapter.Deribit.Configuration
{
    public class Instrument
    {
        /// <summary>
        /// Gets or sets the currency for this intrument
        /// </summary>        
        public Currency Currency { get; set; }
        public Kind[] Kind { get; set; }
        public bool Expired { get; set; }
    }

    public enum Kind
    {
        Future,
        Options
    }
    public enum Currency
    {
        BTC,
        ETH
    }
}