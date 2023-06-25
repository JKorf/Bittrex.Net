---
title: Getting started
nav_order: 2
---

## Creating client
There are 2 clients available to interact with the Bittrex API, the `BittrexRestClient` and `BittrexSocketClient`. They can be created manually on the fly or be added to the dotnet DI using the `AddBittrex` extension method.

*Manually create a new client*
```csharp
var bittrexRestClient = new BittrexRestClient(options =>
{
    // Set options here for this client
});

var bittrexSocketClient = new BittrexSocketClient(options =>
{
    // Set options here for this client
});
```

*Using dotnet dependency inject*
```csharp
services.AddBittrex(
    restOptions => {
        // set options for the rest client
    },
    socketClientOptions => {
        // set options for the socket client
    }); 
    
// IBittrexRestClient, IBittrexSocketClient and IBittrexOrderBookFactory are now available for injecting
```

Different options are available to set on the clients, see this example
```csharp
var bittrexRestClient = new BittrexRestClient(options =>
{
    options.ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET");
    options.RequestTimeout = TimeSpan.FromSeconds(60);
});
```
Alternatively, options can be provided before creating clients by using `SetDefaultOptions` or during the registration in the DI container: 
```csharp
BittrexRestClient.SetDefaultOptions(options => {
    // Set options here for all new clients
});
var bittrexRestClient = new BittrexRestClient();
```
More info on the specific options can be found in the [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Options.html)

### Dependency injection
See [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Dependency%20Injection.html)

