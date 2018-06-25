# ![Icon](https://github.com/JKorf/Bittrex.Net/blob/master/Resources/icon.png?raw=true) Bittrex.Net 

![Build status](https://travis-ci.org/JKorf/Bittrex.Net.svg?branch=master)

Bittrex.Net is a .Net wrapper for the Bittrex API as described on [Bittrex](https://bittrex.com/Home/Api). It includes all features the API provides using clear and readable C# objects including 
* Reading market info
* Placing and managing orders
* Reading balances and funds

Next to that it adds some convenience features like:
* Access to the SignalR websocket, allowing for realtime updates
* Configurable rate limiting
* Autmatic logging

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Bittrex.Net/issues)**

---
Also check out my other exchange API wrappers:
<table>
<tr>
<td><img src="https://github.com/JKorf/Binance.Net/blob/master/Resources/binance-coin.png?raw=true">
<br />
<a href="https://github.com/JKorf/Binance.Net">Binance</a>
</td>
<td><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Resources/icon.png?raw=true">
<br />
<a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a></td>
</table>


## Installation
![Nuget version](https://img.shields.io/nuget/v/bittrex.net.svg) ![Nuget downloads](https://img.shields.io/nuget/dt/Bittrex.Net.svg)

Available on [NuGet](https://www.nuget.org/packages/Bittrex.Net/):
```
PM> Install-Package Bittrex.Net
```
To get started with Bittrex.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/Bittrex.Net/). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'Bittrex.Net' and hit enter. The Bittrex.Net package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use Bittrex.Net in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that Bittrex.Net will be installed in. After selecting the correct project type  `Install-Package Bittrex.Net`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using Bittrex.Net.

## Getting started
To get started we have to add the Bittrex.Net namespace:  `using Bittrex.Net;`.

Bittrex.Net provides two clients to interact with the Bittrex API. The  `BittrexClient`  provides all rest API calls. The  `BittrexSocketClient`  provides functions to interact with the SignalR websocket provided by the Bittrex API. Both clients are disposable and as such can be used in a  `using`statement.

Most API methods are available in two flavors, sync and async:
````C#
public void NonAsyncMethod()
{
    using(var client = new BittrexClient())
    {
        var result = client.GetTicker("BTC-ETH");
    }
}

public async Task AsyncMethod()
{
    using(var client = new BittrexClient())
    {
        var result2 = await client.GetTickerAsync("BTC-ETH");
    }
}
````

## Response handling
All API requests will respond with a CallResult object. This object contains whether the call was successful, the data returned from the call and an error if the call wasn't successful. As such, one should always check the Success flag when processing a response.
For example:
````C#
using(var client = new BittrexClient())
{
	var priceResult = client.GetTicker("BTC-ETH");
	if (priceResult.Success)
		Console.WriteLine($"BTC-ETH price: {priceResult.Data.Last}");
	else
		Console.WriteLine($"Error: {priceResult.Error.Message}");
}
````

## Options & Authentication
The default behavior of the clients can be changed by providing options to the constructor, or using the `SetDefaultOptions` before creating a new client. Api credentials can be provided in these options.

## Websockets
The Bittrex API exposes a SignalR Websocket connection for realtime updates. The websocket provides updates regarding the latest prices and trades of all markets, as well as updates for orders and balances for users. Listening to the websocket is easier to implement and less demanding of the Bittrex server than polling using the Rest API.

#### Subscribing
To subscribe to a socket the `SubscribeXXX` methods on the `BittrexSocketClient` can be used:
````C#
    var socketClient = new BittrexSocketClient();
	var subcribtionSuccess = socketClient.SubscribeToMarketSummariesUpdate("BTC-ETH", data =>
	{
		// Handle data
	});
````

#### Unsubscribing
To unsubscribe from the socket the `UnsubscribeFromStream` method in combination with the stream ID received from subscribing can be used. Alternatively, all subscriptions can be unsubscribed on a client using the `UnsubscribeAllStreams` method:
````C#
    var socketClient = new BittrexSocketClient();
	var subcribtionSuccess = socketClient.SubscribeToMarketSummariesUpdate("BTC-ETH", data =>
	{
		// Handle data
	});
	
	socketClient.UnsubscribeFromStream(subcribtionSuccess.Data); // Unsubscribes a single sub
    socketClient.UnsubscribeAllStreams(); // Unsubscribes all subs on this client 
}
````

#### Connection events
If the connection gets lost when it was connected Bittrex.Net will automatically try to reconnect to the websocket. So when the computer that runs the code loses the internet connection or the Bittrex service fails it will keep retrying to connect to the service as long as there are still subscriptions on any client. To be notified of when this happens there are 2 events to which can be listened, the ConnectionLost and ConnectionRestored events. Note that these are on class events, not instance events. This is because internally there is only a single websocket shared over all clients.
````C#
    var socketClient = new BittrexSocketClient();
    socketClient.ConnectionLost += () => 
    {
        Console.WriteLine("Connection lost!");
    };

    socketClient.ConnectionRestored += () => 
    {
        Console.WriteLine("Connection restored after being lost!");
    };
````

## Release notes
* Version 2.1.9 - 25 jun 2018
	* Updated base rest api address

* Version 2.1.8 - 08 jun 2018
	* Fix for broken datettime parsing

* Version 2.1.7 - 08 jun 2018
	* Fix DateTime objects to have Kind set to Utc

* Version 2.1.6 - 04 jun 2018
	* Added additional events for socket connection
	* Fix for null reference in QueryExchangeState

* Version 2.1.5 - 09 may 2018
	* Added support for multiple accounts in socket client

* Version 2.1.4 - 07 may 2018
	* Fix for logging issue

* Version 2.1.3 - 07 may 2018
	* Moved from beta endpoint to release endpoint
	* Updated CryptoExchange.Net base

* Version 2.1.2 - 17 apr 2018
	* Fix for failed resubscribe handling after connection is restored
	* Additional error checking on socket data
	
* Version 2.1.1 - 06 apr 2018
	* Fixed reconnect when connection is closed without error
	* More error checking in socket client

* Version 2.1.0 - 05 apr 2018
	* Updated to new Bittrex beta socket implementation
	* Cleaned reconnection logic
	
* Version 2.0.10 - 23 mar 2018

	* Fix for CloudFlare bypass
	* Updated socket dispose
	* Updated base

* Version 2.0.9 - 21 mar 2018
	* Allow multiple log writers

* Version 2.0.8 - 12 mar 2018
	* Updated packages to fix freezing when called from UI thread

* Version 2.0.7 - 09 mar 2018
	* Updated base

* Version 2.0.6 - 09 mar 2018
	* Fix for exception when parsing PlaceConditionalOrder result
	* Fix for UI thread freezing

* Version 2.0.5 - 08 mar 2018
	* Cleaned socket implementation

* Version 2.0.4 - 08 mar 2018
	* Removed initial try without CloudFlare cookies in socket client
	* Cleanup, removed unused files

* Version 2.0.2/2.0.3 - 05 mar 2018
	* Fix for freezes when calling from UI thread

* Version 2.0.1 - 03 mar 2018
	* Fix for bug in URL building

* Version 2.0.0 - 01 mar 2018
	* Updated to use a base package, which introduces some changes in syntax, but keeps functionality unchanged
	
* Version 1.4.1 - 26 feb 2018
	* Small fixes
	
* Version 1.4.0 - 26 feb 2018
	* Changed how to set (default) options
	* Added SetProxy to REST client
	* Fix for OrderSide being default in some returns
	
* Version 1.3.15 - 21 feb 2018
	* Fix for wrong orderside in socket exchange delta event 
	
* Version 1.3.14 - 21 feb 2018
	* Fix for Trade updates from websocket not containing Price/Rate

* Version 1.3.13 - 20 feb 2018 
	* Fix for ExchangeDeltas update for a market being called for all subscriptions

* Version 1.3.12 - 20 feb 2018 
	* Added V2 order endpoint to support conditional orders
	* Combined BittrexExchangeState and BittrexStreamExchangeState

* Version 1.3.11 - 15 feb 2018
	* Fix for freezes when making calls from UI thread

* Version 1.3.10 - 15 feb 2018
	* Fix for MarketName being Null in QueryExchangeState
	* Cleaned BittrexSocketClient and ExchangeState objects
	* Fix for error messages being duplicated

* Version 1.3.9 - 07 feb 2018
	* Fix for deadlock if certain methods were called from the UI thread

* Version 1.3.8 - 31 jan 2018
	* Added Notice field to BittrexCurrency response
	* Added Notice, LogoUrl and IsSponsored fields to BittrexMarketSummary response, removed DisplayMarketName which is no longer returned (was always null)
	* Changed BittrexMarketSummary fields to nullable decimals since they can be null for new markets

* Version 1.3.7 - 17 jan 2018
	* Updated CloudFlareUtilities package to fix socket connection

* Version 1.3.6 - 12 jan 2018
	* Added missing merge

* Version 1.3.5 - 12 jan 2018
	* Added orderbook websocket endpoints
	* Added basic proxy functionality
	* Small fixed in order json parsing
	* Fix for nullreference when theres no connection

* Version 1.3.4 - 04 jan 2018
	* Fix for websockets for all platforms

* Version 1.3.3 - 02 jan 2018
	* Fix for stream connections on platforms with Websocket protocol supported
	* Parallel foreach for stream event callbacks to improve performance

* Version 1.3.2 - 11 dec 2017
	* Added baseUrl parameter to constructor for mocking
	* Added IBittrexClient interface for mocking

* Version 1.3.1 - 29 nov 2017
	* Added candle endpoints
	* Made ExecuteRequest and GetUrl protected so they can be overridden
	* Fixed some small potential threading problems
	
* Version 1.3.0 - 24 nov 2017
	* Changed websocket implementation to support new Bittrex socket implementation on all platforms
	* Added SubscribeToMarketDeltaStreamAsync which returns a Task

* Version 1.2.2 - 20 nov 2017
	* Temporary fix for `BittrexSocketClient` connection after changes on the Bittrex socket API

* Version 1.2.1 - 15 nov 2017
	* CloudFlare is now used as backup, it'll first try without. This should improve connection time when CloudFlare isn't enabled

* Version 1.2.0 - 13 nov 2017
	* **NETStandard2.0 supported now**
	* Extended CloudFlare bypass for more platforms

* Version 1.1.3 - 13 nov 2017
	* Added CloudFlare bypass in socket client

* Version 1.1.2 - 10 nov 2017
	* Added ratelimiting options
	* Added connection lost/restored events in socket client
	* Added log verbosity None to be able to mute all logging
	* Added encryptor to dispose

* Version 1.1.1 - 9 nov 2017
	* Fix for error in wrong category
	* Small code documentation fixes

* Version 1.1.0 - 9 nov 2017
	* Added automatic configurable retry on server errors
	* Refactor on error returns

* Version 1.0.1 - 8 nov 2017
	* Added reconnect functionality in socket client as long as there are still subscriptions open

* Version 1.0.0 - 6 nov 2017
	* Release version 1.0.0
	* Additional unit tests, also for the socket client	
	* Small refactoring for unit testability
	* Small cleanup
	
* Version 0.0.4 - 4 nov 2017
	* Added icon

* Version 0.0.3 - 1 nov 2017
	* Small naming changes in socket client
	* Added api key checks in private endpoints
	* Updated documentation
	* Added example project

* Version 0.0.2 - 1 nov 2017
	* Added BittrexSocketClient
	* Updated documentation
