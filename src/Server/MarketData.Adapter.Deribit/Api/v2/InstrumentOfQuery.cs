using System;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public interface IInstrumentOfQuery
    {
        Task<IEnumerable<InstrumentDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken);
    }

    public class InstrumentOfQuery : IInstrumentOfQuery
    {
        private readonly ServiceConfig serviceConfig;
        private readonly IHttpClientFactory httpClientFactory;
        private string url;
        private string Url
        {
            get
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = this.serviceConfig.Url;
                    if (!url.EndsWith("/"))
                    {
                        url += "/";
                    }
                }
                return url;
            }
        }

        public InstrumentOfQuery(IHttpClientFactory httpClientFactory, ServiceConfig serviceConfig)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.serviceConfig = serviceConfig ?? throw new ArgumentNullException(nameof(serviceConfig));
        }
        
        public async Task<IEnumerable<InstrumentDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            using (var client = httpClientFactory.CreateClient())
            {
                response = await client.GetAsync($"{this.Url}get_instruments?currency={configuration.Currency}&kind={configuration.Kind}&expired={configuration.Expired}");
            }
            if (response == null)
            {
                throw new Exception("An unknown error occurred while executing the query");
            }
            cancellationToken.ThrowIfCancellationRequested();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");
            }
            var contentStr = await response.Content.ReadAsStringAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return JsonConvert.DeserializeObject<JsonRpcEnvelopeDto<InstrumentDto[]>>(contentStr)?.result ?? Enumerable.Empty<InstrumentDto>();
        }
    }
}