# ouya.*

> --------------------- ------------------------------------------------------------------------------------------
> __Type__              plugin.ouya
> __Revision__          001
> __Keywords__          ouya, in-app-purchase, iap, store, gameNetwork
> __Sample code__       
> __See also__          [Marketplace](http://www.coronalabs.com/store/plugin)
> __Availability__      Pro, Enterprise
> --------------------- ------------------------------------------------------------------------------------------

## Overview

The OUYA plugin is available for Corona PRO and Corona Enterprise.
The plugin adds an interface for in-app-purchases which allows purchases of entitlements and consumables.
The plugin also adds support for accessing the gamer unique identifier.

## Platforms

Ouya is only supported on Android.

## Syntax

	local ouya = require "ouya"

## Functions

#### plugin_ouya.ouyaSetDeveloperId

Before IAP functions can be invoked the developer id must be set. This developer id corresponds with the signing key, and bundle id, and needs to match for proper encryption/decryption.

#### plugin_ouya.asyncLuaOuyaFetchGamerUUID

After setting the developer id, the gamer unique identifier can be accessed.

#### plugin_ouya.asyncLuaOuyaRequestProducts



#### [ouya.function2()][plugin.ouya.nameOfFunction2MarkdownFile]

## Project Settings

### SDK

When you build using the Corona Simulator, the server automatically takes care of integrating the plugin into your project. 

All you need to do is add an entry into a `plugins` table of your `build.settings`. The following is an example of a minimal `build.settings` file:

``````
settings =
{
	plugins =
	{
		["plugin.ouya"] =
		{
			publisherId = "tv.ouya"
		},
	},		
}
``````

### Enterprise

The required AndroidManifest.xml settings go here...

## Sample Code

<a target=_blank href="https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin"><b>InAppPurchasesPlugin</b></a> - Uses ouya plugin for in-app-purchases for Mac and Windows (required Corona PRO)

## Support

More support is available from the inneractive team:

* [E-mail](mailto:tim@tagenigma.com)
* [Forum](http://forums.ouya.tv/categories/corona-on-ouya)
* [Docs](https://devs.ouya.tv/developers/docs/corona)
* [Plugin Publisher](http://www.ouya.tv)
