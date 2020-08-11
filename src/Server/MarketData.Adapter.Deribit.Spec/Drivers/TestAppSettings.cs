using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;

namespace MarketData.Adapter.Deribit.Spec.Drivers
{
    public class TestAppSettings
    {
        public ServiceConfig ServiceConfiguration { get; set; }
        public InstrumentConfig[] Instruments { get; set; }

        public string Logging { get; set; }
    }
}