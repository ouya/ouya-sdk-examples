# ouya.*

> --------------------- ------------------------------------------------------------------------------------------
> __Type__              [library][api.type.library]
> __Revision__          [REVISION_LABEL](REVISION_URL)
> __Keywords__          OUYA
> __Availability__      Pro, Enterprise
> __Platforms__			Android
> --------------------- ------------------------------------------------------------------------------------------

## Overview

The OUYA plugin allows you to build games supported on the OUYA system. This plugin adds an interface for <nobr>in-app</nobr> purchases (entitlements and consumables) as well as support for accessing the gamer unique identifier.

In addition, this plugin integrates with OUYA-Everywhere Input which shares code with the `OuyaFramework` for controller support. Without needing to rebuild or republish, new support for controllers and devices will be added with OUYA OTA updates.

Plugin has been upgraded to ODK version: 2.0.1353.


## Gotchas

Before in-app purchase functions can be invoked, wait a couple seconds to allow Java to load the OUYA plugin.
When you invoke an <nobr>in-app</nobr> purchase method, wait for the callback to happen before making a call to another <nobr>in-app</nobr> purchase function. Each call should happen in serial versus asynchronous.


## Functions

### Named Java Methods


#### ouyaSDK.initOuyaPlugin

Requires callback arguments for onSuccess, onFailure, and the initialization plugin values.
This method initializes the plugin and invokes the corresponding callback.


#### ouyaSDK.asyncLuaOuyaInitInput

Requires callback arguments for onGenericMotionEvent, onKeyDown, and onKeyUp.
This method subcribes input callbacks for axis and button events.
This is also connected with OUYA-Everywhere input.
OUYA-Everywhere input ensures the same controller mappings will work on all supported OUYA devices.
New devices can be adding without needing to recompile the app/game.


#### ouyaSDK.asyncLuaOuyaGetControllerName

Requires onGetControllerName callback and PlayerNum arguments.
The onGetControllerName callback is passed the string of the controller name. The controller name comes from the OUYA Framework which provides a more detailed name of the controller than the device name on the KeyEvent and MotionEvent.
PlayerNum is a zero-based integer which is intended to indicate controller #1, #2, #3, and #4.


#### ouyaSDK.asyncLuaOuyaRequestGamerInfo

Requires initOuyaPlugin to be successfully invoked.

This method has success, failure, and cancel callbacks.

The success event provides access to the gamer's username and unique identifier.


#### ouyaSDK.asyncLuaOuyaRequestProducts

Requires initOuyaPlugin to be successfully invoked.

This method has success, failure, and cancel callbacks.

The success event provides access purchasable details for name, description, and price for the given entitlement/consumable purchasable identifier.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


#### ouyaSDK.asyncLuaOuyaRequestPurchase

Requires initOuyaPlugin to be successfully invoked.

This method has success, failure, and cancel callbacks.

The success event provides opens the purchase dialog for the requested purchasable identifier.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


#### ouyaSDK.asyncLuaOuyaRequestReceipts

Requires initOuyaPlugin to be successfully invoked.

This method has success, failure, and cancel callbacks.

The success event returns the list of receipts associated with the bundle identifier for the gamer.

The failure event provides an error code and description.

The cancel event provides no arguments and indicates the request was cancelled.


### Wrapped Methods


#### plugin_ouya.initOuyaPlugin

Be sure to set your developer id from the [developer portal](http://devs.ouya.tv).

```lua
local ouya = require("plugin.ouya") -- load the ouya plugin

callbacksInitOuyaPlugin = require "callbacksInitOuyaPlugin"

local json = require "json"
plugin_ouya = require "plugin_ouya"

if nil ~= plugin_ouya and nil ~= plugin_ouya.initOuyaPlugin then
	local data = {
	[1] = {
	    ["key"] = "tv.ouya.developer_id",
	    ["value"] = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0"
	}};
	local jsonData = json.encode(data);
	--print (jsonData);
	plugin_ouya.initOuyaPlugin(
		callbacksInitOuyaPlugin.onSuccess,
		callbacksInitOuyaPlugin.onFailure,
		jsonData);
end
```


#### plugin_ouya.asyncLuaOuyaInitInput

Wraps calling ouyaSDK.asyncLuaOuyaInitInput to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaGetControllerName

Wraps calling ouyaSDK.asyncLuaOuyaGetControllerName to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestGamerInfo

Wraps calling ouyaSDK.asyncLuaOuyaRequestGamerInfo to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestProducts

Wraps calling ouyaSDK.asyncLuaOuyaRequestProducts to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestPurchase

Wraps calling ouyaSDK.asyncLuaOuyaRequestPurchase to check if the plugin has been initialized before invoking.


#### plugin_ouya.asyncLuaOuyaRequestReceipts

Wraps calling ouyaSDK.asyncLuaOuyaRequestReceipts to check if the plugin has been initialized before invoking.


## Signing Key

Remember to include the `key.der` signing key from the OUYA developer portal inside your main Corona project directory, alongside `main.lua`.


## Project Settings

To use this plugin, add an entry into the `plugins` table of `build.settings`. When added, the build server will integrate the plugin during the build phase.

``````lua
settings =
{
	plugins =
	{
		["plugin.ouya"] =
		{
			publisherId = "tv.ouya",
			supportedPlatforms = { android=true },
		},
	},
}
``````

In addition, you must add an intent filter to the `android` table within `build.settings` so that your game icon will appear in the OUYA Play section:

``````lua
settings =
{
	android =
   {
		mainIntentFilter =
		{
			categories = { "tv.ouya.intent.category.GAME" },
		}
	},
}
``````


## Sample Code

* Enterprise samples &mdash; [https://github.com/ouya/docs/blob/master/corona.md](https://github.com/ouya/docs/blob/master/corona.md)

* In-App Purchase sample &mdash; [https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin/](https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin)

* Virtual Controller sample &mdash; [https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/VirtualControllerPlugin/](https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/VirtualControllerPlugin)


## Documentation

[https://devs.ouya.tv/developers/docs/corona/](https://devs.ouya.tv/developers/docs/corona)


## Support

* [http://www.ouya.tv](http://www.ouya.tv)
* [OUYA Forums](http://forums.ouya.tv/categories/corona-on-ouya)
* [Corona Forums](http://forums.coronalabs.com/forum/627-ouya/)
* [Developer Support](https://github.com/ouya/docs/blob/master/developer_support.md)
* [tgraupmann@ouya.tv](mailto:tgraupmann@ouya.tv)