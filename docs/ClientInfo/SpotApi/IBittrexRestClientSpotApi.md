---
title: IBittrexRestClientSpotApi
has_children: true
parent: IBittrexClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`BittrexClient > SpotApi > IBittrexRestClient`  
*Bittrex spot API endpoints*
  
***
*Get the ISpotClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.*  
**ISpotClient CommonSpotClient { get; }**  
***
*Endpoints related to account settings, info or actions*  
**IBittrexRestClientSpotApiAccount Account { get; }**  
***
*Endpoints related to retrieving market and system data*  
**IBittrexRestClientSpotApiExchangeData ExchangeData { get; }**  
***
*Endpoints related to orders and trades*  
**IBittrexRestClientSpotApiTrading Trading { get; }**  
