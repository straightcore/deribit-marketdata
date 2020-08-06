using System;

namespace MarketDataAdapterService.Configuration
{
    public class ServiceConfig
    {
        public string Uri { get; set; }
        public Authentification Authentification { get; set; }

        public bool TestMode { get; set; }
    }
}