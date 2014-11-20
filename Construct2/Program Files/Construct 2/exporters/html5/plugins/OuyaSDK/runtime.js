// ECMAScript 5 strict mode
"use strict";

// define the OuyaSDK for ContentShell communication

// start of OUYA SDK

	var OuyaSDK = Object();

	var OuyaController = {
		AXIS_LS_X: 0,
		AXIS_LS_Y: 1,
		AXIS_RS_X: 11,
		AXIS_RS_Y: 14,
		AXIS_L2: 17,
		AXIS_R2: 18,
		BUTTON_O: 96,
		BUTTON_U: 99,
		BUTTON_Y: 100,
		BUTTON_A: 97,
		BUTTON_L1: 102,
		BUTTON_R1: 103,
		BUTTON_L3: 106,
		BUTTON_R3: 107,
		BUTTON_DPAD_UP: 19,
		BUTTON_DPAD_DOWN: 20,
		BUTTON_DPAD_RIGHT: 22,
		BUTTON_DPAD_LEFT: 21,
		BUTTON_MENU: 82,
		MAX_CONTROLLERS: 4};

	function createAxis(index) {
	   var axis = 0;
	   axis.value = 0;
	   return axis;
	}

	function createButton(index) {
	   var button = Object();
	   button.id = index;
	   button.index = index;
	   button.value = false;
	   return button;
	}

	function createGamepad(index) {
	   var pad = Object();
	   pad.id = index;
	   pad.index = index;
	   pad.axes = Array(createAxis(0), createAxis(1), createAxis(2), createAxis(3), createAxis(4), createAxis(5), createAxis(6), createAxis(7), createAxis(8), createAxis(9),
	   	createAxis(10), createAxis(11), createAxis(12), createAxis(13), createAxis(14), createAxis(15), createAxis(16), createAxis(17), createAxis(18), createAxis(19));
	   pad.buttons = Array(createButton(0), createButton(1), createButton(2), createButton(3), createButton(4), createButton(5), createButton(6), createButton(7), createButton(8), createButton(9),
	   	createButton(10), createButton(11), createButton(12), createButton(13), createButton(14), createButton(15), createButton(16), createButton(17), createButton(18), createButton(19));
	   return pad;
	}

	function createGamepads() {
		return Array(createGamepad(0), createGamepad(1), createGamepad(2), createGamepad(3));
	}

	var gamepads = Array();

	if (navigator != undefined &&
		navigator.userAgent != undefined &&
		navigator.userAgent == "Mozilla/5.0 (Linux; Android 4.1.2; OUYA Console Build/JZO54L-OUYA) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/19.77.34.5 Mobile Safari/537.36") {
		gamepads = createGamepads();
		navigator["getGamepads"] = function () { return gamepads; }
	}

	var onGenericMotionEvent = function (playerNum, axis, val) {
		var newAxis = axis;
		switch (axis)
		{
		case OuyaController.AXIS_LS_X:
			newAxis = 0;
			break;
		case OuyaController.AXIS_LS_Y:
			newAxis = 1;
			break;
		case OuyaController.AXIS_RS_X:
			newAxis = 3;
			break;
		case OuyaController.AXIS_RS_Y:
			newAxis = 4;
			break;
		case OuyaController.AXIS_L2:
			newAxis = 2;
			break;
		case OuyaController.AXIS_R2:
			newAxis = 5;
			break;
		}
		gamepads[playerNum].axes[newAxis] = val;
		gamepads[playerNum].axes[newAxis].value = val;
	}

	var onKeyDown = function (playerNum, button) {
		var newButton = button;
		switch (button)
		{
		case OuyaController.BUTTON_O:
			newButton = 0;
			break;
		case OuyaController.BUTTON_U:
			newButton = 1;
			break;
		case OuyaController.BUTTON_Y:
			newButton = 2;
			break;
		case OuyaController.BUTTON_A:
			newButton = 3;
			break;
		case OuyaController.BUTTON_L1:
			newButton = 4;
			break;
		case OuyaController.BUTTON_R1:
			newButton = 5;
			break;
		case OuyaController.BUTTON_L3:
			newButton = 10;
			break;
		case OuyaController.BUTTON_R3:
			newButton = 11;
			break;
		case OuyaController.BUTTON_DPAD_UP:
			newButton = 12;
			break;
		case OuyaController.BUTTON_DPAD_DOWN:
			newButton = 13;
			break;
		case OuyaController.BUTTON_DPAD_LEFT:
			newButton = 14;
			break;
		case OuyaController.BUTTON_DPAD_RIGHT:
			newButton = 15;
			break;
		case OuyaController.BUTTON_MENU:
			newButton = 9;
			gamepads[playerNum].buttons[newButton] = true;
			setTimeout(function() { gamepads[playerNum].buttons[newButton] = false; }, 1000);
			return;
		}
		gamepads[playerNum].buttons[newButton] = true;
	}

	var onKeyUp = function (playerNum, button) {
		var newButton = button;
		switch (button)
		{
		case OuyaController.BUTTON_O:
			newButton = 0;
			break;
		case OuyaController.BUTTON_U:
			newButton = 1;
			break;
		case OuyaController.BUTTON_Y:
			newButton = 2;
			break;
		case OuyaController.BUTTON_A:
			newButton = 3;
			break;
		case OuyaController.BUTTON_L1:
			newButton = 4;
			break;
		case OuyaController.BUTTON_R1:
			newButton = 5;
			break;
		case OuyaController.BUTTON_L3:
			newButton = 10;
			break;
		case OuyaController.BUTTON_R3:
			newButton = 11;
			break;
		case OuyaController.BUTTON_DPAD_UP:
			newButton = 12;
			break;
		case OuyaController.BUTTON_DPAD_DOWN:
			newButton = 13;
			break;
		case OuyaController.BUTTON_DPAD_LEFT:
			newButton = 14;
			break;
		case OuyaController.BUTTON_DPAD_RIGHT:
			newButton = 15;
			break;
		case OuyaController.BUTTON_MENU:
			//ignore
			return;
		}
		gamepads[playerNum].buttons[newButton] = false;
	}

	OuyaSDK.method = "";
	OuyaSDK.initOuyaPlugin = function(jsonData, onSuccess, onFailure) {
		OuyaSDK.initValues = jsonData;
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.method = "initOuyaPlugin";
	};
	OuyaSDK.requestGamerInfo = function(onSuccess, onFailure, onCancel) {
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.onCancel = onCancel;
		OuyaSDK.method = "requestGamerInfo";
	};
	OuyaSDK.requestProducts = function(products, onSuccess, onFailure, onCancel) {
		OuyaSDK.products = products;
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.onCancel = onCancel;
		OuyaSDK.method = "requestProducts";
	};
	OuyaSDK.requestPurchase = function(purchasable, onSuccess, onFailure, onCancel) {
		OuyaSDK.purchasable = purchasable;
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.onCancel = onCancel;
		OuyaSDK.method = "requestPurchase";
	};
	OuyaSDK.requestReceipts = function(onSuccess, onFailure, onCancel) {
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.onCancel = onCancel;
		OuyaSDK.method = "requestReceipts";
	};
	OuyaSDK.setSafeArea = function(amount, onSuccess, onFailure) {
		OuyaSDK.safeAreaAmount = amount;
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.method = "setSafeArea";
	}
	OuyaSDK.shutdown = function(onSuccess, onFailure) {
		OuyaSDK.onSuccess = onSuccess;
		OuyaSDK.onFailure = onFailure;
		OuyaSDK.method = "shutdown";
	}

