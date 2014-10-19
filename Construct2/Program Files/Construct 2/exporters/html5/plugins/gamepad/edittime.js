function GetPluginSettings()
{
	return {
		"name":			"Gamepad",				// as appears in 'insert object' dialog, can be changed as long as "id" stays the same
		"id":			"gamepad",				// this is used to identify this plugin and is saved to the project; never change it
		"version":		"1.0",					// (float in x.y format) Plugin version - C2 shows compatibility warnings based on this
		"description":	"Receive input from a gamepad device, e.g. console controllers.",
		"author":		"Scirra",
		"help url":		"http://www.scirra.com/manual/143/gamepad",
		"category":		"Input",				// Prefer to re-use existing categories, but you can set anything here
		"type":			"object",				// either "world" (appears in layout and is drawn), else "object"
		"rotatable":	false,					// only used when "type" is "world".  Enables an angle property on the object.
		"flags":		0						// uncomment lines to enable flags...
						| pf_singleglobal		// exists project-wide, e.g. mouse, keyboard.  "type" must be "object".
					//	| pf_texture			// object has a single texture (e.g. tiled background)
					//	| pf_position_aces		// compare/set/get x, y...
					//	| pf_size_aces			// compare/set/get width, height...
					//	| pf_angle_aces			// compare/set/get angle (recommended that "rotatable" be set to true)
					//	| pf_appearance_aces	// compare/set/get visible, opacity...
					//	| pf_tiling				// adjusts image editor features to better suit tiled images (e.g. tiled background)
					//	| pf_animations			// enables the animations system.  See 'Sprite' for usage
					//	| pf_zorder_aces		// move to top, bottom, layer...
	};
};

////////////////////////////////////////
// Parameter types:
// AddNumberParam(label, description [, initial_string = "0"])			// a number
// AddStringParam(label, description [, initial_string = "\"\""])		// a string
// AddAnyTypeParam(label, description [, initial_string = "0"])			// accepts either a number or string
// AddCmpParam(label, description)										// combo with equal, not equal, less, etc.
// AddComboParamOption(text)											// (repeat before "AddComboParam" to add combo items)
// AddComboParam(label, description [, initial_selection = 0])			// a dropdown list parameter
// AddObjectParam(label, description)									// a button to click and pick an object type
// AddLayerParam(label, description)									// accepts either a layer number or name (string)
// AddLayoutParam(label, description)									// a dropdown list with all project layouts
// AddKeybParam(label, description)										// a button to click and press a key (returns a VK)
// AddAnimationParam(label, description)								// a string intended to specify an animation name
// AddAudioFileParam(label, description)								// a dropdown list with all imported project audio files

////////////////////////////////////////
// Conditions

// AddCondition(id,					// any positive integer to uniquely identify this condition
//				flags,				// (see docs) cf_none, cf_trigger, cf_fake_trigger, cf_static, cf_not_invertible,
//									// cf_deprecated, cf_incompatible_with_triggers, cf_looping
//				list_name,			// appears in event wizard list
//				category,			// category in event wizard list
//				display_str,		// as appears in event sheet - use {0}, {1} for parameters and also <b></b>, <i></i>
//				description,		// appears in event wizard dialog when selected
//				script_name);		// corresponding runtime function name

AddCondition(1, cf_trigger, "On gamepad connected", "Gamepads", "On gamepad connected", "Triggered when a gamepad device is connected to the computer.", "OnGamepadConnected");

AddCondition(2, cf_trigger, "On gamepad disconnected", "Gamepads", "On gamepad disconnected", "Triggered when a gamepad device is disconnected from the computer.", "OnGamepadDisconnected");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddComboParamOption("Button A/OUYA O");
AddComboParamOption("Button B/OUYA U");
AddComboParamOption("Button X/OUYA Y");
AddComboParamOption("Button Y/OUYA A");
AddComboParamOption("Left shoulder button/OUYA L1");
AddComboParamOption("Right shoulder button/OUYA R1");
AddComboParamOption("Left shoulder trigger/OUYA L2");
AddComboParamOption("Right shoulder trigger/OUYA R2");
AddComboParamOption("Back / View");
AddComboParamOption("Start / Menu/OUYA MENU");
AddComboParamOption("Left analog button/OUYA L3");
AddComboParamOption("Right analog button/OUYA R3");
AddComboParamOption("D-pad up/OUYA DPAD UP");
AddComboParamOption("D-pad down/OUYA DPAD DOWN");
AddComboParamOption("D-pad left/OUYA DPAD LEFT");
AddComboParamOption("D-pad right/OUYA DPAD RIGHT");
AddComboParam("Button", "Choose a gamepad button to test.");
AddCondition(3, cf_none, "Is button down", "Input", "Gamepad {0} <b>{1}</b> is down", "Test if a gamepad button is currently held down.", "IsButtonDown");

