# ![Icon](https://github.com/JKorf/Bittrex.Net/blob/master/Bittrex.Net/Icon/icon.png?raw=true) Bittrex.Net 

![Build status](https://travis-ci.org/JKorf/Bittrex.Net.svg?branch=master)

Bittrex.Net is a .Net wrapper for the Bittrex API as described on [Bittrex](https://bittrex.com/Home/Api). It includes all features the API provides using clear and readable C# objects including 
* Reading market info
* Placing and managing orders
* Reading balances and funds

Additionally it adds some convenience features like:
* Access to the SignalR websocket, allowing for realtime updates
* Configurable rate limiting
* Autmatic logging

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Bittrex.Net/issues)**

## CryptoExchange.Net
Implementation is build upon the CryptoExchange.Net library, make sure to also check out the documentation on that: [docs](https://github.com/JKorf/CryptoExchange.Net)

Other CryptoExchange.Net implementations:
<table>
<tr>
<td><a href="https://github.com/JKorf/Bitfinex.Net"><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Bitfinex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a>
</td>
<td><a href="https://github.com/JKorf/Binance.Net"><img src="https://github.com/JKorf/Binance.Net/blob/master/Binance.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Binance.Net">Binance</a>
</td>
<td><a href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/blob/master/CoinEx.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/CoinEx.Net">CoinEx</a>
</td>
<td><a href="https://github.com/JKorf/Huobi.Net"><img src="https://github.com/JKorf/Huobi.Net/blob/master/Huobi.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Huobi.Net">Huobi</a>
</td>
<td><a href="https://github.com/JKorf/Kucoin.Net"><img src="https://github.com/JKorf/Kucoin.Net/blob/master/Kucoin.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Kucoin.Net">Kucoin</a>
</td>
<td><a href="https://github.com/JKorf/Kraken.Net"><img src="https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Kraken.Net">Kraken</a>
</td>
</tr>
</table>
Implementations from third parties:
<table>
	<tr>
		<td>
			<a href="https://github.com/Zaliro/Switcheo.Net">
				<img src="https://github.com/Zaliro/Switcheo.Net/blob/master/Resources/switcheo-coin.png?raw=true">
			</a>
			<br />
			<a href="https://github.com/Zaliro/Switcheo.Net">Switcheo</a>
		</td>
		<td>
			<a href="https://github.com/ridicoulous/LiquidQuoine.Net">
				<img src="https://github.com/ridicoulous/LiquidQuoine.Net/blob/master/Resources/icon.png?raw=true">
			</a>
			<br />
			<a href="https://github.com/ridicoulous/LiquidQuoine.Net">Liquid</a>
		</td>
		<td><a href="https://github.com/burakoner/OKEx.Net"><img src="https://raw.githubusercontent.com/burakoner/OKEx.Net/master/Okex.Net/Icon/icon.png"></a>
		<br />
		<a href="https://github.com/burakoner/OKEx.Net">OKEx</a>
		</td>
		</td>
	<td><a href="https://github.com/ridicoulous/Bitmex.Net"><img src="https://github.com/ridicoulous/Bitmex.Net/blob/master/Bitmex.Net/Icon/icon.png"></a>
<br />
<a href="https://github.com/ridicoulous/Bitmex.Net">Bitmex</a>
</td>
<td><a href="https://github.com/intelligences/HitBTC.Net"><img src="https://github.com/intelligences/HitBTC.Net/blob/master/src/HitBTC.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/intelligences/HitBTC.Net">HitBTC</a>
</td>
	</tr>
	
</table>

## Donations
Donations are greatly appreciated and a motivation to keep improving.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  


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

Bittrex.Net provides three clients to interact with the Bittrex API. The  `BittrexClient`  provides all V1.1 rest API calls, whereas the `BittrexClientV3` gives access to the V3 rest API calls. The  `BittrexSocketClient`  provides functions to interact with the SignalR websocket provided by the Bittrex API. Both clients are disposable and as such can be used in a  `using`statement.

## Release notes
* Version 4.1.9 - 11 dec 2020
    * Updated CryptoExchange.Net
    * Implemented IExchangeClient

* Version 4.1.8 - 19 nov 2020
    * Added conditional order subscription to socket client
    * Changed operand on conditional order to an enum
    * Added clientWithdrawalId to withdraw methods and models
    * Updated v3 market/currency models

* Version 4.1.7 - 08 Oct 2020
    * Fix for double events for kline subscriptions
    * Updated CryptoExchange.Net

* Version 4.1.6 - 03 Sep 2020
    * Fixed V3 GetOrderBook limit parameter serialization

* Version 4.1.5 - 31 Aug 2020
    * Added GetSequence extension method documentation
    * Added GetOrderBook limit parameter

* Version 4.1.4 - 28 Aug 2020
    * Updated CryptoExchange
    * Updated BittrexSymbolOrderBook to V3

* Version 4.1.3 - 12 Aug 2020
    * Updated CryptoExchange.Net

* Version 4.1.2 - 05 Aug 2020
    * Fixed GetHistoricalKlines date parameters
    * Added overloads on the V3 socket client to subscribe multiple symbols at once

* Version 4.1.1 - 20 Jul 2020
    * Fixed reference

* Version 4.1.0 - 20 Jul 2020
    * Updated V3 rest client
    * Added V3 socket client

* Version 4.0.10 - 21 Jun 2020
    * Updated CryptoExchange

* Version 4.0.9 - 16 Jun 2020
    * Updated CryptoExchange.Net

* Version 4.0.8 - 074 Jun 2020
    * Updated CryptoExchange

* Version 4.0.7 - 03 Mar 2020
    * Updated CryptoExchange

* Version 4.0.6 - 27 Jan 2020
    * Updated CryptoExchange.Net

* Version 4.0.5 - 14 Nov 2019
    * Fixed NotImplementedException sometimes being triggered when reconnecting

* Version 4.0.4 - 12 Nov 2019
    * Added retry on SignalR hub invoke for websockets

* Version 4.0.3 - 27 oct 2019
	* Fixed GetOrderBook in socket client

* Version 4.0.2 - 23 Oct 2019
	* Fixed summary symbol mapping

* Version 4.0.1 - 23 Oct 2019
	* Fixed symbol validation

* Version 4.0.0 - 23 Oct 2019
	* See CryptoExchange.Net 3.0 release notes
	* Added input validation
	* Added CancellationToken support to all requests
	* Now using IEnumerable<> for collections
	* Renamed Candle -> Kline
	* Renamed Market -> Symbol
	* Renamed ExchangeState -> OrderBook

* Version 3.1.8 - 06 Oct 2019
    * Changed ask/bid in BittrexStreamMarketSummary to be nullable

* Version 3.1.7 - 15 Aug 2019
    * Updated code docs
    * Updated V3 client

* Version 3.1.6 - 07 Aug 2019
    * Updated CryptoExchange.Net

* Version 3.1.5 - 05 Aug 2019
    * Added xml file for code documentation

* Version 3.1.4 - 09 jul 2019
	* Updated BittrexSymbolOrderBook

* Version 3.1.3 - 14 may 2019
	* Added an order book implementation for easily keeping an updated order book
	* Added additional constructor to ApiCredentials to be able to read from file

* Version 3.1.2 - 06 may 2019
	* Fixed limit being a nullable field on orders

* Version 3.1.1 - 06 may 2019
	* Fixed market order type parsing
	* Added support for the V3 open beta API

* Version 3.1.0 - 01 may 2019
	* Updated to latest CryptoExchange.Net
		* Adds response header to REST call result
		* Added rate limiter per API key
		* Unified socket client workings

* Version 3.0.9 - 09 mar 2019
	* Fixed stream order condition parsing

* Version 3.0.8 - 08 mar 2019
	* Fixed Bid/Ask being nullable in market summary

* Version 3.0.7 - 07 mar 2019
	* Fixed parsing of ConditionType
	* Fixed linq exception on Query in socket client

* Version 3.0.6 - 01 mar 2019
	* Fixed nullable fields in orders
	* CallResult to WebCallResult for BittrexClient

* Version 3.0.5 - 01 feb 2019
	* Updated CryptoExchange.Net

* Version 3.0.4 - 11 jan 2019
	* Updated CryptoExchange.Net

* Version 3.0.3 - 29 dec 2018
	* Updated CryptoExchange.Net

* Version 3.0.2 - 17 dec 2018
	* Fix for error while reconnecting

* Version 3.0.1 - 06 dec 2018
	 * Fix for socket client
	 * Fix for freezes if called from UI thread

* Version 3.0.0 - 05 dec 2018
	* Updated to CryptoExchange.Net version 2
		* Libraries now use the same standard functionalities
		* Objects returned by socket subscriptions standardized across libraries

* Version 2.1.20 - 05 nov 2018
	* Fix for v2 api methods resulting in INVALID_SIGNATURE

* Version 2.1.19 - 01 nov 2018
	* Fix MarketSummaryLite failing to deserialize when there is a new market without data

* Version 2.1.18 - 25 sep 2018
	* Fixed PlaceConditionalOrder endpoint

* Version 2.1.17 - 21 sep 2018
	* Updated CryptoExchange.Net
	* Fixed Kline methods

* Version 2.1.16 - 21 aug 2018
	* Fix for default api credentials getting disposed

* Version 2.1.15 - 20 aug 2018
	* Updated CryptoExchange.Net for bugfix

* Version 2.1.14 - 16 aug 2018
	* Added interface for socket client
	* Fixed some minor Resharper warnings
	* Updated CryptoExchange.Net

* Version 2.1.13 - 13 aug 2018
	* Fixed auth calls
	
* Version 2.1.12 - 13 aug 2018
	* Updated CryptoExchange.Net

* Version 2.1.11 - 13 aug 2018
	* Updated CryptoExchange.Net

* Version 2.1.10 - 03 jul 2018
	* Small fix socket event binding

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
