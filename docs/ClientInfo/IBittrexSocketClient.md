---
title: Socket API documentation
has_children: true
---
*[generated documentation]*  
### BittrexSocketClient  
*Client for accessing the Bittrex websocket API*
  
***
*Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.*  
**void SetApiCredentials(ApiCredentials credentials);**  
***
*Spot streams*  
**[IBittrexSocketClientSpotApi](SpotApi/IBittrexSocketClientSpotApi.html) SpotApi { get; }**  
