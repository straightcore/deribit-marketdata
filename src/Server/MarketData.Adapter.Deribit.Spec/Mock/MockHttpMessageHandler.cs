
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.Spec.Mock
{
    public class MockHttpMessageHandler : DelegatingHandler
    {
        private const string HttpResponseContentBTCFuture = "{ \"jsonrpc\": \"2.0\", \"id\": 5, \"result\": [ { \"tick_size\": 0.5, \"taker_commission\": 0.0005, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 10, \"max_liquidation_commission\": 0.005, \"max_leverage\": 100, \"maker_commission\": -0.0002, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"BTC-25SEP20\", \"expiration_timestamp\": 1601020800000, \"creation_timestamp\": 1584691203000, \"contract_size\": 10, \"block_trade_commission\": 0.0001, \"base_currency\": \"BTC\" }, { \"tick_size\": 0.5, \"taker_commission\": 0.00075, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 10, \"max_liquidation_commission\": 0.005, \"max_leverage\": 100, \"maker_commission\": -0.0002, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"BTC-25DEC20\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1591954980000, \"contract_size\": 10, \"block_trade_commission\": 0.0001, \"base_currency\": \"BTC\" }, { \"tick_size\": 0.5, \"taker_commission\": 0.00075, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 10, \"max_liquidation_commission\": 0.005, \"max_leverage\": 100, \"maker_commission\": -0.0002, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"BTC-26MAR21\", \"expiration_timestamp\": 1616745600000, \"creation_timestamp\": 1593072007000, \"contract_size\": 10, \"block_trade_commission\": 0.0001, \"base_currency\": \"BTC\" }, { \"tick_size\": 0.5, \"taker_commission\": 0.00075, \"settlement_period\": \"perpetual\", \"quote_currency\": \"USD\", \"min_trade_amount\": 10, \"max_liquidation_commission\": 0.005, \"max_leverage\": 100, \"maker_commission\": -0.00025, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"BTC-PERPETUAL\", \"expiration_timestamp\": 32503708800000, \"creation_timestamp\": 1534167754000, \"contract_size\": 10, \"block_trade_commission\": 0.0001, \"base_currency\": \"BTC\" } ], \"usIn\": 1596884357108985, \"usOut\": 1596884357109344, \"usDiff\": 359, \"testnet\": true}";
        private const string HttpResponseContentBTCOption = "{ \"jsonrpc\": \"2.0\", \"result\": [ { \"tick_size\": 0.05, \"taker_commission\": 0.0005, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": 0, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1592553660000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.05, \"taker_commission\": 0.0005, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": 0, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-25SEP20\", \"expiration_timestamp\": 1601020800000, \"creation_timestamp\": 1584691260000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.05, \"taker_commission\": 0.00075, \"settlement_period\": \"perpetual\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": -0.00025, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-PERPETUAL\", \"expiration_timestamp\": 32503734000000, \"creation_timestamp\": 1547647327000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" } ], \"usIn\": 1596884522414077, \"usOut\": 1596884522414378, \"usDiff\": 301, \"testnet\": true}";
        private const string HttpResponseContentETHFuture = "{ \"jsonrpc\": \"2.0\", \"result\": [ { \"tick_size\": 0.05, \"taker_commission\": 0.0005, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": 0, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1592553660000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.05, \"taker_commission\": 0.0005, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": 0, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-25SEP20\", \"expiration_timestamp\": 1601020800000, \"creation_timestamp\": 1584691260000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.05, \"taker_commission\": 0.00075, \"settlement_period\": \"perpetual\", \"quote_currency\": \"USD\", \"min_trade_amount\": 1, \"max_liquidation_commission\": 0.01, \"max_leverage\": 50, \"maker_commission\": -0.00025, \"kind\": \"future\", \"is_active\": true, \"instrument_name\": \"ETH-PERPETUAL\", \"expiration_timestamp\": 32503734000000, \"creation_timestamp\": 1547647327000, \"contract_size\": 1, \"block_trade_commission\": 0.0001, \"base_currency\": \"ETH\" } ], \"usIn\": 1596885675428312, \"usOut\": 1596885675428585, \"usDiff\": 273, \"testnet\": true}";
        private const string HttpResponseContentETHOption = "{ \"jsonrpc\": \"2.0\", \"result\": [ { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 390, \"settlement_period\": \"day\", \"quote_currency\": \"USD\", \"option_type\": \"call\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-10AUG20-390-C\", \"expiration_timestamp\": 1597046400000, \"creation_timestamp\": 1596873660000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 400, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-25SEP20-400-P\", \"expiration_timestamp\": 1601020800000, \"creation_timestamp\": 1586155320000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 345, \"settlement_period\": \"day\", \"quote_currency\": \"USD\", \"option_type\": \"call\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-9AUG20-345-C\", \"expiration_timestamp\": 1596960000000, \"creation_timestamp\": 1596819660000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 500, \"settlement_period\": \"week\", \"quote_currency\": \"USD\", \"option_type\": \"call\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-21AUG20-500-C\", \"expiration_timestamp\": 1597996800000, \"creation_timestamp\": 1596700860000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 520, \"settlement_period\": \"week\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-14AUG20-520-P\", \"expiration_timestamp\": 1597392000000, \"creation_timestamp\": 1596340260000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 800, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20-800-P\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1595888280000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 120, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20-120-P\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1593072060000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 480, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20-480-P\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1593072060000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 760, \"settlement_period\": \"month\", \"quote_currency\": \"USD\", \"option_type\": \"call\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-25DEC20-760-C\", \"expiration_timestamp\": 1608883200000, \"creation_timestamp\": 1595751840000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 260, \"settlement_period\": \"week\", \"quote_currency\": \"USD\", \"option_type\": \"call\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-21AUG20-260-C\", \"expiration_timestamp\": 1597996800000, \"creation_timestamp\": 1596819720000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" }, { \"tick_size\": 0.0005, \"taker_commission\": 0.0004, \"strike\": 430, \"settlement_period\": \"week\", \"quote_currency\": \"USD\", \"option_type\": \"put\", \"min_trade_amount\": 1, \"maker_commission\": 0.0004, \"kind\": \"option\", \"is_active\": true, \"instrument_name\": \"ETH-21AUG20-430-P\", \"expiration_timestamp\": 1597996800000, \"creation_timestamp\": 1596700860000, \"contract_size\": 1, \"block_trade_commission\": 0.00015, \"base_currency\": \"ETH\" } ],\"usIn\": 1596885755054298, \"usOut\": 1596885755059924, \"usDiff\": 5626, \"testnet\": true}";

        private readonly HttpResponseMessage response;

        // public MockHttpMessageHandler(HttpResponseMessage response) => this.response = response ?? throw new System.ArgumentNullException(nameof(response));
        public MockHttpMessageHandler()
        {

        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsyncOverride(request, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsyncOverride(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RequestUri.Query))
            {
                return Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                });
            }
            var queryMapping = new HashSet<string>(request.RequestUri.Query.Replace("?", string.Empty).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(item => item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Last()));
            var isFuture = queryMapping.Contains("Future", StringComparer.InvariantCultureIgnoreCase);
            if (queryMapping.Contains("BTC", StringComparer.InvariantCultureIgnoreCase) && isFuture)
            {
                return Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(HttpResponseContentBTCFuture, Encoding.UTF8, "application/json")
                });
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(HttpResponseContentBTCOption, Encoding.UTF8, "application/json")
                });
            }
            if (queryMapping.Contains("ETH", StringComparer.InvariantCultureIgnoreCase) && isFuture)
            {
                return Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(HttpResponseContentETHFuture, Encoding.UTF8, "application/json")
                });
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(HttpResponseContentETHOption, Encoding.UTF8, "application/json")
                });
            }
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
            });
        }
    }
}