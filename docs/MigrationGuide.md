There are a decent amount of breaking changes when moving from version 6.x.x to version 7.x.x. Although the interface has changed, the available endpoints/information have not, so there should be no need to completely rewrite your program.
Most endpoints are now available under a slightly different name or path, and most data models have remained the same, barring a few renames.
In this document most changes will be described. If you have any other questions or issues when updating, feel free to open an issue.

Changes related to `IExchangeClient`, options and client structure are also (partially) covered in the [CryptoExchange.Net Migration Guide](https://github.com/JKorf/CryptoExchange.Net/wiki/Migration-Guide)

### Namespaces
There are a few namespace changes:  
|Type|Old|New|
|----|---|---|
|Enums|`Bittrex.Net.Objects`|`Bittrex.Net.Enums`  |
|Clients|`Bittrex.Net`|`Bittrex.Net.Clients`  |
|Client interfaces|`Bittrex.Net.Interfaces`|`Bittrex.Net.Interfaces.Clients`  |
|Objects|`Bittrex.Net.Objects`|`Bittrex.Net.Objects.Models`  |
|SymbolOrderBook|`Bittrex.Net`|`Bittrex.Net.SymbolOrderBooks`|

### Client options
The `BaseAddress` and rate limiting options are now under the `SpotApiOptions`.  
*V6*
````C#
var bittrexClient = new BittrexClient(new BittrexClientOptions
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	BaseAddress = "ADDRESS",
	RateLimitingBehaviour = RateLimitingBehaviour.Fail
});
````

*V7*
````C#
var bittrexClient = new BittrexClient(new BittrexClientOptions
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	SpotApiOptions = new RestApiClientOptions
	{
		BaseAddress = "ADDRESS",
		RateLimitingBehaviour = RateLimitingBehaviour.Fail
	}
});
````

### Client structure
Version 7 adds the `SpotApi` Api client under the `BittrexClient`, and a topic underneath that. This is done to keep the same client structure as other exchange implementations, more info on this [here](https://github.com/Jkorf/CryptoExchange.Net/wiki/Clients).
In the BittrexSocketClient a `SpotStreams` Api client is added. This means all calls will have changed, though most will only need to add `SpotApi.[Topic].XXX`/`SpotStreams.XXX`:

*V6*
````C#
var balances = await bittrexClient.GetBalancesAsync();
var withdrawals = await bittrexClient.GetClosedWithdrawalsAsync();

var tickers = await bittrexClient.GetTickersAsync();
var symbols = await bittrexClient.GetSymbolsAsync();

var order = await bittrexClient.PlaceOrderAsync();
var trades = await bittrexClient.GetUserTradesAsync();

var sub = bittrexSocketClient.SubscribeToTickerUpdatesAsync(DataHandler);
````

*V7*  
````C#
var balances = await bittrexClient.SpotApi.Account.GetBalancesAsync();
var withdrawals = await bittrexClient.SpotApi.Account.GetClosedWithdrawalsAsync();

var tickers = await bittrexClient.SpotApi.ExchangeData.GetTickersAsync();
var symbols = await bittrexClient.SpotApi.ExchangeData.GetSymbolsAsync();

var order = await bittrexClient.SpotApi.Trading.PlaceOrderAsync();
var trades = await bittrexClient.SpotApi.Trading.GetUserTradesAsync();

var sub = bittrexSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(DataHandler);
````

### Definitions
Some names have been changed to a common definition. This includes where the name is part of a bigger name  
|Old|New||
|----|---|---|
|`Currency`|`Asset`|`GetCurrenciesAsync` -> `GetAssetsAsync`|
|`Cancelled`|`Canceled`||
|`StartAt/Open/High/Low/Close`|`OpenTime/OpenPrice/HighPrice/LowPrice/ClosePrice`||
|`AskRate/BidRate`|`BestAskPrice/BestBidPrice`||
|`Direction`|`Side`||
|`Commission`|`Fee`||

Some names have slightly changed to be consistent across different libraries   
`FillQuantity` -> `QuantityFilled`  

### BittrexSymbolOrderBook
The constructor for the `BittrexSymbolOrderBook` no longer requires a `limit` parameter. It is still available to set via the `BittrexOrderBookOptions`. If not set in the options the default value will be `25`.  
*V6*
````C#
var book = new BittrexSymbolOrderBook("BTC-USDT", 25);
````

*V7*
````C#
var book = new BittrexSymbolOrderBook("BTC-USDT", new BittrexOrderBookOptions {
	Limit = 25,
});
````

### Changed methods
The Bittrex API uses the term `ceiling order` to reference orders where the quantity is specified in the quote asset instead of the base asset. This has been changed to make this consistent with other CryptoExchange.Net implementations, and you can now specify the quoteQuantity in an order to use the ceiling functionality:  
*V6*
````C#
bittrexClient.SpotApi.Trading.PlaceOrderAsync(
	"BTC-USDT",
	OrderSide.Buy,
	OrderType.MarketCeiling,
	TimeInForce.FillOrKill,
	ceiling: 50);
````

*V7*
````C#
bittrexClient.SpotApi.Trading.PlaceOrderAsync(
	"BTC-USDT",
	OrderSide.Buy,
	OrderType.Market,
	TimeInForce.FillOrKill,
	quoteQuantity: 50);
````
