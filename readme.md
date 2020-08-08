# Deribit Marketdata Adapter project

> # TODO:
> [!WARNING]
1- Use credential to connect by websocket // awaiting receive email to confirm my account
2- Begin the fetch instruments by configuration
     -> Given the configuration is correct
     -> When I start the sservice
     -> Then I fetch all instruments

3- Do the marketdata puller
4- Optimize the code
5- begin the datastorage


# Issues 

> [!CAUTION]
> Bad Request get instrument

info: System.Net.Http.HttpClient.Default.LogicalHandler[100]
      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
      Start processing HTTP request GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
info: System.Net.Http.HttpClient.Default.ClientHandler[100]
      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
      Sending HTTP request GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
info: System.Net.Http.HttpClient.Default.ClientHandler[101]
      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
      Received HTTP response after 196.2512ms - BadRequest
info: System.Net.Http.HttpClient.Default.LogicalHandler[101]
      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
      End processing HTTP request after 205.2525ms - BadRequest

but `curl https://test.deribit.com/api/v2/public/get_instruments?currency=ETH&kind=Option&expired=False` works

It is JSON Rpc protocol with recall async the client. Need to use JSON Rpc library

[https://www.tpeczek.com/2020/06/json-rpc-in-aspnet-core-with.html](https://www.tpeczek.com/2020/06/json-rpc-in-aspnet-core-with.html)

Deribit does not respect the JSON-RPC over HTTP convention
[https://www.jsonrpc.org/historical/json-rpc-over-http.html](https://www.jsonrpc.org/historical/json-rpc-over-http.html)

Deribit repo
[https://github.com/deribit/deribit-api-clients/tree/master/csharp](https://github.com/deribit/deribit-api-clients/tree/master/csharp)
> [!TIP]
> I success to use the api deribit, after change .net v4.5 to dotnetcore 3.1 and include the project.
> They do not provide nuget package.
> // Best pratice -> fork the project, include it in the CI/CD system and provide an local artifact NUGET by the internal repo 

