---
title: IBittrexRestClientSpotApiTrading
has_children: false
parent: IBittrexRestClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`BittrexRestClient > SpotApi > Trading`  
*Bittrex trading endpoints, placing and mananging orders.*
  

***

## CancelAllOrdersAsync  

[https://bittrex.github.io/api/v3#operation--orders-open-delete](https://bittrex.github.io/api/v3#operation--orders-open-delete)  
<p>

*Cancels all open orders*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.CancelAllOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexOrder>>> CancelAllOrdersAsync(string? symbol = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Only cancel open orders for a specific symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelConditionalOrderAsync  

[https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--delete](https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--delete)  
<p>

*Cancels a condtional order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.CancelConditionalOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string orderId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orderId|Id of the conditional order|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelMultipleOrdersAsync  

[https://bittrex.github.io/api/v3#operation--batch-post](https://bittrex.github.io/api/v3#operation--batch-post)  
<p>

*Cancel multiple orders in a single call*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.CancelMultipleOrdersAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> CancelMultipleOrdersAsync(string[] ordersToCancel, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|ordersToCancel|Orders to cancel|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelOrderAsync  

[https://bittrex.github.io/api/v3#operation--orders--orderId--delete](https://bittrex.github.io/api/v3#operation--orders--orderId--delete)  
<p>

*Cancels an order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.CancelOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexOrder>> CancelOrderAsync(string orderId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orderId|The id of the order|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetClosedConditionalOrdersAsync  

[https://bittrex.github.io/api/v3#operation--conditional-orders-closed-get](https://bittrex.github.io/api/v3#operation--conditional-orders-closed-get)  
<p>

*Gets a list of closed conditional orders*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetClosedConditionalOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = default, DateTime? startTime = default, DateTime? endTime = default, int? pageSize = default, string? nextPageToken = default, string? previousPageToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Filter by symbol|
|_[Optional]_ startTime|Filter the list by time|
|_[Optional]_ endTime|Filter the list by time|
|_[Optional]_ pageSize|The max amount of results to return|
|_[Optional]_ nextPageToken|The id of the object after which to return results. Typically the last id of the previous page|
|_[Optional]_ previousPageToken|The id of the object before which to return results. Typically the first id of the next page|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetClosedOrdersAsync  

[https://bittrex.github.io/api/v3#operation--orders-closed-get](https://bittrex.github.io/api/v3#operation--orders-closed-get)  
<p>

*Gets a list of closed orders*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetClosedOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexOrder>>> GetClosedOrdersAsync(string? symbol = default, DateTime? startTime = default, DateTime? endTime = default, int? pageSize = default, string? nextPageToken = default, string? previousPageToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Filter the list by symbol|
|_[Optional]_ startTime|Filter the list by time|
|_[Optional]_ endTime|Filter the list by time|
|_[Optional]_ pageSize|The max amount of results to return|
|_[Optional]_ nextPageToken|The id of the object after which to return results. Typically the last order id of the previous page|
|_[Optional]_ previousPageToken|The id of the object before which to return results. Typically the first order id of the next page|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetConditionalOrderAsync  

[https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--get](https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--get)  
<p>

*Get details on a condtional order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetConditionalOrderAsync();  
```  

```csharp  
Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderId|Id of the conditional order|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenConditionalOrdersAsync  

[https://bittrex.github.io/api/v3#operation--conditional-orders-open-get](https://bittrex.github.io/api/v3#operation--conditional-orders-open-get)  
<p>

*Get list op open conditional orders*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetOpenConditionalOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Filter by symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenOrdersAsync  

[https://bittrex.github.io/api/v3#operation--orders-open-get](https://bittrex.github.io/api/v3#operation--orders-open-get)  
<p>

*Gets a list of open orders. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetOpenOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexOrder>>> GetOpenOrdersAsync(string? symbol = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|The symbol to get open orders for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderAsync  

[https://bittrex.github.io/api/v3#operation--orders--orderId--get](https://bittrex.github.io/api/v3#operation--orders--orderId--get)  
<p>

*Gets info on an order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexOrder>> GetOrderAsync(string orderId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orderId|The id of the order to retrieve|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderTradesAsync  

[https://bittrex.github.io/api/v3#operation--orders--orderId--executions-get](https://bittrex.github.io/api/v3#operation--orders--orderId--executions-get)  
<p>

*Gets trades for an order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetOrderTradesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetOrderTradesAsync(string orderId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orderId|The id of the order to retrieve trades for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradeByIdAsync  

[https://bittrex.github.io/api/v3#operation--executions--executionId--get](https://bittrex.github.io/api/v3#operation--executions--executionId--get)  
<p>

*Gets info on a user trade*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetUserTradeByIdAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexUserTrade>> GetUserTradeByIdAsync(string tradeId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|tradeId|The id of the trade to retrieve|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradesAsync  

[https://bittrex.github.io/api/v3#operation--executions-get](https://bittrex.github.io/api/v3#operation--executions-get)  
<p>

*Gets user trades*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.GetUserTradesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetUserTradesAsync(string? symbol = default, DateTime? startTime = default, DateTime? endTime = default, int? pageSize = default, string? nextPageToken = default, string? previousPageToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Filter by symbol|
|_[Optional]_ startTime|Filter the list by time|
|_[Optional]_ endTime|Filter the list by time|
|_[Optional]_ pageSize|The max amount of results to return|
|_[Optional]_ nextPageToken|The id of the object after which to return results. Typically the last withdrawal id of the previous page|
|_[Optional]_ previousPageToken|The id of the object before which to return results. Typically the first withdrawal id of the next page|
|_[Optional]_ ct|Cancellation token|

</p>

***

## PlaceConditionalOrderAsync  

[https://bittrex.github.io/api/v3#operation--conditional-orders-post](https://bittrex.github.io/api/v3#operation--conditional-orders-post)  
<p>

*Place a new conditional order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.PlaceConditionalOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexConditionalOrder>> PlaceConditionalOrderAsync(string symbol, ConditionalOrderOperand operand, BittrexUnplacedOrder? orderToCreate = default, BittrexLinkedOrder? orderToCancel = default, decimal? triggerPrice = default, decimal? trailingStopPercent = default, string? clientConditionalOrderId = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol of the order|
|operand|The operand of the order|
|_[Optional]_ orderToCreate|Order to create when condition is triggered|
|_[Optional]_ orderToCancel|Order to cancel when condition is triggered|
|_[Optional]_ triggerPrice|Trigger price|
|_[Optional]_ trailingStopPercent|Trailing stop percent|
|_[Optional]_ clientConditionalOrderId|Client order id for conditional order|
|_[Optional]_ ct|Cancellation token|

</p>

***

## PlaceMultipleOrdersAsync  

[https://bittrex.github.io/api/v3#operation--batch-post](https://bittrex.github.io/api/v3#operation--batch-post)  
<p>

*Place multiple orders in a single call*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.PlaceMultipleOrdersAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> PlaceMultipleOrdersAsync(BittrexNewBatchOrder[] orders, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orders|Orders to place|
|_[Optional]_ ct|Cancellation token|

</p>

***

## PlaceOrderAsync  

[https://bittrex.github.io/api/v3#operation--orders-post](https://bittrex.github.io/api/v3#operation--orders-post)  
<p>

*Places an order*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Trading.PlaceOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, TimeInForce timeInForce, decimal? quantity = default, decimal? price = default, decimal? quoteQuantity = default, string? clientOrderId = default, bool? useAwards = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol of the order|
|side|The side of the order|
|type|The type of order|
|timeInForce|The time in force of the order|
|_[Optional]_ quantity|The quantity of the order|
|_[Optional]_ price|The price of the order (limit orders only)|
|_[Optional]_ quoteQuantity|The amount of quote quantity to use|
|_[Optional]_ clientOrderId|Id to track the order by|
|_[Optional]_ useAwards|Option to use Bittrex credits for the order|
|_[Optional]_ ct|Cancellation token|

</p>
