---
title: Examples
nav_order: 3
---

## Basic operations
Make sure to read the [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Clients.html#processing-request-responses) on processing responses.

### Get market data
```csharp
// Getting info on all symbols
var symbolData = await bittrexRestClient.SpotApi.ExchangeData.GetSymbolsAsync();

// Getting tickers for all symbols
var tickerData = await bittrexRestClient.SpotApi.ExchangeData.GetTickersAsync();

// Getting the order book of a symbol
var orderBookData = await bittrexRestClient.SpotApi.ExchangeData.GetOrderBookAsync("BTC-USDT");

// Getting recent trades of a symbol
var tradeHistoryData = await bittrexRestClient.SpotApi.ExchangeData.GetTradeHistoryAsync("BTC-USDT");
```

### Requesting balances
```csharp
var accountData = await bittrexRestClient.SpotApi.Account.GetBalancesAsync();
```
### Placing order
```csharp
// Placing a buy limit order for 0.001 BTC at a price of 50000USDT each
var orderData = await bittrexRestClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Buy,
                OrderType.Limit,
                TimeInForce.GoodTillCanceled,
                0.001m,
                50000);
        
// Placing a market buy order for 50USDT
var orderData = await bittrexRestClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Buy,
                OrderType.Market,
                TimeInForce.FillOrKill,
                quoteQuantity: 50);         
                
                                                    
// Place a stop loss order, place a limit order of 0.001 BTC at 39000USDT each when the last trade price drops below 40000USDT
var orderData = await bittrexRestClient.SpotApi.Trading.PlaceConditionalOrderAsync(
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
```

### Requesting a specific order
```csharp
// Request info on order with id `1234`
var orderData = await bittrexRestClient.SpotApi.Trading.GetOrderAsync("1234");
```

### Requesting order history
```csharp
// Get all orders conform the parameters
 var ordersData = await bittrexRestClient.SpotApi.Trading.GetClosedOrdersAsync();
```

### Cancel order
```csharp
// Cancel order with id `1234`
var orderData = await bittrexRestClient.SpotApi.Trading.CancelOrderAsync("1234");
```

### Get user trades
```csharp
var userTradesResult = await bittrexRestClient.SpotApi.Trading.GetUserTradesAsync();
```

### Subscribing to market data updates
```csharp
var subscribeResult = await bittrexSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(data =>
{
    // Handle ticker data
});
```

### Subscribing to order updates
```csharp
var subscribeResult = await bittrexSocketClient.SpotStreams.SubscribeToOrderUpdatesAsync(data =>
    data =>
    {
      // Handle order updates
    });
```
