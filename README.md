# Bittrex.Net
![Build status](https://travis-ci.com/JKorf/Bittrex.Net.svg?branch=master) ![Nuget version](https://img.shields.io/nuget/v/Bittrex.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/Bittrex.Net.svg)

Bittrex.Net is a wrapper around the Bittrex API as described on [Bittrex](https://bittrex.com/Home/Api), including all features the API provides using clear and readable objects, both for the REST  as the websocket API's.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Bittrex.Net/issues)**

[Documentation](https://jkorf.github.io/Bittrex.Net/)

## Donations
I develop and maintain this package on my own for free in my spare time. Donations are greatly appreciated. If you prefer to donate any other currency please contact me.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  

## Discord
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). Feel free to join for discussion and/or questions around the CryptoExchange.Net and implementation libraries.

## Release notes
* Version 7.0.0-beta3 - 31 Jan 2022
    * Updated CryptoExchange.Net

* Version 7.0.0-beta2 - 24 Jan 2022
    * Updated CryptoExchange.Net

* Version 7.0.0-beta1 - 15 Jan 2022
    * Updated CryptoExchange.Net

* Version 7.0.0-alpha6 - 07 Jan 2022
    * Updated CrytpoExchange.Net
    * Added optional parameter for socket client lifetime in AddBittrex

* Version 7.0.0-alpha5 - 03 Jan 2022
    * Updated CryptoExchange.Net

* Version 7.0.0-alpha4 - 01 Jan 2022
    * New comon implementation, added AddBittrex extension method

* Version 7.0.0-alpha3 - 27 Dec 2021
    * Updated CryptoExchange.Net

* Version 7.0.0-alpha2 - 21 Dec 2021
    * Update to new CryptoExchange.Net version

* Version 7.0.0-alpha1 - 07 Dec 2021
    * Initial version new CryptoExchange.Net. More documentation coming soon

* Version 6.1.5 - 08 Oct 2021
    * Updated CryptoExchange.Net to fix some socket issues

* Version 6.1.4 - 06 Oct 2021
    * Updated CryptoExchange.Net, fixing socket issue when calling from .Net Framework

* Version 6.1.3 - 05 Oct 2021
    * Updated CryptoExchange.Net

* Version 6.1.2 - 29 Sep 2021
    * Updated CryptoExchange.Net

* Version 6.1.1 - 23 Sep 2021
    * Fixed exception for post requests without parameters

* Version 6.1.0 - 20 Sep 2021
    * Added missing SetApiCredentials method
    * Updated CryptoExchange.Net

* Version 6.0.6 - 15 Sep 2021
    * Updated CryptoExchange.Net

* Version 6.0.5 - 02 Sep 2021
    * Fix for disposing order book closing socket even if there are other connections

* Version 6.0.4 - 26 Aug 2021
    * Updated CryptoExchange.Net

* Version 6.0.3 - 24 Aug 2021
    * Updated CryptoExchange.Net, improving websocket and SymbolOrderBook performance

* Version 6.0.2 - 19 Aug 2021
    * Added GetTradingFeesAsync endpoint

* Version 6.0.1 - 13 Aug 2021
    * Fix for OperationCancelledException being thrown when closing a socket from a .net framework project
    * Fixed unsubscribing not working

* Version 4.0.0 - 12 Aug 2021
	* Release version with new CryptoExchange.Net version 4.0.0
		* Multiple changes regarding logging and socket connection, see [CryptoExchange.Net release notes](https://github.com/JKorf/CryptoExchange.Net#release-notes)

* Version 6.0.0-beta3 - 09 Aug 2021
    * Renamed GetSymbolTradesAsync to GetTradeHistoryAsync
    * Renamed GetExecutionsAsync to GetUserTradesAsync
    * Renamed GetOrderExecutionsAsync to GetOrderTradesAsync
    * Renamed SubscribeToSymbolTradeUpdatesAsync to SubscribeToTradeUpdatesAsync
    * Renamed SubscribeToExecutionUpdatesAsync to SubscribeToUserTradeUpdatesAsync

* Version 6.0.0-beta2 - 26 Jul 2021
    * Updated CryptoExchange.Net

* Version 6.0.0-beta1 - 09 Jul 2021
    * Added Async postfix for async methods
    * Updated CryptoExchange.Net

* Version 5.2.0-beta5 - 07 Jun 2021
    * Fixed BittrexSocketClient Proxy option
    * Updated SignalR client package version
    * Updated CryptoExchange.Net

* Version 5.2.0-beta4 - 27 May 2021
    * Fix for stream data deserialization

* Version 5.2.0-beta3 - 26 May 2021
    * Removed non-async calls
    * Updated to CryptoExchange.Net changes

* Version 5.2.0-beta2 - 06 mei 2021
    * Updated CryptoExchange.Net

* Version 5.2.0-beta1 - 30 apr 2021
    * Updated to CryptoExchange.Net 4.0.0-beta1, new websocket implementation

* Version 5.1.1 - 04 mei 2021
    * Allow TimeInForce null in BittrexOrder result
    * Added GetExecutionById endpoint

* Version 5.1.0 - 28 apr 2021
    * Added batch order placement/cancellation

* Version 5.0.3 - 19 apr 2021
    * Updated CryptoExchange.Net

* Version 5.0.2 - 30 mrt 2021
    * Updated CryptoExchange.Net

* Version 5.0.1 - 15 mrt 2021
    * Fixed socket client authentication

* Version 5.0.0 - 11 mrt 2021
    * Dropped support for V1 API, removed V3 post fixes
    * Added permission endpoints
    * Added executions endpoint
    * Added KlineType to kline endpoints
    * Added execution subscription on socket client

* Version 4.3.1 - 01 mrt 2021
    * Added Nuget SymbolPackage

* Version 4.3.0 - 01 mrt 2021
    * Added config for deterministic build
    * Updated CryptoExchange.Net

* Version 4.2.2 - 22 jan 2021
    * Updated for ICommonKline

* Version 4.2.1 - 14 jan 2021
    * Updated CryptoExchange.Net

* Version 4.2.0 - 21 dec 2020
    * Fix for pageSizes being limited to 100 while max is 200
    * Added SubscribeToHeartbeatAsync on BittrexSocketClientV3
    * Updated CryptoExchange.Net
    * Updated to latest IExchangeClient

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
