## Creating client
There are 2 clients available to interact with the Bittrex API, the `BittrexClient` and `BittrexSocketClient`.

*Create a new rest client*
````C#
var bittrexClient = new BittrexClient(new BittrexClientOptions()
{
	// Set options here for this client
});
````

*Create a new socket client*
````C#
var bittrexSocketClient = new BittrexSocketClient(new BittrexSocketClientOptions()
{
	// Set options here for this client
});
````

Different options are available to set on the clients, see this example
````C#
var bittrexClient = new BittrexClient(new BittrexClientOptions()
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	LogLevel = LogLevel.Trace,
	RequestTimeout = TimeSpan.FromSeconds(60)
});
````
Alternatively, options can be provided before creating clients by using `SetDefaultOptions`:
````C#
BittrexClient.SetDefaultOptions(new BittrexClientOptions{
	// Set options here for all new clients
});
var bittrexClient = new BittrexClient();
````
More info on the specific options can be found on the [CryptoExchange.Net wiki](https://github.com/JKorf/CryptoExchange.Net/wiki/Options)

### Dependency injection
See [CryptoExchange.Net wiki](https://github.com/JKorf/CryptoExchange.Net/wiki/Clients#dependency-injection)

## Usage
Make sure to read the [CryptoExchange.Net wiki](https://github.com/JKorf/CryptoExchange.Net/wiki/Clients#processing-request-responses) on processing responses.

#### Get market data
````C#
// Getting info on all symbols
var symbolData = await bittrexClient.SpotApi.ExchangeData.GetSymbolsAsync();

// Getting tickers for all symbols
var tickerData = await bittrexClient.SpotApi.ExchangeData.GetTickersAsync();

// Getting the order book of a symbol
var orderBookData = await bittrexClient.SpotApi.ExchangeData.GetOrderBookAsync("BTC-USDT");

// Getting recent trades of a symbol
var tradeHistoryData = await bittrexClient.SpotApi.ExchangeData.GetTradeHistoryAsync("BTC-USDT");
````

#### Requesting balances
````C#
var accountData = await bittrexClient.SpotApi.Account.GetBalancesAsync();
````
#### Placing order
````C#
// Placing a buy limit order for 0.001 BTC at a price of 50000USDT each
var orderData = await bittrexClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Buy,
                OrderType.Limit,
                TimeInForce.GoodTillCanceled,
                0.001m,
                50000);
		
// Placing a market buy order for 50USDT
var orderData = await bittrexClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Buy,
                OrderType.Market,
                TimeInForce.FillOrKill,
                quoteQuantity: 50);			
				
													
// Place a stop loss order, place a limit order of 0.001 BTC at 39000USDT each when the last trade price drops below 40000USDT
var orderData = await bittrexClient.SpotApi.Trading.PlaceConditionalOrderAsync(
                "BTC-USDT",
                ConditionalOrderOperand.LesserThan,
                new BittrexUnplacedOrder
                {
                    Price = 39000,
                    Quantity = 0.001m,
                    Side = OrderSide.Sell,
                    Type = OrderType.Limit,
                    Symbol = "BTC-USDT",
                    TimeInForce = TimeInForce.GoodTillCanceled                    
                },
                triggerPrice: 40000);
````

#### Requesting a specific order
````C#
// Request info on order with id `1234`
var orderData = await bittrexClient.SpotApi.Trading.GetOrderAsync("1234");
````

#### Requesting order history
````C#
// Get all orders conform the parameters
 var ordersData = await bittrexClient.SpotApi.Trading.GetClosedOrdersAsync();
````

#### Cancel order
````C#
// Cancel order with id `1234`
var orderData = await bittrexClient.SpotApi.Trading.CancelOrderAsync("1234");
````

#### Get user trades
````C#
var userTradesResult = await bittrexClient.SpotApi.Trading.GetUserTradesAsync();
````

#### Subscribing to market data updates
````C#
var subscribeResult = await bittrexSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(data =>
{
	// Handle ticker data
});
````

#### Subscribing to order updates
````C#
var subscribeResult = await bittrexSocketClient.SpotStreams.SubscribeToOrderUpdatesAsync(data =>
	data =>
	{
	  // Handle order updates
	});
````
