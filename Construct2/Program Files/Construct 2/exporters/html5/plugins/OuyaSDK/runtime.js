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

	var gamepads = createGamepads();

	navigator["getGamepads"] = function () { return gamepads; }

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

	var ouyaInitializationValues = Array();

	Acts.prototype.addInitOuyaPluginValues = function (key, value)
	{
		ouyaInitializationValues[ouyaInitializationValues.length] =
		{
			'key': 'tv.ouya.developer_id',
			'value': '310a8f51-4d6e-4ae5-bda0-b93878e5f5d0'
		};
	};

	function onSuccessInitOuyaPlugin() {
	}

	function onFailureInitOuyaPlugin(errorCode, errorMessage) {
	}

	Acts.prototype.initOuyaPlugin = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.initOuyaPlugin != undefined) {
			var jsonData = JSON.stringify(ouyaInitializationValues);
            OuyaSDK.initOuyaPlugin(jsonData, onSuccessInitOuyaPlugin, onFailureInitOuyaPlugin);
		}
	};

	function onSuccessRequestGamerInfo(jsonData) {
		var gamerInfo = JSON.parse(jsonData);
		var gamerUsername = gamerInfo.username;
		var gamerUuid = gamerInfo.uuid;
	}

	function onFailureRequestGamerInfo(errorCode, errorMessage) {
	}

	function onCancelRequestGamerInfo() {
	}

	Acts.prototype.requestGamerInfo = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestGamerInfo != undefined) {
			OuyaSDK.requestGamerInfo(onSuccessRequestGamerInfo, onFailureRequestGamerInfo, onCancelRequestGamerInfo);
		}
	};

	function onSuccessRequestProducts(jsonData) {
	}

	function onFailureRequestProducts(errorCode, errorMessage) {
	}

	function onCancelRequestProducts() {
	}

	Acts.prototype.requestProducts = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestProducts != undefined) {
			var products = Array("long_sword", "sharp_axe", "cool_level", "awesome_sauce", "__DECLINED__THIS_PURCHASE");
			OuyaSDK.requestProducts(products, onSuccessRequestProducts, onFailureRequestProducts, onCancelRequestProducts);
		}
	};

	function onSuccessRequestPurchase(jsonData) {
	}

	function onFailureRequestPurchase(errorCode, errorMessage) {
	}

	function onCancelRequestPurchase() {
	}

	Acts.prototype.requestPurchase = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestPurchase != undefined) {
			var purchasable = "long_sword";
			OuyaSDK.requestPurchase(purchasable, onSuccessRequestPurchase, onFailureRequestPurchase, onCancelRequestPurchase);
		}
	};

	function onSuccessRequestReceipts(jsonData) {
	}

	function onFailureRequestReceipts(errorCode, errorMessage) {
	}

	function onCancelRequestReceipts() {
	}

	Acts.prototype.requestReceipts = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.requestReceipts != undefined) {
			OuyaSDK.requestReceipts(onSuccessRequestReceipts, onFailureRequestReceipts, onCancelRequestReceipts);
		}
	};

	function onSuccessSetSafeArea() {
	}

	function onFailureSafeArea(errorCode, errorMessage) {
	}

	Acts.prototype.setSafeArea = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.setSafeArea != undefined) {
			var safeAreaAmount = 0.0;
			OuyaSDK.setSafeArea(safeAreaAmount, onSuccessSetSafeArea, onFailureSafeArea);
		}
	};

	function onSuccessShutdown() {
	}

	function onFailureShutdown(errorCode, errorMessage) {
	}

	Acts.prototype.shutdown = function (myparam)
	{
		if (OuyaSDK != undefined &&
			OuyaSDK.shutdown != undefined) {
			OuyaSDK.shutdown(onSuccessShutdown, onFailureShutdown);
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