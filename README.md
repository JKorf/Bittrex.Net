# Bittrex.Net

A .Net wrapper for the Bittrex API as described on [Bittrex](https://bittrex.com/Home/Api), including all features.
## Installation
Bittrex.Net is available on [Nuget](https://www.nuget.org/packages/Bittrex.Net/).
```
pm> Install-Package Bittrex.Net
```

## Examples

## Usage
Start using the API by including `using Bittrex.Net;` in your usings.
Bittrex.Net provides two clients to interact with the Bittrex API. The `BittrexClient` provides all rest API calls. The `BittrexSocketClient` provides functions to interact with the SignalR websocket provided by the Bittrex API. Both clients are disposable and as such can be used in a `using` statement:
```C#
using(var client = new BittrexClient())
{
}

using(var client = new BittrexSocketClient())
{
}
```

For most API methods Bittrex.Net provides two versions, synchronized and async calls. 

### Setting API credentials
For private endpoints (trading, order history, account info etc) an API key and secret has to be provided. For this the `SetApiCredentials` method can be used in both clients, or the credentials can be provided as arguments:
```C#
using(var client = new BittrexClient("APIKEY", "APISECRET"))
{
	client.SetApiCredentials("APIKEY", "APISECRET");
}
```
Alternatively the credentials can be set as default in BittrexDefaults to provide them to all new clients.
```C#
BittrexDefaults.SetDefaultApiCredentials("APIKEY", "APISECRET");
```
API credentials can be managed at https://bittrex.com/Manage#sectionApi. Make sure to enable the required permission for the right API calls.

### Response handling
All API requests will respond with an BittrexApiResult object. This object contains whether the call was successful, the data returned from the call and an error message if the call wasn't successful. As such, one should always check the Success flag when processing a response.
For example:
```C#
using(var client = new BittrexClient())
{
	var priceResult = client.GetTicker("BTC-ETH");
	if (priceResult.Success)
		Console.WriteLine($"BTC-ETH price: {priceResult.Last}");
	else
		Console.WriteLine($"Error: {priceResult.Message}");
}
```

### Requests
Public requests:
```C#
var markets = client.GetMarkets();
var currencies = client.GetCurrencies();
var price = client.GetTicker("BTC-ETH");
var marketSummary = client.GetMarketSummary("BTC-ETH");
var marketSummaries = client.GetMarketSummaries();
var orderbook = client.GetOrderBook("BTC-ETH");
var marketHistory = client.GetMarketHistory("BTC-ETH");
```

Private requests:
```C#
var placedOrder = client.PlaceOrder(OrderType.Sell, "BTC-NEO", 1, 1);
var openOrders = client.GetOpenOrders("BTC-NEO");
var orderInfo = client.GetOrder(placedOrder.Result.Uuid);
var canceledOrder = client.CancelOrder(placedOrder.Result.Uuid);
var orderHistory = client.GetOrderHistory("BTC-NEO");

var balance = client.GetBalance("NEO");
var balances = client.GetBalances();
var depositAddress = client.GetDepositAddress("BTC");
var withdraw = client.Withdraw("TEST", 1, "TEST", "TEST");
var withdrawHistory = client.GetWithdrawalHistory();
var depositHistory = client.GetDepositHistory();
```

### Websockets
To be able to keep prices updated in real time the Bittrex API provides a websocket to which can be subscribed. To subscribe to this socket use the `BittrexSocketClient`:
```C#
using(var socketClient = new BittrexSocketClient())
{
	var subcribtionSuccess = socketClient.SubscribeToMarketDelta("BTC-ETH", data =>
	{
		// Handle data
	});
}
```

Unsubscribing from the socket:

To unsubscribe from the socket the `UnsubscribeFromStream` method in combination with the stream ID recieved from subscribing can be used:
```C#
using(var socketClient = new BittrexSocketClient())
{
	var subcribtionSuccess = socketClient.SubscribeToMarketDelta("BTC-ETH", data =>
	{
		// Handle data
	});
	
	socketClient.UnsubscribeFromStream(subcribtionSuccess.Result);
}
```

Alternatively, all sockets can be closed with the `UnsubscribeAllStreams` method. Beware that when a client is disposed the sockets are automatically disposed. This means that if the code is no longer in the using statement the eventhandler won't fire anymore. To prevent this from happening make sure the code doesn't leave the using statement or don't use the socket client in a using statement:
```C#
// Doesn't leave the using block
using(var client = new BittrexSocketClient())
{
	var subcribtionSuccess = socketClient.SubscribeToMarketDelta("BTC-ETH", data =>
	{
		// Handle data
	});

	Console.ReadLine();
}

// Without using block
var client = new BittrexSocketClient();
var subcribtionSuccess = socketClient.SubscribeToMarketDelta("BTC-ETH", data =>
{
	// Handle data
});
```

### Logging
Bittrex.Net will by default log warning and error messages. To change the verbosity `SetLogVerbosity` can be called on a client. The default log verbosity for all new clients can also be set using the `SetDefaultLogVerbosity` in `BittrexDefaults`.

Bittrex.Net logging will default to logging to the Trace (Trace.WriteLine). This can be changed with the `SetLogOutput` method on clients. Alternatively a default output can be set in the `BittrexDefaults` using the `SetDefaultLogOutput` method:
```C#
BittrexDefaults.SetDefaultLogOutput(Console.Out);
BittrexDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);
```


## Release notes
* Version 0.0.3 - 1 nov 2017
	* Small naming changes in socket client
	* Added api key checks in private endpoints
	* Updated documentation
	* Added example project

* Version 0.0.2 - 1 nov 2017
	* Added BittrexSocketClient
	* Updated documentation