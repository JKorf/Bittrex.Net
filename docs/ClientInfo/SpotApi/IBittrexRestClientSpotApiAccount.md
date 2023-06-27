---
title: IBittrexRestClientSpotApiAccount
has_children: false
parent: IBittrexRestClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`BittrexRestClient > SpotApi > Account`  
*Bittrex account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings*
  

***

## CancelWithdrawalAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals--withdrawalId--delete](https://bittrex.github.io/api/v3#operation--withdrawals--withdrawalId--delete)  
<p>

*Cancels a withdrawal*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.CancelWithdrawalAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexWithdrawal>> CancelWithdrawalAsync(string id, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|id|The id of the withdrawal to cancel|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAccountAsync  

[https://bittrex.github.io/api/v3#operation--account-get](https://bittrex.github.io/api/v3#operation--account-get)  
<p>

*Get account info*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetAccountAsync();  
```  

```csharp  
Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAccountVolumeAsync  

[https://bittrex.github.io/api/v3#operation--account-volume-get](https://bittrex.github.io/api/v3#operation--account-volume-get)  
<p>

*Get account volume*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetAccountVolumeAsync();  
```  

```csharp  
Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAssetPermissionAsync  

[https://bittrex.github.io/api/v3#operation--account-permissions-currencies--currencySymbol--get](https://bittrex.github.io/api/v3#operation--account-permissions-currencies--currencySymbol--get)  
<p>

*Get permissions for a specific asset*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetAssetPermissionAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionAsync(string asset, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAssetPermissionsAsync  

[https://bittrex.github.io/api/v3#operation--account-permissions-currencies-get](https://bittrex.github.io/api/v3#operation--account-permissions-currencies-get)  
<p>

*Get permissions for all assets*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetAssetPermissionsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetBalanceAsync  

[https://bittrex.github.io/api/v3#operation--balances--currencySymbol--get](https://bittrex.github.io/api/v3#operation--balances--currencySymbol--get)  
<p>

*Gets current balance for an asset*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetBalanceAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string asset, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The name of the asset to get balance for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetBalancesAsync  

[https://bittrex.github.io/api/v3#operation--balances-get](https://bittrex.github.io/api/v3#operation--balances-get)  
<p>

*Gets current balances. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetBalancesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetClosedDepositsAsync  

[https://bittrex.github.io/api/v3#operation--deposits-closed-get](https://bittrex.github.io/api/v3#operation--deposits-closed-get)  
<p>

*Gets list of closed deposits*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetClosedDepositsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetClosedDepositsAsync(string? asset = default, DepositStatus? status = default, DateTime? startTime = default, DateTime? endTime = default, int? pageSize = default, string? nextPageToken = default, string? previousPageToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Filter the list by asset|
|_[Optional]_ status|Filter the list by status of the deposit|
|_[Optional]_ startTime|Filter the list by time|
|_[Optional]_ endTime|Filter the list by time|
|_[Optional]_ pageSize|The max amount of results to return|
|_[Optional]_ nextPageToken|The id of the object after which to return results. Typically the last deposit id of the previous page|
|_[Optional]_ previousPageToken|The id of the object before which to return results. Typically the first deposit id of the next page|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetClosedWithdrawalsAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals-closed-get](https://bittrex.github.io/api/v3#operation--withdrawals-closed-get)  
<p>

*Gets a list of closed withdrawals*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetClosedWithdrawalsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetClosedWithdrawalsAsync(string? asset = default, WithdrawalStatus? status = default, DateTime? startTime = default, DateTime? endTime = default, int? pageSize = default, string? nextPageToken = default, string? previousPageToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Filter by asset|
|_[Optional]_ status|Filter by status|
|_[Optional]_ startTime|Filter the list by time|
|_[Optional]_ endTime|Filter the list by time|
|_[Optional]_ pageSize|The max amount of results to return|
|_[Optional]_ nextPageToken|The id of the object after which to return results. Typically the last withdrawal id of the previous page|
|_[Optional]_ previousPageToken|The id of the object before which to return results. Typically the first withdrawal id of the next page|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositAddressAsync  

[https://bittrex.github.io/api/v3#operation--addresses--currencySymbol--get](https://bittrex.github.io/api/v3#operation--addresses--currencySymbol--get)  
<p>

*Gets deposit addresses for an asset*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetDepositAddressAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string asset, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The name of the asset to get the deposit address for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositAddressesAsync  

[https://bittrex.github.io/api/v3#operation--addresses-get](https://bittrex.github.io/api/v3#operation--addresses-get)  
<p>

*Gets list of deposit addresses*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetDepositAddressesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexDepositAddress>>> GetDepositAddressesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositAsync  

[https://bittrex.github.io/api/v3#operation--deposits--depositId--get](https://bittrex.github.io/api/v3#operation--deposits--depositId--get)  
<p>

*Gets a deposit by id*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetDepositAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexDeposit>> GetDepositAsync(string depositId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|depositId|The id of the deposit|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositsByTransactionIdAsync  

[https://bittrex.github.io/api/v3#operation--deposits-ByTxId--txId--get](https://bittrex.github.io/api/v3#operation--deposits-ByTxId--txId--get)  
<p>

*Gets list of deposits for a transaction id*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetDepositsByTransactionIdAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|transactionId|The id of the transaction|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetFiatTransactionFeesAsync  

[https://bittrex.github.io/api/v3#operation--account-fees-fiat-get](https://bittrex.github.io/api/v3#operation--account-fees-fiat-get)  
<p>

*Get account withdrawal/deposit fees*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetFiatTransactionFeesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexFiatFee>>> GetFiatTransactionFeesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenDepositsAsync  

[https://bittrex.github.io/api/v3#operation--deposits-open-get](https://bittrex.github.io/api/v3#operation--deposits-open-get)  
<p>

*Gets list of open deposits. Sequence number of the data available via ResponseHeaders.GetSequence()*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetOpenDepositsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetOpenDepositsAsync(string? asset = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Filter the list by asset|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenWithdrawalsAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals-open-get](https://bittrex.github.io/api/v3#operation--withdrawals-open-get)  
<p>

*Gets a list of open withdrawals*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetOpenWithdrawalsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetOpenWithdrawalsAsync(string? asset = default, WithdrawalStatus? status = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Filter by asset|
|_[Optional]_ status|Filter by status|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolPermissionAsync  

[https://bittrex.github.io/api/v3#operation--account-permissions-markets--marketSymbol--get](https://bittrex.github.io/api/v3#operation--account-permissions-markets--marketSymbol--get)  
<p>

*Get permissions for a specific symbol*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetSymbolPermissionAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolPermissionsAsync  

[https://bittrex.github.io/api/v3#operation--account-permissions-markets-get](https://bittrex.github.io/api/v3#operation--account-permissions-markets-get)  
<p>

*Get permissions for all symbols*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetSymbolPermissionsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradingFeesAsync  

[https://bittrex.github.io/api/v3#operation--account-fees-trading-get](https://bittrex.github.io/api/v3#operation--account-fees-trading-get)  
<p>

*Get account trading fees*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetTradingFeesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexTradingFee>>> GetTradingFeesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetWithdrawalAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals--withdrawalId--get](https://bittrex.github.io/api/v3#operation--withdrawals--withdrawalId--get)  
<p>

*Gets withdrawal by id*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetWithdrawalAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexWithdrawal>> GetWithdrawalAsync(string id, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|id|The id of the withdrawal|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetWithdrawalsByTransactionIdAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals-ByTxId--txId--get](https://bittrex.github.io/api/v3#operation--withdrawals-ByTxId--txId--get)  
<p>

*Gets a list of withdrawals by transaction id*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetWithdrawalsByTransactionIdAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|transactionId|The id of the transaction|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetWithdrawalWhitelistAddressesAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals-allowed-addresses-get](https://bittrex.github.io/api/v3#operation--withdrawals-allowed-addresses-get)  
<p>

*Gets a list of whitelisted address for withdrawals*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.GetWithdrawalWhitelistAddressesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## RequestDepositAddressAsync  

[https://bittrex.github.io/api/v3#operation--addresses-post](https://bittrex.github.io/api/v3#operation--addresses-post)  
<p>

*Request a deposit address for an asset*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.RequestDepositAddressAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexDepositAddress>> RequestDepositAddressAsync(string asset, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The name of the asset to get request a deposit address for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## WithdrawAsync  

[https://bittrex.github.io/api/v3#operation--withdrawals-post](https://bittrex.github.io/api/v3#operation--withdrawals-post)  
<p>

*Withdraw from Bittrex*  

```csharp  
var client = new BittrexRestClient();  
var result = await client.SpotApi.Account.WithdrawAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<BittrexWithdrawal>> WithdrawAsync(string asset, decimal quantity, string address, string? addressTag = default, string? clientWithdrawId = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The asset to withdraw|
|quantity|The quantity to withdraw|
|address|The address to withdraw to|
|_[Optional]_ addressTag|A tag for the address|
|_[Optional]_ clientWithdrawId|Client id to identify the withdrawal|
|_[Optional]_ ct|Cancellation token|

</p>
