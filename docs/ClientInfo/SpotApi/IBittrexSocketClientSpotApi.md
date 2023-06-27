---
title: IBittrexSocketClientSpotApi
has_children: true
parent: Socket API documentation
---
*[generated documentation]*  
`BittrexSocketClient > SpotApi`  
*Bittrex Spot streams*
  

***

## SubscribeToBalanceUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Balance](https://bittrex.github.io/api/v3#method-Balance)  
<p>

*Subscribe to balance updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToBalanceUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToConditionalOrderUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Conditional-Order](https://bittrex.github.io/api/v3#method-Conditional-Order)  
<p>

*Subscribe to conditional order updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToConditionalOrderUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToConditionalOrderUpdatesAsync(Action<DataEvent<BittrexConditionalOrderUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToDepositUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Deposit](https://bittrex.github.io/api/v3#method-Deposit)  
<p>

*Subscribe to deposit updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToDepositUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToHeartbeatAsync  

[https://bittrex.github.io/api/v3#method-Heartbeat](https://bittrex.github.io/api/v3#method-Heartbeat)  
<p>

*Subscribe to heartbeat updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToHeartbeatAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onHeartbeat|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToKlineUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Candle](https://bittrex.github.io/api/v3#method-Candle)  
<p>

*Subscribe to kline(candle) updates for a symbol*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToKlineUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|interval|Interval of the candles|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToKlineUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Candle](https://bittrex.github.io/api/v3#method-Candle)  
<p>

*Subscribe to kline(candle) updates for a symbol*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToKlineUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols|
|interval|Interval of the candles|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderBookUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Orderbook](https://bittrex.github.io/api/v3#method-Orderbook)  
<p>

*Subscribe to order book updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToOrderBookUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|depth|The depth of the oder book to receive update for|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderBookUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Orderbook](https://bittrex.github.io/api/v3#method-Orderbook)  
<p>

*Subscribe to order book updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToOrderBookUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols|
|depth|The depth of the oder book to receive update for|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Order](https://bittrex.github.io/api/v3#method-Order)  
<p>

*Subscribe to order updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToOrderUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSymbolSummaryUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Market-Summaries](https://bittrex.github.io/api/v3#method-Market-Summaries)  
<p>

*Subscribe to all symbol summary updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToSymbolSummaryUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSymbolSummaryUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Market-Summary](https://bittrex.github.io/api/v3#method-Market-Summary)  
<p>

*Subscribe to symbol summary updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToSymbolSummaryUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol, Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSymbolSummaryUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Market-Summary](https://bittrex.github.io/api/v3#method-Market-Summary)  
<p>

*Subscribe to symbol summary updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToSymbolSummaryUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Tickers](https://bittrex.github.io/api/v3#method-Tickers)  
<p>

*Subscribe to all symbols ticker updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Ticker](https://bittrex.github.io/api/v3#method-Ticker)  
<p>

*Subscribe to symbol ticker updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Ticker](https://bittrex.github.io/api/v3#method-Ticker)  
<p>

*Subscribe to symbol ticker updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTradeUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Trade](https://bittrex.github.io/api/v3#method-Trade)  
<p>

*Subscribe to symbol trade updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTradeUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Trade](https://bittrex.github.io/api/v3#method-Trade)  
<p>

*Subscribe to symbol trade updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToUserTradeUpdatesAsync  

[https://bittrex.github.io/api/v3#method-Execution](https://bittrex.github.io/api/v3#method-Execution)  
<p>

*Subscribe to user trade updates*  

```csharp  
var client = new BittrexSocketClient();  
var result = await client.SpotApi.SubscribeToUserTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BittrexExecutionUpdate>> onUpdate, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|onUpdate|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>
