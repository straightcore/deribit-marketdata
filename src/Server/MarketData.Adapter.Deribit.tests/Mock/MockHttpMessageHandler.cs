using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.tests.Mock
{
    public class MockHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsyncOverride(request, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsyncOverride(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_fakeResponse);
        }
    }
}