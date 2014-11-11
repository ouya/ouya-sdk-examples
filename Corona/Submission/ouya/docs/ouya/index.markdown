# ouya.*

> __Type__              plugin.ouya

> __Revision__          004 (ODK-2.0.1353.1)

> __Keywords__          ouya, in-app-purchase, iap, store, gameNetwork

> __See also__          [Marketplace](http://www.coronalabs.com/store/plugin)

> __Availability__      Basic, Pro, Enterprise

> __Platforms__	       Android


## Overview

The OUYA plugin allows you to build games supported on the OUYA system.

The plugin provides an interface for <nobr>in-app</nobr> purchases (entitlements and consumables).

The plugin provides access to the gamer unique identifier and username.

The plugin integrates with OUYA-Everywhere Input which shares code with the OuyaFramework for controller support. Without needing to rebuild or republish, new support for controllers and devices will be added with OUYA OTA updates.


## Syntax

local ouya = require("plugin.ouya") 

This causes the plugin to initialize which registers the Java Named Functions making it possible to call into the plugin from Lua.


## Functions

Before in-app-purchase functions can be invoked, wait a couple seconds to allow Java to load the ouya plugin.
When you invoke an in-app-purchase method wait for the callback to happen before making a call to another in-app-purchase function.
Each call should happen in serial versus asyncronous.


##### Named Java methods:


#### ouyaSDK.initOuyaPlugin

Requires callback arguments for onSuccess and onFailure.
This method passes the plugin intialization values to start the OUYA Plugin.
The onSuccess callback is invoked if the plugin initializes successfully.
The onFailure callback is invoked if the plugin fails to initialize.

lua
```
callbacksInitOuyaPlugin.onSuccess = function ()
end

callbacksInitOuyaPlugin.onFailure = function (errorCode, errorMessage)
end

callbacksInitOuyaPlugin = require "callbacksInitOuyaPlugin"
local json = require "json"
if nil ~= ouyaSDK and nil ~= ouyaSDK.initOuyaPlugin then
	local data = {
	[1] = {
	    ["key"] = "tv.ouya.developer_id",
	    ["value"] = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0"
	}};
	local jsonData = json.encode(data);
	ouyaSDK.initOuyaPlugin(callbacksInitOuyaPlugin.onSuccess, callbacksInitOuyaPlugin.onFailure, jsonData);
end
```

#### ouyaSDK.asyncLuaOuyaInitInput

Requires callback arguments for onGenericMotionEvent, onKeyDown, and onKeyUp.
This method subcribes input callbacks for axis and button events.
This is also connected with OUYA-Everywhere input.
OUYA-Everywhere input ensures the same controller mappings will work on all supported OUYA devices.
New devices can be adding without needing to recompile the app/game.

lua
```
inputs.onGenericMotionEvent = function (playerNum, axis, val)
end

inputs.onKeyDown = function (playerNum, button)
end

inputs.onKeyUp = function (playerNum, button)
end

if nil ~= ouyaSDK and nil ~= ouyaSDK.asyncLuaOuyaInitInput then
	ouyaSDK.asyncLuaOuyaInitInput(inputs.onGenericMotionEvent, inputs.onKeyDown, inputs.onKeyUp);
end
```

#### ouyaSDK.asyncLuaOuyaGetControllerName

Requires onGetControllerName callback and PlayerNum arguments.
The onGetControllerName callback is passed the string of the controller name. The controller name comes from the OUYA Framework which provides a more detailed name of the controller than the device name on the KeyEvent and MotionEvent.
PlayerNum is a zero-based integer which is intended to indicate controller #1, #2, #3, and #4.


#### ouyaSDK.asyncLuaOuyaRequestGamerInfo

Requires developer id to be set.

This method has success, failure, and cancel callbacks.

The success event provides access to the gamer's username and unique identifier.


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


##### Wrapped Methods:


Be sure to set your developer id from the [developer portal](http://devs.ouya.tv).


#### plugin_ouya.initOuyaPlugin

Wraps calling ouyaSDK.initOuyaPlugin so that the plugin is only initialized once.

```lua
local ouya = require("plugin.ouya") -- load the ouya plugin

callbacksInitOuyaPlugin = require "callbacksInitOuyaPlugin"
local json = require "json"
plugin_ouya = require "plugin_ouya"

if nil ~= ouyaSDK and nil ~= ouyaSDK.initOuyaPlugin then
	local data = {
	[1] = {
	    ["key"] = "tv.ouya.developer_id",
	    ["value"] = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0" -- YOUR DEVELOPER ID from the dev portal
	}};
	local jsonData = json.encode(data);
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


#### key.der

This is the signing key from the developer portal that goes into your Corona Assets folder (next to main.lua).


## Project Settings

### SDK

When you build using the Corona Simulator, the server automatically takes care of integrating the plugin into your project. 

All you need to do is add an entry into a `plugins` table of your `build.settings`. The following is an example of a minimal `build.settings` file:

```lua
settings =
{
	plugins =
	{
		["plugin.ouya"] =
		{
			publisherId = "tv.ouya"
			supportedPlatforms = { android=true },
		},
	},		
}
```


In addition, you must add an intent filter to the `android` table within `build.settings` so that your game icon will appear in the OUYA Play section:

```
   android =
   {
      mainIntentFilter =
      {
         categories = { "tv.ouya.intent.category.GAME" },
      }
   },
```

## Sample Code

* Enterprise samples &mdash; [https://github.com/ouya/docs/blob/master/corona.md](https://github.com/ouya/docs/blob/master/corona.md)

* In-App-Purchase sample &mdash; [https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin/](https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/InAppPurchasesPlugin)

* Virtual Controller sample &mdash; [https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/VirtualControllerPlugin/](https://github.com/ouya/ouya-sdk-examples/tree/master/Corona/Submission/ouya/samples/VirtualControllerPlugin)


## Support

Support is available from the OUYA team, online hangouts, and the community forums:

* [http://www.ouya.tv](http://www.ouya.tv)
* [OUYA Forums](http://forums.ouya.tv/categories/corona-on-ouya)
* [Corona Forums](http://forums.coronalabs.com/forum/627-ouya/)
* [OUYA Office Hours](https://github.com/ouya/docs/blob/master/developer_support.md)
* [tgraupmann@ouya.tv](mailto:tgraupmann@ouya.tv)
