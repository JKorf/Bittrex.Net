---
title: IBittrexRestClientSpotApiExchangeData
has_children: false
parent: IBittrexRestClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`BittrexRestClient > SpotApi > ExchangeData`  
*Bittrex exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.*
  

***

## GetAssetAsync  

[https://bittrex.github.io/api/v3#operation--currencies--symbol--get](https://bittrex.github.io/api/v3#operation--currencies--symbol--get)  
<p>

*Gets info on a asset*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetAssetAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexAsset>> GetAssetAsync(string asset, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The name of the asset|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAssetsAsync  

[https://bittrex.github.io/api/v3#operation--currencies-get](https://bittrex.github.io/api/v3#operation--currencies-get)  
<p>

*Gets a list of all assets*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetAssetsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexAsset>>> GetAssetsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetHistoricalKlinesAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--historical--year---month---day--get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--historical--year---month---day--get)  
<p>

*Gets historical klines for a symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetHistoricalKlinesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexKline>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = default, int? day = default, KlineType? type = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get klines for|
|interval|The interval of the klines|
|year|The year to get klines for|
|_[Optional]_ month|The month to get klines for|
|_[Optional]_ day|The day to get klines for|
|_[Optional]_ type|The type of klines|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetKlinesAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--recent-get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--recent-get)  
<p>

*Gets the klines for a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetKlinesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexKline>>> GetKlinesAsync(string symbol, KlineInterval interval, KlineType? type = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get klines for|
|interval|The interval of the klines|
|_[Optional]_ type|The type of klines|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderBookAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--orderbook-get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--orderbook-get)  
<p>

*Gets the order book of a symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string symbol, int? limit = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get the order book for|
|_[Optional]_ limit|The number of results per side for the order book (1, 25 or 500)|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetServerTimeAsync  

[https://bittrex.github.io/api/v3#operation--ping-get](https://bittrex.github.io/api/v3#operation--ping-get)  
<p>

*Gets the server time*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetServerTimeAsync();  
```  

```csharp  
Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--get)  
<p>

*Gets information about a symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetSymbolAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get info for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolsAsync  

[https://bittrex.github.io/api/v3#operation--markets-get](https://bittrex.github.io/api/v3#operation--markets-get)  
<p>

*Gets information about all available symbols*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexSymbol>>> GetSymbolsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolSummariesAsync  

[https://bittrex.github.io/api/v3#operation--markets-summaries-get](https://bittrex.github.io/api/v3#operation--markets-summaries-get)  
<p>

*Gets summaries of all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetSymbolSummariesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexSymbolSummary>>> GetSymbolSummariesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolSummaryAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--summary-get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--summary-get)  
<p>

*Gets summary of a symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetSymbolSummaryAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexSymbolSummary>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get info for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTickerAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--ticker-get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--ticker-get)  
<p>

*Gets the ticker of a symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetTickerAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexTick>> GetTickerAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get ticker for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTickersAsync  

[https://bittrex.github.io/api/v3#operation--markets-tickers-get](https://bittrex.github.io/api/v3#operation--markets-tickers-get)  
<p>

*Gets list of tickers for all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetTickersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexTick>>> GetTickersAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradeHistoryAsync  

[https://bittrex.github.io/api/v3#operation--markets--marketSymbol--trades-get](https://bittrex.github.io/api/v3#operation--markets--marketSymbol--trades-get)  
<p>

*Gets the trade history of a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.ExchangeData.GetTradeHistoryAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexTrade>>> GetTradeHistoryAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get trades for|
|_[Optional]_ ct|Cancellation token|

</p>
