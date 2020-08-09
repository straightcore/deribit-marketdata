using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketData.Adapter.Deribit.Api.v2;
using MarketData.Adapter.Deribit.Configuration;
using MarketData.Adapter.Deribit.Json.rpc;
using MarketData.Adapter.Deribit.tests.Mock;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace MarketData.Adapter.Deribit.tests.Api.v2
{
    [TestFixture]
    [NonParallelizable]
    public class InstrumentOfQueryTests
    {
        // [Test]
        // public async Task Should_query_http_service_when_call_get_instrument()
        // {
        //     var tuple = BuildHttpClientFactory(new JsonRpcEnvelopeDto<InstrumentDto>() { Id = 1, JsonRPCVersion = "2" });
        //     var httpFactory = tuple.Item1;
        //     var httpRequest = tuple.Item2;
        //     var serviceConfig = new Configuration.ServiceConfig()
        //     {
        //         Url = "https://test.deribit.com/api/v2/public/",
        //         FetchInterval = 1,
        //         TestMode = true,
        //     };
        //     var query = new InstrumentOfQuery(httpFactory, serviceConfig);
        //     var configuration = new InstrumentConfig() { Currency = Currency.BTC, Kind = Kind.Future, Expired = false };
        //     var resultQuery = await query.GetInstrumentsAsync(configuration, CancellationToken.None);
        //     Assert.That(resultQuery, Is.Not.Null);
        //     httpFactory.Received(1).CreateClient(Arg.Any<string>());
        //     await httpRequest.Received(1)
        //         .SendAsyncOverride(Arg.Is<HttpRequestMessage>(message => string.Equals(message.RequestUri.OriginalString,   
        //             $"https://test.deribit.com/api/v2/public/get_instruments?currency={configuration.Currency}&kind={configuration.Kind}&expired={configuration.Expired}"))
        //             , Arg.Any<CancellationToken>());            
        // }

        private static Tuple<IHttpClientFactory, MockHttpMessageHandler> BuildHttpClientFactory(JsonRpcEnvelopeDto<InstrumentResponseDto> response)
        {
            var httpFactory = Substitute.For<IHttpClientFactory>();
            var fakeHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
            });
            var httpClient = Substitute.ForPartsOf<HttpClient>(fakeHttpMessageHandler);
            httpFactory.CreateClient().Returns(httpClient);
            httpFactory.ClearReceivedCalls();
            return new Tuple<IHttpClientFactory, MockHttpMessageHandler>(httpFactory, fakeHttpMessageHandler);
        }
    }
}