AddCondition(4, cf_none, "Has gamepads", "Gamepads", "Has gamepads", "Test if any gamepad is connected and active. A button may need to be pressed before a gamepad counts as active.", "HasGamepads");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddComboParamOption("Left analog X axis");
AddComboParamOption("Left analog Y axis");
AddComboParamOption("Right analog X axis");
AddComboParamOption("Right analog Y axis");
AddComboParam("Axis", "Select the gamepad axis to test.");
AddCmpParam("Comparison", "How to select the axis value (ranging from -100 to 100).");
AddNumberParam("Value", "The value to compare to, from -100 to 100.");
AddCondition(5, cf_none, "Compare axis", "Input", "Gamepad {0} <b>{1}</b> {2} {3}", "Test the value of one of the gamepad's axes.", "CompareAxis");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddComboParamOption("Button A/OUYA O");
AddComboParamOption("Button B/OUYA U");
AddComboParamOption("Button X/OUYA Y");
AddComboParamOption("Button Y/OUYA A");
AddComboParamOption("Left shoulder button/OUYA L1");
AddComboParamOption("Right shoulder button/OUYA R1");
AddComboParamOption("Left shoulder trigger/OUYA L2");
AddComboParamOption("Right shoulder trigger/OUYA R2");
AddComboParamOption("Back / View");
AddComboParamOption("Start / Menu/OUYA MENU");
AddComboParamOption("Left analog button/OUYA L3");
AddComboParamOption("Right analog button/OUYA R3");
AddComboParamOption("D-pad up/OUYA DPAD UP");
AddComboParamOption("D-pad down/OUYA DPAD DOWN");
AddComboParamOption("D-pad left/OUYA DPAD LEFT");
AddComboParamOption("D-pad right/OUYA DPAD RIGHT");
AddComboParam("Button", "Choose a gamepad button to test.");
AddCondition(6, cf_fake_trigger, "On button pressed", "Input", "On gamepad {0} <b>{1}</b> pressed", "Triggered when a gamepad button is pressed.", "OnButtonDown");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddComboParamOption("Button A/OUYA O");
AddComboParamOption("Button B/OUYA U");
AddComboParamOption("Button X/OUYA Y");
AddComboParamOption("Button Y/OUYA A");
AddComboParamOption("Left shoulder button/OUYA L1");
AddComboParamOption("Right shoulder button/OUYA R1");
AddComboParamOption("Left shoulder trigger/OUYA L2");
AddComboParamOption("Right shoulder trigger/OUYA R2");
AddComboParamOption("Back / View");
AddComboParamOption("Start / Menu/OUYA MENU");
AddComboParamOption("Left analog button/OUYA L3");
AddComboParamOption("Right analog button/OUYA R3");
AddComboParamOption("D-pad up/OUYA DPAD UP");
AddComboParamOption("D-pad down/OUYA DPAD DOWN");
AddComboParamOption("D-pad left/OUYA DPAD LEFT");
AddComboParamOption("D-pad right/OUYA DPAD RIGHT");
AddComboParam("Button", "Choose a gamepad button to test.");
AddCondition(7, cf_fake_trigger, "On button released", "Input", "On gamepad {0} <b>{1}</b> released", "Triggered when a gamepad button is released.", "OnButtonUp");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddCondition(8, cf_fake_trigger, "On any button pressed", "Input", "On gamepad {0} any button pressed", "Triggered when any button on a gamepad is pressed.", "OnAnyButtonDown");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddCondition(9, cf_fake_trigger, "On any button released", "Input", "On gamepad {0} any button released", "Triggered when any button on a gamepad is released.", "OnAnyButtonUp");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddNumberParam("Button index", "Enter the zero-based index of the button to test if down.");
AddCondition(10, cf_none, "Is button index down", "Input", "Gamepad {0} button <b>{1}</b> is down", "Test if a gamepad button is currently held down, by its numerical index.", "IsButtonIndexDown");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddNumberParam("Button index", "Enter the zero-based index of the button to test if pressed.");
AddCondition(11, cf_fake_trigger, "On button index pressed", "Input", "On gamepad {0} button <b>{1}</b> pressed", "Triggered when a gamepad button is pressed, by its numerical index.", "OnButtonIndexDown");

AddNumberParam("Gamepad", "Enter the zero-based index of the gamepad to test.");
AddNumberParam("Button index", "Enter the zero-based index of the button to test if released.");
AddCondition(12, cf_fake_trigger, "On button index released", "Input", "On gamepad {0} button <b>{1}</b> released", "Triggered when a gamepad button is released, by its numerical index.", "OnButtonIndexUp");

AddCondition(13, cf_none, "Gamepads are supported", "Gamepads", "Gamepads are supported", "True if the current platform or browser supports gamepad input.", "SupportsGamepad");


////////////////////////////////////////
// Actions

// AddAction(id,				// any positive integer to uniquely identify this action
//			 flags,				// (see docs) af_none, af_deprecated
//			 list_name,			// appears in event wizard list
//			 category,			// category in event wizard list
//			 display_str,		// as appears in event sheet - use {0}, {1} for parameters and also <b></b>, <i></i>
//			 description,		// appears in event wizard dialog when selected
//			 script_name);		// corresponding runtime function name

