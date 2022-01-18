---
title: IBittrexClientSpotApi
has_children: true
parent: IBittrexClient
---
*[generated documentation]*  
`BittrexClient > SpotApi`  
*Bittrex spot API endpoints*
  
***
*Get the ISpotClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.*  
**ISpotClient CommonSpotClient { get; }**  
***
*Endpoints related to account settings, info or actions*  
**[IBittrexClientSpotApiAccount](IBittrexClientSpotApiAccount.html) Account { get; }**  
***
*Endpoints related to retrieving market and system data*  
**[IBittrexClientSpotApiExchangeData](IBittrexClientSpotApiExchangeData.html) ExchangeData { get; }**  
***
*Endpoints related to orders and trades*  
**[IBittrexClientSpotApiTrading](IBittrexClientSpotApiTrading.html) Trading { get; }**  
