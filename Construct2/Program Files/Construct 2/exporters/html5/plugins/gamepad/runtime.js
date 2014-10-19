// ECMAScript 5 strict mode
"use strict";

assert2(cr, "cr namespace not created");
assert2(cr.plugins_, "cr.plugins_ not created");

/////////////////////////////////////
// Plugin class
cr.plugins_.gamepad = function(runtime)
{
	this.runtime = runtime;
};

(function ()
{
	var pluginProto = cr.plugins_.gamepad.prototype;

	var isSupported = false;

	/////////////////////////////////////
	// Object type class
	pluginProto.Type = function(plugin)
	{
		this.plugin = plugin;
		this.runtime = plugin.runtime;

		isSupported = !!(navigator["getGamepads"] || navigator["webkitGetGamepads"] || navigator["mozGetGamepads"] || navigator["gamepads"] || navigator["webkitGamepads"] || navigator["MozGamepads"] || window["cr_getGamepads"]);
	};

	var typeProto = pluginProto.Type.prototype;

	// called on startup for each object type
	typeProto.onCreate = function()
	{
	};

	var gamepadRuntime = null;
	var gamepadInstance = null;
	var controllers = new Array(16);
	var osToken = "";
	var browserToken = "";

	/////////////////////////////////////
	// Control mappings
	// ctrlmap[osToken][browserToken][controllertoken] = function (index, isAxis, value)
	// Reference control system (which everything else maps to) is Windows/Chrome/XBox 360
	// See http://www.scirra.com/forum/topic49061.html
	var axisOffset = 16;
	var curCtrlMap = null;
	var ctrlmap = {};
	ctrlmap["windows"] = {};
	ctrlmap["windows"]["firefox"] = {};

	function doControllerMapping(index, isAxis, buttonmap, axismap)
	{
		if (isAxis)
		{
			if (index >= axismap.length)
				return -1;			// unknown axis

			if (cr.is_number(axismap[index]))
				return axismap[index] + axisOffset;
			else
			{
				return axismap[index];	// returning array
			}
		}
		else
		{
			if (index >= buttonmap.length)
				return -1;			// unknown button

			return buttonmap[index];
		}
	};

	// How to write a mapping:
	// Construct 2 normalises all controller states to the format used by the XBox 360 controller
	// on Chrome/Windows.  Everything is mapped to a single 20-element array in the format:
	// 0: A								10: Left analog button
	// 1: B								11: Right analog button
	// 2: X								12: D-pad up
	// 3: Y								13: D-pad down
	// 4: Left shoulder button			14: D-pad left
	// 5: Right shoulder button			15: D-pad right
	// 6: Left shoulder trigger			16: Left analog X axis
	// 7: Right shoulder trigger		17: Left analog Y axis
	// 8: Back							18: Right analog X axis
	// 9: Start							19: Right analog Y axis
	// Since different controllers return buttons and axes in a different order on different
	// browsers and OSs, all combinations need to be mapped to the above 20-element array,
	// called the 'c2state'.
	// The buttons mapping array translates a raw button input to the c2state, and the axis
	// mapping array translates a raw axis input to the range 0, 1, 2 or 3 (automatically offset
	// by 16 to fit in to the c2state).  However, sometimes an axis is mapped to a pair of buttons,
	// e.g. for the D-pad coming up as an axis.  In this case the mapping is another array
	// of the buttons to map the axis to; the first element for the button when axis negative,
	// and the second element for the button when the axis positive.  e.g. XBox 360 on Firefox/Windows
	// has axis 5 mapped to D-pad left and right; the entry is [14, 15] to map negative (left) to
	// c2state 14 (D-pad left), and positive (right) to c2state 15 (D-pad right).
	// If no mapping exists for an OS/browser/controller configuration, it defaults to assuming it's
	// the same as Windows/Chrome/XBox 360, done by the defaultMap function.

	var win_ff_xbox360_buttons = [0, 1, 2, 3, 4, 5, 8, 9, 10, 11];
	var win_ff_xbox360_axes    = [0, 1, [7, 6], 2, 3, [14, 15], [12, 13]];
	ctrlmap["windows"]["firefox"]["xbox360"] = function (index, isAxis)
	{
		return doControllerMapping(index, isAxis, win_ff_xbox360_buttons, win_ff_xbox360_axes);
	};

	var win_ff_lda_buttons = [2, 0, 1, 3, 4, 6, 5, 7, 8, 9];
	var win_ff_lda_axes    = [0, 1, 2, 3, [14, 15], [12, 13]];
	ctrlmap["windows"]["firefox"]["logitechdualaction"] = function (index, isAxis)
	{
		return doControllerMapping(index, isAxis, win_ff_lda_buttons, win_ff_lda_axes);
	};

	function defaultMap(index, isAxis)
	{
		if (isAxis)
		{
			if (index >= 4)
				return -1;		// unknown axis

			return index + axisOffset;
		}
		else
		{
			if (index >= 16)
				return -1;		// unknown button

			return index;
		}
	};

	function getMapper(id_)
	{
		if (!curCtrlMap)
			return defaultMap;

		var controllertoken = "";
		if (id_.toLowerCase != undefined) {
			var id = id_.toLowerCase();

			if (id.indexOf("xbox 360") > -1)
				controllertoken = "xbox360";
			else if (id.indexOf("logitech dual action") > -1)
				controllertoken = "logitechdualaction";
		}

		var curmap = curCtrlMap[controllertoken];

		return curmap || defaultMap;
	};

	/////////////////////////////////////

	function onConnected(e)
	{
		controllers[e["gamepad"]["index"]] = e["gamepad"];
		gamepadRuntime.trigger(cr.plugins_.gamepad.prototype.cnds.OnGamepadConnected, gamepadInstance);
	};

	function onDisconnected(e)
	{
		gamepadRuntime.trigger(cr.plugins_.gamepad.prototype.cnds.OnGamepadDisconnected, gamepadInstance);
		controllers[e["gamepad"]["index"]] = null;
	};

	/////////////////////////////////////
	// Instance class
	pluginProto.Instance = function(type)
	{
		this.type = type;
		this.runtime = type.runtime;
		gamepadRuntime = this.runtime;
		gamepadInstance = this;
	};

	var instanceProto = pluginProto.Instance.prototype;

	// called whenever an instance is created
	instanceProto.onCreate = function()
	{
		this.deadzone = this.properties[0];
		this.lastButton = 0;

		var userAgent = navigator.userAgent;

		// Check PC vs Mac
		osToken = "windows";

		if (/mac/i.test(userAgent))
			osToken = "mac";

		curCtrlMap = ctrlmap[osToken];

		// Check Chrome vs Firefox
		browserToken = "chrome";

		if (/firefox/i.test(userAgent))
			browserToken = "firefox";

		if (curCtrlMap)
			curCtrlMap = curCtrlMap[browserToken];

		window.addEventListener("webkitgamepadconnected", onConnected, false);
		window.addEventListener("webkitgamepaddisconnected", onDisconnected, false);
		window.addEventListener("MozGamepadConnected", onConnected, false);
		window.addEventListener("MozGamepadDisconnected", onDisconnected, false);
		window.addEventListener("gamepadconnected", onConnected, false);
		window.addEventListener("gamepaddisconnected", onDisconnected, false);

		this.runtime.tickMe(this);

		this.activeControllers = [];
	};

	instanceProto.tick = function ()
	{
		this.activeControllers.length = 0;

		var gamepads = null;
		var synthetic = false;

		if (navigator["getGamepads"])
			gamepads = navigator["getGamepads"]();
		else if (navigator["webkitGetGamepads"])
			gamepads = navigator["webkitGetGamepads"]();
		else if (navigator["mozGetGamepads"])
			gamepads = navigator["mozGetGamepads"]();
		else if (navigator["msGetGamepads"])
			gamepads = navigator["msGetGamepads"]();
		else if (this.runtime.isWindows8Capable && window["cr_getGamepads"])
		{
			gamepads = window["cr_getGamepads"]();
			synthetic = true;
		}
		else
			gamepads = navigator["gamepads"] || navigator["webkitGamepads"] || navigator["MozGamepads"] || controllers;

		if (!gamepads)
			return;

		var i, len, j, lenj, mapfunc, index, value;
		for (i = 0, len = gamepads.length; i < len; i++)
		{
			var pad = gamepads[i];

			if (!pad)
				continue;

			// Save controller state
			if (!pad.c2state)
			{
				pad.c2state = new Array(20);
				pad.c2oldstate = new Array(20);

				for (j = 0; j < 20; j++)
					pad.c2oldstate[j] = 0;
			}
			else
			{
				// Get previous tick's state
				for (j = 0; j < 20; j++)
					pad.c2oldstate[j] = pad.c2state[j];
			}

			mapfunc = (synthetic ? defaultMap : getMapper(pad.id));

			for (j = 0, lenj = pad["buttons"].length; j < lenj; j++)
			{
				if (pad != undefined &&
					pad["buttons"] != undefined &&
					pad["buttons"][j] != undefined &&
					typeof pad["buttons"][j]["value"] !== "undefined")
					value = pad["buttons"][j]["value"];
				else
					value = pad["buttons"][j];

				index = mapfunc(j, false, value);

				if (index >= 0 && index < 20)
				{
					pad.c2state[index] = value * 100;

					// Ensure lastButton is always set to a useful value even if no conditions used
					if (pad.c2state[index] >= 50 && pad.c2oldstate[index] < 50)
						this.lastButton = index;
				}
			}

			for (j = 0, lenj = pad["axes"].length; j < lenj; j++)
			{
				value = pad["axes"][j];
				index = mapfunc(j, true, value);

				if (cr.is_number(index))
				{
					if (index >= 0 && index < 20)
						pad.c2state[index] = value * 100;
				}
				else
				{
					// returned array: map to buttons
					pad.c2state[index[0]] = 0;
					pad.c2state[index[1]] = 0;

					if (value <= 0)
						pad.c2state[index[0]] = Math.abs(value * 100);
					else
						pad.c2state[index[1]] = Math.abs(value * 100);
				}
			}

			this.activeControllers.push(pad);
		}
	};

	instanceProto.saveToJSON = function ()
	{
		return { "lastButton": this.lastButton };
	};

	instanceProto.loadFromJSON = function (o)
	{
		this.lastButton = o["lastButton"];
	};

	/**BEGIN-PREVIEWONLY**/
	instanceProto.getDebuggerValues = function (propsections)
	{
		var i, len, count = 0;
		for (i = 0, len = controllers.length; i < len; ++i)
		{
			if (controllers[i])
				count++;
		}

		propsections.push({
			"title": "Gamepad",
			"properties": [
				{"name": "Last button", "value": this.lastButton, "readonly": true}
			]
		});
	};
	/**END-PREVIEWONLY**/

	//////////////////////////////////////
	// Conditions
	function Cnds() {};

	Cnds.prototype.SupportsGamepad = function ()
	{
		return isSupported;
	};

	Cnds.prototype.OnGamepadConnected = function ()
	{
		return true;
	};

	Cnds.prototype.OnGamepadDisconnected = function ()
	{
		return true;
	};

	Cnds.prototype.IsButtonDown = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		var ret = pad.c2state[button] >= 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	Cnds.prototype.OnButtonDown = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		// Is down this tick and was not down last tick
		var ret = pad.c2state[button] >= 50 && pad.c2oldstate[button] < 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	Cnds.prototype.OnButtonUp = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		// Is not down this tick and was down last tick
		var ret = pad.c2state[button] < 50 && pad.c2oldstate[button] >= 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	Cnds.prototype.HasGamepads = function ()
	{
		return this.activeControllers.length > 0;
	};

	Cnds.prototype.CompareAxis = function (gamepad, axis, comparison, value)
	{
		gamepad = Math.floor(gamepad);
		axis = Math.floor(axis);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		var axisvalue = pad.c2state[axis + axisOffset];

		// To implement a radial deadzone, we need to take in to account the
		// other axis value. Even values are X axis and odd values are Y axis,
		// so use this to get the other axis.
		var othervalue = 0;

		if (axis % 2 === 0)										// is X axis
			othervalue = pad.c2state[axis + axisOffset + 1];	// get next axis (Y)
		else
			othervalue = pad.c2state[axis + axisOffset - 1];	// get previous axis (X)

		// Is within the deadzone radius: return 0
		if (Math.sqrt(axisvalue * axisvalue + othervalue * othervalue) <= this.deadzone)
			axisvalue = 0;

		return cr.do_cmp(axisvalue, comparison, value);
	};

	Cnds.prototype.OnAnyButtonDown = function (gamepad)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		// Is any button down this tick and was not down last tick
		var i, len;
		for (i = 0, len = pad.c2state.length; i < len; i++)
		{
			if (pad.c2state[i] >= 50 && pad.c2oldstate[i] < 50)
			{
				this.lastButton = i;
				return true;
			}
		}

		return false;
	};

	Cnds.prototype.OnAnyButtonUp = function (gamepad)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		// Is any button up this tick and was down last tick
		var i, len;
		for (i = 0, len = pad.c2state.length; i < len; i++)
		{
			if (pad.c2state[i] < 50 && pad.c2oldstate[i] >= 50)
			{
				this.lastButton = i;
				return true;
			}
		}

		return false;
	};

	Cnds.prototype.IsButtonIndexDown = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		button = Math.floor(button);

		if (button < 0 || button >= pad.c2state.length)
			return false;

		var ret = pad.c2state[button] >= 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	Cnds.prototype.OnButtonIndexDown = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		button = Math.floor(button);

		if (button < 0 || button >= pad.c2state.length)
			return false;

		// Is down this tick and was not down last tick
		var ret = pad.c2state[button] >= 50 && pad.c2oldstate[button] < 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	Cnds.prototype.OnButtonIndexUp = function (gamepad, button)
	{
		gamepad = Math.floor(gamepad);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
			return false;

		var pad = this.activeControllers[gamepad];

		if (!pad.c2state)
			return false;

		button = Math.floor(button);

		if (button < 0 || button >= pad.c2state.length)
			return false;

		// Is not down this tick and was down last tick
		var ret = pad.c2state[button] < 50 && pad.c2oldstate[button] >= 50;

		if (ret)
			this.lastButton = button;

		return ret;
	};

	pluginProto.cnds = new Cnds();

	//////////////////////////////////////
	// Actions
	function Acts() {};
	pluginProto.acts = new Acts();

	//////////////////////////////////////
	// Expressions
	function Exps() {};

	Exps.prototype.GamepadCount = function (ret)
	{
		ret.set_int(this.activeControllers.length);
	};

	Exps.prototype.GamepadID = function (ret, index)
	{
		if (index < 0 || index >= this.activeControllers.length)
		{
			ret.set_string("");
			return;
		}

		ret.set_string(this.activeControllers[index].id);
	};

	Exps.prototype.GamepadAxes = function (ret, index)
	{
		if (index < 0 || index >= this.activeControllers.length)
		{
			ret.set_string("");
			return;
		}

		var axes = this.activeControllers[index]["axes"];
		var str = "";

		var i, len;
		for (i = 0, len = axes.length; i < len; i++)
		{
			str += "Axis " + i + ": " + Math.round(axes[i] * 100) + "\n";
		}

		ret.set_string(str);
	};

	Exps.prototype.GamepadButtons = function (ret, index)
	{
		if (index < 0 || index >= this.activeControllers.length)
		{
			ret.set_string("");
			return;
		}

		var buttons = this.activeControllers[index]["buttons"];
		var str = "";

		var i, len, value;
		for (i = 0, len = buttons.length; i < len; i++)
		{
			if (typeof buttons[i]["value"] !== "undefined")
				value = buttons[i]["value"];
			else
				value = buttons[i];

			str += "Button " + i + ": " + Math.round(value * 100) + "\n";
		}

		ret.set_string(str);
	};

	Exps.prototype.RawButton = function (ret, gamepad, index)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_float(0);
			return;
		}

		var state = this.activeControllers[gamepad]["buttons"];

		if (!state || index < 0 || index >= state.length)
		{
			ret.set_float(0);
			return;
		}

		if (typeof state[index]["value"] !== "undefined")
			ret.set_float(state[index]["value"]);
		else
			ret.set_float(state[index]);
	};

	Exps.prototype.RawAxis = function (ret, gamepad, index)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_float(0);
			return;
		}

		var state = this.activeControllers[gamepad]["axes"];

		if (!state || index < 0 || index >= state.length)
		{
			ret.set_float(0);
			return;
		}

		ret.set_float(state[index]);
	};

	Exps.prototype.RawButtonCount = function (ret, gamepad)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_int(0);
			return;
		}

		ret.set_int(this.activeControllers[gamepad]["buttons"].length);
	};

	Exps.prototype.RawAxisCount = function (ret, gamepad)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_int(0);
			return;
		}

		ret.set_int(this.activeControllers[gamepad]["axes"].length);
	};

	Exps.prototype.Button = function (ret, gamepad, index)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_float(0);
			return;
		}

		var state = this.activeControllers[gamepad].c2state;

		if (!state || index < 0 || index >= axisOffset)
		{
			ret.set_float(0);
			return;
		}

		ret.set_float(state[index]);
	};

	Exps.prototype.Axis = function (ret, gamepad, index)
	{
		gamepad = Math.floor(gamepad);
		index = Math.floor(index);

		if (gamepad < 0 || gamepad >= this.activeControllers.length)
		{
			ret.set_float(0);
			return;
		}

		var state = this.activeControllers[gamepad].c2state;

		if (!state || index < 0 || index >= 4)
		{
			ret.set_float(0);
			return;
		}

		var value = state[index + axisOffset];

		// To implement a radial deadzone, we need to take in to account the
		// other axis value. Even values are X axis and odd values are Y axis,
		// so use this to get the other axis.
		var othervalue = 0;

		if (index % 2 === 0)								// is X axis
			othervalue = state[index + axisOffset + 1];		// get next axis (Y)
		else
			othervalue = state[index + axisOffset - 1];		// get previous axis (X)

		// Is within the deadzone radius: return 0
		if (Math.sqrt(value * value + othervalue * othervalue) <= this.deadzone)
			value = 0;

		ret.set_float(value);
	};

	Exps.prototype.LastButton = function (ret)
	{
		ret.set_int(this.lastButton);
	};

	pluginProto.exps = new Exps();

}());