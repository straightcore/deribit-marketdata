# Deribit Marketdata Adapter project

# Exercice
 
***Guidelines:***

This exercise is intended to be straight forward, but you may encounter some questions or issues - please work through these using your best judgement and document any assumptions/decisions you take (comments inside of the code are fine for that).

In an effort to have something complete for #1, please document (code comments are fine for that as well) any design decisions you take that you may have done differently with more time.

***Coding Exercise:***

Using C# and .netcore, implement a market data recorder for Deribit instruments and expose that market data for client consumption.

1. Using either Websockets or Rest (your choice), fetch the latest price per instrument every N seconds while respecting the rate limit. Persist the retrieved data into the data store of your choice (text file, database, etc).

2. Time permitting, create a service for clients to retrieve the pricing data you've persisted above.

 

> [!NOTE]
> Deribit API References:
>
>* https://docs.deribit.com/v2/#deribit-api-v2-0-0
>
>* https://docs.deribit.com/v2/#public-get_instruments
>
>* https://docs.deribit.com/v2/#public-get_last_trades_by_instrument


# Methodology
 
In the best, I use the S.O.L.I.D. implementation, to have the capability to test my code with BDD and TDD with Red, Green, Refactoring methologies.

I use external dependencies to resolve my implementation.

This project should be used in CI/CD workflow. 

# Technologies

- dotnet core 3.1 
- JSON-RPC ApI by OpenAPI, I rework it and rebuild in dotnet core 3.1. 
- SQL Lite, to store the market data. This choice is very simple, I need to show you that it works !
    > [!NOTE]- For more performance / scalability, I use [mongodb with timeseries](https://www.mongodb.com/blog/post/schema-design-for-time-series-data-in-mongodb#:~:text=%20Schema%20Design%20for%20Time%20Series%20Data%20in,series%20data%20in%20a%20database%3F%20In...%20More%20) 

# In reality
- I don't finish the code in TDD / BDD, I chose between delivery and expose my capability to delivery my code with the best pratices applied.
- I don't use the websocket, I never used the JSON-RPC and I failed to execute a query with "WebSocket" api and I find the API by OpenAPI for deribit. I favored delivery.

- EntityFramework
    - dotnet tool install --global dotnet-ef
    - dotnet add package Microsoft.EntityFrameworkCore.Design
    - dotnet ef migrations add InitialCreate
    - dotnet ef database update

> # TODO:
> [!WARNING]
>1- Use credential to connect by websocket // awaiting receive email to confirm my account
>
>2- Begin the fetch instruments by configuration
>
>    - Given the configuration is correct
>    - When I start the sservice
>    - Then I fetch all instruments
>
>3- Do the marketdata puller
>
>4- Optimize the code
>
>5- begin the datastorage

> [!CAUTION]
># Issues 
> Bad Request get instrument
>
>info: System.Net.Http.HttpClient.Default.LogicalHandler[100]
>      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>      Start processing HTTP request GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>info: System.Net.Http.HttpClient.Default.ClientHandler[100]
>      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>      Sending HTTP request GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>info: System.Net.Http.HttpClient.Default.ClientHandler[101]
>      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>      Received HTTP response after 196.2512ms - BadRequest
>info: System.Net.Http.HttpClient.Default.LogicalHandler[101]
>      => HTTP GET https://test.deribit.com/api/v2/public/get_instruments?currency=BTC&kind=Future&expired=False
>      End processing HTTP request after 205.2525ms - BadRequest

>but `curl https://test.deribit.com/api/v2/public/get_instruments?currency=ETH&kind=Option&expired=False` works

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

# Cron every seconds
I used the Quartz, it is easy and flexible.
[https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/using-quartz.html](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/using-quartz.html)

I did not take the decision for HFT context. Just try to ask the problem with "Simple", "Flexibale" and "adaptable" source code.
The time not permitting to abstract all framework in my implementation. But the best way for the UnitTest and the Specflow test is to abstract the QuartZ Framework. 
If in the future we need to change the framework by another or inhouse scheduler high frequency (dispatcher), we just need to change the implementation and inject by the IOC in the startup.cs 