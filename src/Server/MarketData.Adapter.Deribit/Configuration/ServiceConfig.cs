using System;

namespace MarketData.Adapter.Deribit.Configuration
{
    public class ServiceConfig
    {
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Allows to configure the authentification arguments to connect at the platform
        /// </summary>
        public Authentification Authentification { get; set; }
        /// <summary>
        /// Gets or sets frequency of fetch action. The unit is seconds. 
        /// </summary>
        public int FetchInterval {get;set;}
        public bool TestMode { get; set; }

        public Instrument[] Instruments { get; set; }
    }
}