// end of OUYA SDK

assert2(cr, "cr namespace not created");
assert2(cr.plugins_, "cr.plugins_ not created");

/////////////////////////////////////
// Plugin class
// *** CHANGE THE PLUGIN ID HERE *** - must match the "id" property in edittime.js
cr.plugins_.OuyaSDK = function(runtime)
{
	this.runtime = runtime;
};

(function ()
{
	/////////////////////////////////////
	// *** CHANGE THE PLUGIN ID HERE *** - must match the "id" property in edittime.js
	var pluginProto = cr.plugins_.OuyaSDK.prototype;

	/////////////////////////////////////
	// Object type class
	pluginProto.Type = function(plugin)
	{
		this.plugin = plugin;
		this.runtime = plugin.runtime;
	};

	var typeProto = pluginProto.Type.prototype;

	// called on startup for each object type
	typeProto.onCreate = function()
	{
	};

	/////////////////////////////////////
	// Instance class
	pluginProto.Instance = function(type)
	{
		this.type = type;
		this.runtime = type.runtime;

		// any other properties you need, e.g...
		// this.myValue = 0;
	};

	var instanceProto = pluginProto.Instance.prototype;

	// called whenever an instance is created
	instanceProto.onCreate = function()
	{
		// note the object is sealed after this call; ensure any properties you'll ever need are set on the object
		// e.g...
		// this.myValue = 0;
	};

	// called whenever an instance is destroyed
	// note the runtime may keep the object after this call for recycling; be sure
	// to release/recycle/reset any references to other objects in this function.
	instanceProto.onDestroy = function ()
	{
	};

	// called when saving the full state of the game
	instanceProto.saveToJSON = function ()
	{
		// return a Javascript object containing information about your object's state
		// note you MUST use double-quote syntax (e.g. "property": value) to prevent
		// Closure Compiler renaming and breaking the save format
		return {
			// e.g.
			//"myValue": this.myValue
		};
	};

	// called when loading the full state of the game
	instanceProto.loadFromJSON = function (o)
	{
		// load from the state previously saved by saveToJSON
		// 'o' provides the same object that you saved, e.g.
		// this.myValue = o["myValue"];
		// note you MUST use double-quote syntax (e.g. o["property"]) to prevent
		// Closure Compiler renaming and breaking the save format
	};

	// only called if a layout object - draw to a canvas 2D context
	instanceProto.draw = function(ctx)
	{
	};

	// only called if a layout object in WebGL mode - draw to the WebGL context
	// 'glw' is not a WebGL context, it's a wrapper - you can find its methods in GLWrap.js in the install
	// directory or just copy what other plugins do.
	instanceProto.drawGL = function (glw)
	{
	};

	// The comments around these functions ensure they are removed when exporting, since the
	// debugger code is no longer relevant after publishing.
	/**BEGIN-PREVIEWONLY**/
	instanceProto.getDebuggerValues = function (propsections)
	{
		// Append to propsections any debugger sections you want to appear.
		// Each section is an object with two members: "title" and "properties".
		// "properties" is an array of individual debugger properties to display
		// with their name and value, and some other optional settings.
		propsections.push({
			"title": "My debugger section",
			"properties": [
				// Each property entry can use the following values:
				// "name" (required): name of the property (must be unique within this section)
				// "value" (required): a boolean, number or string for the value
				// "html" (optional, default false): set to true to interpret the name and value
				//									 as HTML strings rather than simple plain text
				// "readonly" (optional, default false): set to true to disable editing the property

				// Example:
				// {"name": "My property", "value": this.myValue}
			]
		});
	};

	instanceProto.onDebugValueEdited = function (header, name, value)
	{
		// Called when a non-readonly property has been edited in the debugger. Usually you only
		// will need 'name' (the property name) and 'value', but you can also use 'header' (the
		// header title for the section) to distinguish properties with the same name.
		if (name === "My property")
			this.myProperty = value;
	};
	/**END-PREVIEWONLY**/

	//////////////////////////////////////
	// Conditions
	function Cnds() {};

	Cnds.prototype.onSuccessInitOuyaPlugin = function () {
		if (Acts.prototype.hasOnSuccessInitOuyaPlugin != undefined &&
			Acts.prototype.hasOnSuccessInitOuyaPlugin) {
			Acts.prototype.hasOnSuccessInitOuyaPlugin = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureInitOuyaPlugin = function () {
		if (Acts.prototype.hasOnFailureInitOuyaPlugin != undefined &&
			Acts.prototype.hasOnFailureInitOuyaPlugin) {
			Acts.prototype.hasOnFailureInitOuyaPlugin = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessRequestGamerInfo = function () {
		if (Acts.prototype.hasOnSuccessRequestGamerInfo != undefined &&
			Acts.prototype.hasOnSuccessRequestGamerInfo) {
			Acts.prototype.hasOnSuccessRequestGamerInfo = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureRequestGamerInfo = function () {
		if (Acts.prototype.hasOnFailureRequestGamerInfo != undefined &&
			Acts.prototype.hasOnFailureRequestGamerInfo) {
			Acts.prototype.hasOnFailureRequestGamerInfo = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onCancelRequestGamerInfo = function () {
		if (Acts.prototype.hasOnCancelRequestGamerInfo != undefined &&
			Acts.prototype.hasOnCancelRequestGamerInfo) {
			Acts.prototype.hasOnCancelRequestGamerInfo = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessRequestProducts = function () {
		if (Acts.prototype.hasOnSuccessRequestProducts != undefined &&
			Acts.prototype.hasOnSuccessRequestProducts) {
			Acts.prototype.hasOnSuccessRequestProducts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureRequestProducts = function () {
		if (Acts.prototype.hasOnFailureRequestProducts != undefined &&
			Acts.prototype.hasOnFailureRequestProducts) {
			Acts.prototype.hasOnFailureRequestProducts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onCancelRequestProducts = function () {
		if (Acts.prototype.hasOnCancelRequestProducts != undefined &&
			Acts.prototype.hasOnCancelRequestProducts) {
			Acts.prototype.hasOnCancelRequestProducts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessRequestPurchase = function () {
		if (Acts.prototype.hasOnSuccessRequestPurchase != undefined &&
			Acts.prototype.hasOnSuccessRequestPurchase) {
			Acts.prototype.hasOnSuccessRequestPurchase = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureRequestPurchase = function () {
		if (Acts.prototype.hasOnFailureRequestPurchase != undefined &&
			Acts.prototype.hasOnFailureRequestPurchase) {
			Acts.prototype.hasOnFailureRequestPurchase = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onCancelRequestPurchase = function () {
		if (Acts.prototype.hasOnCancelRequestPurchase != undefined &&
			Acts.prototype.hasOnCancelRequestPurchase) {
			Acts.prototype.hasOnCancelRequestPurchase = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessRequestReceipts = function () {
		if (Acts.prototype.hasOnSuccessRequestReceipts != undefined &&
			Acts.prototype.hasOnSuccessRequestReceipts) {
			Acts.prototype.hasOnSuccessRequestReceipts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureRequestReceipts = function () {
		if (Acts.prototype.hasOnFailureRequestReceipts != undefined &&
			Acts.prototype.hasOnFailureRequestReceipts) {
			Acts.prototype.hasOnFailureRequestReceipts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onCancelRequestReceipts = function () {
		if (Acts.prototype.hasOnCancelRequestReceipts != undefined &&
			Acts.prototype.hasOnCancelRequestReceipts) {
			Acts.prototype.hasOnCancelRequestReceipts = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessSetSafeArea = function () {
		if (Acts.prototype.hasOnSuccessSetSafeArea != undefined &&
			Acts.prototype.hasOnSuccessSetSafeArea) {
			Acts.prototype.hasOnSuccessSetSafeArea = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureSetSafeArea = function () {
		if (Acts.prototype.hasOnFailureSetSafeArea != undefined &&
			Acts.prototype.hasOnFailureSetSafeArea) {
			Acts.prototype.hasOnFailureSetSafeArea = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onSuccessShutdown = function () {
		if (Acts.prototype.hasOnSuccessShutdown != undefined &&
			Acts.prototype.hasOnSuccessShutdown) {
			Acts.prototype.hasOnSuccessShutdown = false;
			return true;
		}
		return false;
	}

	Cnds.prototype.onFailureShutdown = function () {
		if (Acts.prototype.hasOnFailureShutdown != undefined &&
			Acts.prototype.hasOnFailureShutdown) {
			Acts.prototype.hasOnFailureShutdown = false;
			return true;
		}
		return false;
	}


	// the example condition
	Cnds.prototype.MyCondition = function (myparam)
	{
		// return true if number is positive
		return myparam >= 0;
	};

	// ... other conditions here ...

	pluginProto.cnds = new Cnds();

	//////////////////////////////////////
	// Actions
	function Acts() {};

	Acts.prototype.ouyaInitializationValues = Array();

	Acts.prototype.addInitOuyaPluginValues = function (key, value)
	{
		Acts.prototype.ouyaInitializationValues[Acts.prototype.ouyaInitializationValues.length] =
		{
			'key': key,
			'value': value
		};
	};

	Acts.prototype.hasOnSuccessInitOuyaPlugin = false;
	Acts.prototype.onSuccessInitOuyaPlugin = function () {
		Acts.prototype.hasOnSuccessInitOuyaPlugin = true;
	}

	Acts.prototype.hasOnFailureInitOuyaPlugin = false;
	Acts.prototype.onFailureInitOuyaPlugin = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureInitOuyaPlugin = errorCode;
		Acts.prototype.errorMessageOnFailureInitOuyaPlugin = errorMessage;
		Acts.prototype.hasOnFailureInitOuyaPlugin = true;
	}

	Acts.prototype.initOuyaPlugin = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.initOuyaPlugin != undefined) {
			var jsonData = JSON.stringify(Acts.prototype.ouyaInitializationValues);
            OuyaSDK.initOuyaPlugin(jsonData, Acts.prototype.onSuccessInitOuyaPlugin, Acts.prototype.onFailureInitOuyaPlugin);
		}
	};

	Acts.prototype.onSuccessRequestGamerInfo = function (jsonData) {
		Acts.prototype.resultOnSuccessRequestGamerInfo = jsonData;
		Acts.prototype.hasOnSuccessRequestGamerInfo = true;
	}

	Acts.prototype.onFailureRequestGamerInfo = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureRequestGamerInfo = errorCode;
		Acts.prototype.errorMessageOnFailureRequestGamerInfo = errorMessage;
		Acts.prototype.hasOnFailureRequestGamerInfo = true;
	}

	Acts.prototype.onCancelRequestGamerInfo = function () {
		Acts.prototype.hasOnCancelRequestGamerInfo = true;
	}

	Acts.prototype.requestGamerInfo = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestGamerInfo != undefined) {
			OuyaSDK.requestGamerInfo(Acts.prototype.onSuccessRequestGamerInfo, Acts.prototype.onFailureRequestGamerInfo, Acts.prototype.onCancelRequestGamerInfo);
		}
	};

	Acts.prototype.hasOnSuccessRequestProducts = false;
	Acts.prototype.onSuccessRequestProducts = function (jsonData) {
		Acts.prototype.resultOnSuccessRequestProducts = jsonData;
		Acts.prototype.hasOnSuccessRequestProducts = true;
	}

	Acts.prototype.hasOnFailureRequestProducts = false;
	Acts.prototype.onFailureRequestProducts = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureRequestProducts = errorCode;
		Acts.prototype.errorMessageOnFailureRequestProducts = errorMessage;
		Acts.prototype.hasOnFailureRequestProducts = true;
	}

	Acts.prototype.hasOnCancelRequestProducts = false;
	Acts.prototype.onCancelRequestProducts = function () {
		Acts.prototype.hasOnCancelRequestProducts = true;
	}

	Acts.prototype.requestProducts = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestProducts != undefined) {
			var products = myparam.split(',');
			OuyaSDK.requestProducts(products, Acts.prototype.onSuccessRequestProducts, Acts.prototype.onFailureRequestProducts, Acts.prototype.onCancelRequestProducts);
		}
	};

	Acts.prototype.hasOnSuccessRequestPurchase = false;
	Acts.prototype.onSuccessRequestPurchase = function (jsonData) {
		Acts.prototype.resultOnSuccessRequestPurchase = jsonData;
		Acts.prototype.hasOnSuccessRequestPurchase = true;
	}

	Acts.prototype.hasOnFailureRequestPurchase = false;
	Acts.prototype.onFailureRequestPurchase = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureRequestPurchase = errorCode;
		Acts.prototype.errorMessageOnFailureRequestPurchase = errorMessage;
		Acts.prototype.hasOnFailureRequestPurchase = true;
	}

	Acts.prototype.hasOnCancelRequestPurchase = false;
	Acts.prototype.onCancelRequestPurchase = function () {
		Acts.prototype.hasOnCancelRequestPurchase = true;
	}

	Acts.prototype.requestPurchase = function (purchasable)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestPurchase != undefined &&
			purchasable != "") {
			OuyaSDK.requestPurchase(purchasable, Acts.prototype.onSuccessRequestPurchase, Acts.prototype.onFailureRequestPurchase, Acts.prototype.onCancelRequestPurchase);
		}
	};

	Acts.prototype.hasOnSuccessRequestReceipts = false;
	Acts.prototype.resultOnSuccessRequestReceipts = "";
	Acts.prototype.onSuccessRequestReceipts = function (jsonData) {
		Acts.prototype.resultOnSuccessRequestReceipts = jsonData;
		Acts.prototype.hasOnSuccessRequestReceipts = true;
	}

	Acts.prototype.hasOnFailureRequestReceipts = false;
	Acts.prototype.onFailureRequestReceipts = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureRequestReceipts = errorCode;
		Acts.prototype.errorMessageOnFailureRequestReceipts = errorMessage;
		Acts.prototype.hasOnFailureRequestReceipts = true;
	}

	Acts.prototype.hasOnCancelRequestReceipts = false;
	Acts.prototype.onCancelRequestReceipts = function () {
		Acts.prototype.hasOnCancelRequestReceipts = true;
	}

	Acts.prototype.requestReceipts = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestReceipts != undefined) {
			OuyaSDK.requestReceipts(Acts.prototype.onSuccessRequestReceipts, Acts.prototype.onFailureRequestReceipts, Acts.prototype.onCancelRequestReceipts);
		}
	};

	Acts.prototype.hasOnSuccessSetSafeArea = false;
	Acts.prototype.onSuccessSetSafeArea = function () {
		Acts.prototype.hasOnSuccessSetSafeArea = true;
	}

	Acts.prototype.hasOnFailureSetSafeArea = false;
	Acts.prototype.onFailureSetSafeArea = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureSetSafeArea = errorCode;
		Acts.prototype.errorMessageOnFailureSetSafeArea = errorMessage;
		Acts.prototype.hasOnFailureSetSafeArea = true;
	}

	Acts.prototype.setSafeArea = function (safeAreaAmount)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.setSafeArea != undefined) {
			OuyaSDK.setSafeArea(safeAreaAmount, Acts.prototype.onSuccessSetSafeArea, Acts.prototype.onFailureSetSafeArea);
		}
	};

	Acts.prototype.hasOnSuccessShutdown = false;
	Acts.prototype.onSuccessShutdown = function () {
		Acts.prototype.hasOnSuccessShutdown = true;
	}

	Acts.prototype.hasOnFailureShutdown = false;
	Acts.prototype.onFailureShutdown = function (errorCode, errorMessage) {
		Acts.prototype.errorCodeOnFailureShutdown = errorCode;
		Acts.prototype.errorMessageOnFailureShutdown = errorMessage;
		Acts.prototype.hasOnFailureShutdown = true;
	}

	Acts.prototype.shutdown = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.shutdown != undefined) {
			OuyaSDK.shutdown(Acts.prototype.onSuccessShutdown, Acts.prototype.onFailureShutdown);
		}
	};

	// the example action
	Acts.prototype.MyAction = function (myparam)
	{
		// alert the message
		alert(myparam);
	};

	// ... other actions here ...

	pluginProto.acts = new Acts();

	//////////////////////////////////////
	// Expressions
	function Exps() {};

	Exps.prototype.errorCodeOnFailureInitOuyaPlugin = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureInitOuyaPlugin != undefined) {
			result = Acts.prototype.errorCodeOnFailureInitOuyaPlugin;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureInitOuyaPlugin = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureInitOuyaPlugin != undefined) {
			result = Acts.prototype.errorMessageOnFailureInitOuyaPlugin;
		}
		ret.set_string(result);
	};

	Exps.prototype.GamerInfoUsername = function (ret)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestGamerInfo != undefined) {
			var gamerInfo = JSON.parse(Acts.prototype.resultOnSuccessRequestGamerInfo);
			result = gamerInfo.username;
		}
		ret.set_string(result);
	};

	Exps.prototype.GamerInfoUuid = function (ret)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestGamerInfo != undefined) {
			var gamerInfo = JSON.parse(Acts.prototype.resultOnSuccessRequestGamerInfo);
			result = gamerInfo.uuid;
		}
		ret.set_string(result);
	};

	Exps.prototype.errorCodeOnFailureRequestGamerInfo = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureRequestGamerInfo != undefined) {
			result = Acts.prototype.errorCodeOnFailureRequestGamerInfo;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureRequestGamerInfo = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureRequestGamerInfo != undefined) {
			result = Acts.prototype.errorMessageOnFailureRequestGamerInfo;
		}
		ret.set_string(result);
	};

	Exps.prototype.ProductsLength = function (ret)
	{
		var result = 0;
		if (Acts.prototype.resultOnSuccessRequestProducts != undefined) {
			var products = JSON.parse(Acts.prototype.resultOnSuccessRequestProducts);
			result = products.length;
		}
		ret.set_int(result);
	};

	Exps.prototype.GetProductsIdentifier = function (ret, index)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestProducts != undefined) {
			var products = JSON.parse(Acts.prototype.resultOnSuccessRequestProducts);
			if (products != undefined) {
				var product = products[index];
				if (product != undefined) {
					result = product.identifier;
				}
			}

		}
		ret.set_string(result);
	};

	Exps.prototype.GetProductsName = function (ret, index)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestProducts != undefined) {
			var products = JSON.parse(Acts.prototype.resultOnSuccessRequestProducts);
			if (products != undefined) {
				var product = products[index];
				if (product != undefined) {
					result = product.name;
				}
			}

		}
		ret.set_string(result);
	};

	Exps.prototype.GetProductsDescription = function (ret, index)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestProducts != undefined) {
			var products = JSON.parse(Acts.prototype.resultOnSuccessRequestProducts);
			if (products != undefined) {
				var product = products[index];
				if (product != undefined) {
					result = product.description;
				}
			}

		}
		ret.set_string(result);
	};

	Exps.prototype.GetProductsLocalPrice = function (ret, index)
	{
		var result = 0.0;
		if (Acts.prototype.resultOnSuccessRequestProducts != undefined) {
			var products = JSON.parse(Acts.prototype.resultOnSuccessRequestProducts);
			if (products != undefined) {
				var product = products[index];
				if (product != undefined) {
					result = product.localPrice;
				}
			}

		}
		ret.set_float(result);
	};

	Exps.prototype.errorCodeOnFailureRequestProducts = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureRequestProducts != undefined) {
			result = Acts.prototype.errorCodeOnFailureRequestProducts;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureRequestProducts = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureRequestProducts != undefined) {
			result = Acts.prototype.errorMessageOnFailureRequestProducts;
		}
		ret.set_string(result);
	};

	Exps.prototype.errorCodeOnFailureRequestPurchase = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureRequestPurchase != undefined) {
			result = Acts.prototype.errorCodeOnFailureRequestPurchase;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureRequestPurchase = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureRequestPurchase != undefined) {
			result = Acts.prototype.errorMessageOnFailureRequestPurchase;
		}
		ret.set_string(result);
	};

	Exps.prototype.ReceiptsLength = function (ret)
	{
		var result = 0;
		if (Acts.prototype.resultOnSuccessRequestReceipts != undefined) {
			var receipts = JSON.parse(Acts.prototype.resultOnSuccessRequestReceipts);
			result = receipts.length;
		}
		ret.set_int(result);
	};

	Exps.prototype.GetReceiptsIdentififer = function (ret, index)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestReceipts != undefined) {
			var receipts = JSON.parse(Acts.prototype.resultOnSuccessRequestReceipts);
			if (receipts != undefined) {
				var receipt = receipts[index];
				if (receipt != undefined) {
					result = receipt.identifier;
				}
			}

		}
		ret.set_string(result);
	};

	Exps.prototype.GetReceiptsGeneratedDate = function (ret, index)
	{
		var result = "";
		if (Acts.prototype.resultOnSuccessRequestReceipts != undefined) {
			var receipts = JSON.parse(Acts.prototype.resultOnSuccessRequestReceipts);
			if (receipts != undefined) {
				var receipt = receipts[index];
				if (receipt != undefined) {
					result = receipt.generatedDate;
				}
			}

		}
		ret.set_string(result);
	};

	Exps.prototype.GetReceiptsLocalPrice = function (ret, index)
	{
		var result = 0.0;
		if (Acts.prototype.resultOnSuccessRequestReceipts != undefined) {
			var receipts = JSON.parse(Acts.prototype.resultOnSuccessRequestReceipts);
			if (receipts != undefined) {
				var receipt = receipts[index];
				if (receipt != undefined) {
					result = receipt.localPrice;
				}
			}

		}
		ret.set_float(result);
	};

	Exps.prototype.errorCodeOnFailureRequestReceipts = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureRequestReceipts != undefined) {
			result = Acts.prototype.errorCodeOnFailureRequestReceipts;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureRequestReceipts = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureRequestReceipts != undefined) {
			result = Acts.prototype.errorMessageOnFailureRequestReceipts;
		}
		ret.set_string(result);
	};

	Exps.prototype.errorCodeOnFailureSetSafeArea = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureSetSafeArea != undefined) {
			result = Acts.prototype.errorCodeOnFailureSetSafeArea;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureSetSafeArea = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureSetSafeArea != undefined) {
			result = Acts.prototype.errorMessageOnFailureSetSafeArea;
		}
		ret.set_string(result);
	};

	Exps.prototype.errorCodeOnFailureShutdown = function (ret)
	{
		var result = -1;
		if (Acts.prototype.errorCodeOnFailureShutdown != undefined) {
			result = Acts.prototype.errorCodeOnFailureShutdown;
		}
		ret.set_int(result);
	};

	Exps.prototype.errorMessageOnFailureShutdown = function (ret)
	{
		var result = "";
		if (Acts.prototype.errorMessageOnFailureShutdown != undefined) {
			result = Acts.prototype.errorMessageOnFailureShutdown;
		}
		ret.set_string(result);
	};

	// the example expression
	Exps.prototype.MyExpression = function (ret)	// 'ret' must always be the first parameter - always return the expression's result through it!
	{
		ret.set_int(1337);				// return our value
		// ret.set_float(0.5);			// for returning floats
		// ret.set_string("Hello");		// for ef_return_string
		// ret.set_any("woo");			// for ef_return_any, accepts either a number or string
	};

	// ... other expressions here ...

	pluginProto.exps = new Exps();

}());