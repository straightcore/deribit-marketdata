using System;

namespace MarketDataAdapterService.Configuration
{
    public class ServiceConfig
    {
        public string Uri { get; set; }
        public Authentification Authentification { get; set; }
        /// <summary>
        /// Gets or sets frequency of notifications. 
        /// Events will be aggregated over this interval. 
        /// The value raw means no aggregation will be applied.
        /// e.g: 100ms, 1000ms
        /// </summary>
        public string FetchInterval {get;set;}
        public bool TestMode { get; set; }
    }
}