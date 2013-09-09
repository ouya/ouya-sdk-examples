# ouya.*

> __Type__              plugin.ouya<br/>
> __Revision__          001<br/>
> __Keywords__          ouya, in-app-purchase, iap, store, gameNetwork<br/>
> __See also__          [Marketplace](http://www.coronalabs.com/store/plugin)<br/>
> __Availability__      Pro, Enterprise<br/>


## Overview

The OUYA plugin is available for Corona PRO and Corona Enterprise.
The plugin adds an interface for in-app-purchases which allows purchases of entitlements and consumables.
The plugin also adds support for accessing the gamer unique identifier.

## Platforms

Ouya is only supported on Android.

## Syntax

local ouya = require("plugin.ouya") 

This causes the plugin to initialize which registers the Java Named Functions making it possible to call into the plugin from Lua.


#### key.der

This is the signing key from the developer portal that goes into your Corona Assets folder (next to main.lua).


## Functions


#### ouyaSDK.ouyaSetDeveloperId

Before IAP functions can be invoked the developer id must be set.
This developer id corresponds with the signing key, and bundle id, and needs to match for proper encryption/decryption.


#### ouyaSDK.asyncLuaOuyaFetchGamerUUID

Requires developer id to be set.

This method has success, failure, and cancel callbacks.

The success event provides access to the gamer's unique identifier.


#### ouyaSDK.asyncLuaOuyaRequestProducts

Requires developer id to be set.

This method has success, failure, and cancel callbacks.

The success event provides access purchasable details for name, description, and price for the given entitlement/consumable purchasable identifier.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


#### ouyaSDK.asyncLuaOuyaRequestPurchase

Requires developer id to be set.

This method has success, failure, and cancel callbacks.

The success event provides opens the purchase dialog for the requested purchasable identifier.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


#### ouyaSDK.asyncLuaOuyaRequestReceipts

Requires developer id to be set.

This method has success, failure, and cancel callbacks.

The success event returns the list of receipts associated with the bundle identifier for the gamer.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


#### plugin_ouya.developerId = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

Be sure to set your developer id from the developer portal.


#### plugin_ouya.initialize

Common method handles initialization for the wrapper methods.


#### plugin_ouya.ouyaSetDeveloperId

Wraps calling ouyaSDK.ouyaSetDeveloperId so that the plugin is only initialized once.


#### plugin_ouya.asyncLuaOuyaFetchGamerUUID

Wraps calling ouyaSDK.asyncLuaOuyaFetchGamerUUID to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestProducts

Wraps calling ouyaSDK.asyncLuaOuyaRequestProducts to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestPurchase

Wraps calling ouyaSDK.asyncLuaOuyaRequestPurchase to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestReceipts

Wraps calling ouyaSDK.asyncLuaOuyaRequestReceipts to check if the plugin has been initialized before invoking.


## Project Settings

### SDK

When you build using the Corona Simulator, the server automatically takes care of integrating the plugin into your project. 

All you need to do is add an entry into a `plugins` table of your `build.settings`. The following is an example of a minimal `build.settings` file:

```
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
```

Add the intent filter so that your game icon will appear in the OUYA Play section.

```
   android =
   {
      mainIntentFilter =
      {
         categories = { "tv.ouya.intent.category.GAME" },
      }
   },
```


### Enterprise

The plugin is compatible with Corona Enterpise.

You'll find more enterprise specific examples on the OUYA site. https://github.com/ouya/docs/blob/master/corona.md


## Sample Code

<a target=_blank href="https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin"><b>InAppPurchasesPlugin</b></a> - Uses ouya plugin for in-app-purchases for Mac and Windows (required Corona PRO)

## Support

More support is available from the inneractive team:

* [E-mail](mailto:tim@tagenigma.com)
* [Forum](http://forums.ouya.tv/categories/corona-on-ouya)
* [Docs](https://devs.ouya.tv/developers/docs/corona)
* [Plugin Publisher](http://www.ouya.tv)
