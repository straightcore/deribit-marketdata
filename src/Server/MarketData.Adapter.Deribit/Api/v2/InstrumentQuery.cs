using System;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using System.Net.Http;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MarketData.Adapter.Deribit.Api.v2
{
    public interface IInstrumentQuery
    {
        Task<IEnumerable<InstrumentResponseDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken);
    }

    public class InstrumentQuery : IInstrumentQuery
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

        public InstrumentQuery(IHttpClientFactory httpClientFactory, ServiceConfig serviceConfig)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.serviceConfig = serviceConfig ?? throw new ArgumentNullException(nameof(serviceConfig));
        }
        
        public async Task<IEnumerable<InstrumentResponseDto>> GetInstrumentsAsync(InstrumentConfig configuration, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            using (var client = httpClientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync($"{this.Url}get_instruments?currency={configuration.Currency}&kind={configuration.Kind}&expired={configuration.Expired}");
            }
            if (response == null)
            {
                throw new Exception("An unknown error occurred while executing the query");
            }
            cancellationToken.ThrowIfCancellationRequested();
            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<InstrumentResponseDto>();
                // throw new Exception($"{response.StatusCode}: {response.ReasonPhrase}");
            }
            var contentStr = await response.Content.ReadAsStringAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return JsonConvert.DeserializeObject<JsonRpcEnvelopeDto<InstrumentResponseDto[]>>(contentStr)?.result ?? Enumerable.Empty<InstrumentResponseDto>();
        }
    }
}