// example
//AddStringParam("Message", "Enter a string to alert.");
//AddAction(0, af_none, "Alert", "My category", "Alert {0}", "Description for my action!", "MyAction");

////////////////////////////////////////
// Expressions

// AddExpression(id,			// any positive integer to uniquely identify this expression
//				 flags,			// (see docs) ef_none, ef_deprecated, ef_return_number, ef_return_string,
//								// ef_return_any, ef_variadic_parameters (one return flag must be specified)
//				 list_name,		// currently ignored, but set as if appeared in event wizard
//				 category,		// category in expressions panel
//				 exp_name,		// the expression name after the dot, e.g. "foo" for "myobject.foo" - also the runtime function name
//				 description);	// description in expressions panel

AddExpression(0, ef_return_number, "Gamepad count", "Gamepad", "GamepadCount", "The number of currently connected and active gamepads.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(1, ef_return_string, "Gamepad ID", "Gamepad", "GamepadID", "A string identifying the gamepad, e.g. manufacturer and model.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(2, ef_deprecated | ef_return_string, "Gamepad axes", "Gamepad", "GamepadAxes", "(DEBUG) String listing all axis data");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(3, ef_deprecated | ef_return_string, "Gamepad buttons", "Gamepad", "GamepadButtons", "(DEBUG) String listing all button data");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddNumberParam("Index", "Raw button index.");
AddExpression(4, ef_return_number, "Raw button input", "Raw input", "RawButton", "Return raw button input from 0 to 100 for a button index.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddNumberParam("Index", "Raw axis index.");
AddExpression(5, ef_return_number, "Raw axis input", "Raw input", "RawAxis", "Return raw axis input from -100 to 100 for an axis index.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(6, ef_return_number, "Raw button count", "Raw input", "RawButtonCount", "Return number of buttons in raw input.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(7, ef_return_number, "Raw axis count", "Raw input", "RawAxisCount", "Return number of axes in raw input.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddNumberParam("Index", "Button index.");
AddExpression(8, ef_return_number, "Button input", "Gamepad", "Button", "Return button input from 0 to 100 for a button index.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddNumberParam("Index", "Axis index.");
AddExpression(9, ef_return_number, "Axis input", "Gamepad", "Axis", "Return axis input from -100 to 100 for an axis index.");

AddNumberParam("Gamepad", "Index of the gamepad.");
AddExpression(10, ef_return_number, "", "Gamepad", "LastButton", "Return index of last gamepad button pressed or released.");

////////////////////////////////////////
ACESDone();

////////////////////////////////////////
// Array of property grid properties for this plugin
// new cr.Property(ept_integer,		name,	initial_value,	description)		// an integer value
// new cr.Property(ept_float,		name,	initial_value,	description)		// a float value
// new cr.Property(ept_text,		name,	initial_value,	description)		// a string
// new cr.Property(ept_color,		name,	initial_value,	description)		// a color dropdown
// new cr.Property(ept_font,		name,	"Arial,-16", 	description)		// a font with the given face name and size
// new cr.Property(ept_combo,		name,	"Item 1",		description, "Item 1|Item 2|Item 3")	// a dropdown list (initial_value is string of initially selected item)
// new cr.Property(ept_link,		name,	link_text,		description, "firstonly")		// has no associated value; simply calls "OnPropertyChanged" on click

var property_list = [
	new cr.Property(ept_float,		"Analog deadzone", 25,	"Analog stick values (0-100) below this range are returned as zero to smooth the input.")
	];

// Called by IDE when a new object type is to be created
function CreateIDEObjectType()
{
	return new IDEObjectType();
}

// Class representing an object type in the IDE
function IDEObjectType()
{
	assert2(this instanceof arguments.callee, "Constructor called as a function");
}

// Called by IDE when a new object instance of this type is to be created
IDEObjectType.prototype.CreateInstance = function(instance)
{
	return new IDEInstance(instance);
}

// Class representing an individual instance of an object in the IDE
function IDEInstance(instance, type)
{
	assert2(this instanceof arguments.callee, "Constructor called as a function");

	// Save the constructor parameters
	this.instance = instance;
	this.type = type;

	// Set the default property values from the property table
	this.properties = {};

	for (var i = 0; i < property_list.length; i++)
		this.properties[property_list[i].name] = property_list[i].initial_value;

	// Plugin-specific variables
	// this.myValue = 0...
}

// Called when inserted via Insert Object Dialog for the first time
IDEInstance.prototype.OnInserted = function()
{
}

// Called when double clicked in layout
IDEInstance.prototype.OnDoubleClicked = function()
{
}

// Called after a property has been changed in the properties bar
IDEInstance.prototype.OnPropertyChanged = function(property_name)
{
}

// For rendered objects to load fonts or textures
IDEInstance.prototype.OnRendererInit = function(renderer)
{
}

// Called to draw self in the editor if a layout object
IDEInstance.prototype.Draw = function(renderer)
{
}

// For rendered objects to release fonts or textures
IDEInstance.prototype.OnRendererReleased = function(renderer)
{
}