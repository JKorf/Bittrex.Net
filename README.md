# Bittrex.Net ![Icon](https://github.com/JKorf/Bittrex.Net/blob/master/Resources/icon.png?raw=true)

A .Net wrapper for the Bittrex API as described on [Bittrex](https://bittrex.com/Home/Api), including all features.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Bittrex.Net/issues)**

## Installation
![Nuget version](https://img.shields.io/nuget/v/bittrex.net.svg) ![Nuget downloads](https://img.shields.io/nuget/dt/Bittrex.Net.svg)

Available on [NuGet](https://www.nuget.org/packages/Bittrex.Net/):
```
PM> Install-Package Bittrex.Net
```
For more details on installing refer to the [Wiki](https://github.com/JKorf/Bittrex.Net/wiki/Installation)

## Getting started
[Getting started](https://github.com/JKorf/Bittrex.Net/wiki/Getting-started)

## Release notes
* Version 1.1.2 - 10 nov 2017
	* Added ratelimiting options
	* Added connection lost/restored events in socket client
	* Added log verbosity None to be able to mute all logging
	* Added encryptor to dispose

* Version 1.1.1 - 9 nov 2017
	* Fix for error in wrong category
	* Small code documentation fixes

* Version 1.1.0 - 9 nov 2017
	* Added automatic configurable retry on server errors
	* Refactor on error returns

* Version 1.0.1 - 8 nov 2017
	* Added reconnect functionality in socket client as long as there are still subscriptions open

* Version 1.0.0 - 6 nov 2017
	* Release version 1.0.0
	* Additional unit tests, also for the socket client	
	* Small refactoring for unit testability
	* Small cleanup
	
* Version 0.0.4 - 4 nov 2017
	* Added icon

* Version 0.0.3 - 1 nov 2017
	* Small naming changes in socket client
	* Added api key checks in private endpoints
	* Updated documentation
	* Added example project

* Version 0.0.2 - 1 nov 2017
	* Added BittrexSocketClient
	* Updated documentation