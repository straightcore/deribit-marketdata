using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using NSubstitute;

namespace MarketData.Adapter.Deribit.Spec.Mock
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        public Dictionary<string, MockHttpMessageHandler> HttpMessagesHandlerByName { get; } = new Dictionary<string, MockHttpMessageHandler>();

    
        public HttpClient CreateClient(string name)
        {
            var httpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            HttpMessagesHandlerByName[name] = httpMessageHandler;
            var httpClient = Substitute.ForPartsOf<HttpClient>(httpMessageHandler);
            return httpClient;
        }
    }
}