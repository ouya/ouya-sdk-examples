function GetPluginSettings()
{
	return {
		"name":			"OuyaSDK",				// as appears in 'insert object' dialog, can be changed as long as "id" stays the same
		"id":			"OuyaSDK",				// this is used to identify this plugin and is saved to the project; never change it
		"version":		"1.0",					// (float in x.y format) Plugin version - C2 shows compatibility warnings based on this
		"description":	"<appears at the bottom of the insert object dialog>",
		"author":		"<your name/organisation>",
		"help url":		"<your website or a manual entry on Scirra.com>",
		"category":		"General",				// Prefer to re-use existing categories, but you can set anything here
		"type":			"world",				// either "world" (appears in layout and is drawn), else "object"
		"rotatable":	true,					// only used when "type" is "world".  Enables an angle property on the object.
		"flags":		0						// uncomment lines to enable flags...
					//	| pf_singleglobal		// exists project-wide, e.g. mouse, keyboard.  "type" must be "object".
					//	| pf_texture			// object has a single texture (e.g. tiled background)
					//	| pf_position_aces		// compare/set/get x, y...
					//	| pf_size_aces			// compare/set/get width, height...
					//	| pf_angle_aces			// compare/set/get angle (recommended that "rotatable" be set to true)
					//	| pf_appearance_aces	// compare/set/get visible, opacity...
					//	| pf_tiling				// adjusts image editor features to better suit tiled images (e.g. tiled background)
					//	| pf_animations			// enables the animations system.  See 'Sprite' for usage
					//	| pf_zorder_aces		// move to top, bottom, layer...
					//  | pf_nosize				// prevent resizing in the editor
					//	| pf_effects			// allow WebGL shader effects to be added
					//  | pf_predraw			// set for any plugin which draws and is not a sprite (i.e. does not simply draw
												// a single non-tiling image the size of the object) - required for effects to work properly
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

// example

var conditionIndex = -1;

AddNumberParam("Number", "Enter a number to test if positive.");
AddCondition(conditionIndex++, cf_none, "Initialize OUYA Plugin on Success", "Initialization", "initOuyaPlugin onSuccess", "Description for my condition!", "onSuccessInitOuyaPlugin");
AddCondition(conditionIndex++, cf_none, "Initialize OUYA Plugin on Failure", "Initialization", "initOuyaPlugin onFailure", "Description for my condition!", "onFailureInitOuyaPlugin");

AddCondition(conditionIndex++, cf_none, "Request Gamer Info on Success", "RequestGamerInfo", "requestGamerInfoOnSuccess", "Description for my condition!", "onSuccessRequestGamerInfo");
AddCondition(conditionIndex++, cf_none, "Request Gamer Info on Failure", "RequestGamerInfo", "requestGamerInfoOnFailure", "Description for my condition!", "onFailureRequestGamerInfo");
AddCondition(conditionIndex++, cf_none, "Request Gamer Info on Cancel", "RequestGamerInfo", "requestGamerInfoOnCancel", "Description for my condition!", "onCancelRequestGamerInfo");

AddCondition(conditionIndex++, cf_none, "Request Products on Success", "RequestProducts", "requestProductsOnSuccess", "Description for my condition!", "onSuccessRequestProducts");
AddCondition(conditionIndex++, cf_none, "Request Products on Failure", "RequestProducts", "requestProductsOnFailure", "Description for my condition!", "onFailureRequestProducts");
AddCondition(conditionIndex++, cf_none, "Request Products on Cancel", "RequestProducts", "requestProductsOnCancel", "Description for my condition!", "onCancelRequestProducts");

AddCondition(conditionIndex++, cf_none, "Request Purchase on Success", "RequestPurchase", "requestPurchaseOnSuccess", "Description for my condition!", "onSuccessRequestPurchase");
AddCondition(conditionIndex++, cf_none, "Request Purchase on Failure", "RequestPurchase", "requestPurchaseOnFailure", "Description for my condition!", "onFailureRequestPurchase");
AddCondition(conditionIndex++, cf_none, "Request Purchase on Cancel", "RequestPurchase", "requestPurchaseOnCancel", "Description for my condition!", "onCancelRequestPurchase");

AddCondition(conditionIndex++, cf_none, "Request Receipts on Success", "RequestReceipts", "requestReceiptsOnSuccess", "Description for my condition!", "onSuccessRequestReceipts");
AddCondition(conditionIndex++, cf_none, "Request Receipts on Failure", "RequestReceipts", "requestReceiptsOnFailure", "Description for my condition!", "onFailureRequestReceipts");
AddCondition(conditionIndex++, cf_none, "Request Receipts on Cancel", "RequestReceipts", "requestReceiptsOnCancel", "Description for my condition!", "onCancelRequestReceipts");

AddCondition(conditionIndex++, cf_none, "Set Safe Area on Success", "SetSafeArea", "setSafeAreasOnSuccess", "Description for my condition!", "onSuccessSetSafeArea");
AddCondition(conditionIndex++, cf_none, "Set Safe Area on Failure", "SetSafeArea", "setSafeAreaOnFailure", "Description for my condition!", "onFailureSetSafeArea");

AddCondition(conditionIndex++, cf_none, "Shutdown on Success", "Shutdown", "shutdownOnSuccess", "Description for my condition!", "onSuccessShutdown");
AddCondition(conditionIndex++, cf_none, "Shutdown on Failure", "Shutdown", "shutdownOnFailure", "Description for my condition!", "onFailureShutdown");

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

var actionIndex = -1;

AddStringParam("key", "Enter an initialization key.");
AddStringParam("value", "Enter an initialization value.");
AddAction(++actionIndex, af_none, "Add Initialization OUYA Plugin Values", "Initialization", "addInitOuyaPluginValues key={0} value={1}", "Description for my action!", "addInitOuyaPluginValues");

AddAction(++actionIndex, af_none, "Initialize OUYA Plugin", "Initialization", "initOuyaPlugin", "Description for my action!", "initOuyaPlugin");

AddAction(++actionIndex, af_none, "Request Gamer Info", "RequestGamerInfo", "requestGamerInfo", "Description for my action!", "requestGamerInfo");

AddStringParam("products", "Enter product ids separated by commas.");
AddAction(++actionIndex, af_none, "Request Products", "RequestProducts", "requestProducts({0})", "Description for my action!", "requestProducts");

AddStringParam("purchasable", "Enter product id to purchase.");
AddAction(++actionIndex, af_none, "Request Purchase", "RequestPurchase", "requestPurchase({0})", "Description for my action!", "requestPurchase");

AddAction(++actionIndex, af_none, "Request Receipts", "RequestReceipts", "requestReceipts", "Description for my action!", "requestReceipts");

AddNumberParam("SafeAreaAmount", "Enter the safe area amount from 0.0 to 1.0");
AddAction(++actionIndex, af_none, "Set Safe Area", "SetSafeArea", "setSafeArea({0})", "Description for my action!", "setSafeArea");

AddAction(++actionIndex, af_none, "Shutdown", "Shutdown", "shutdown", "Description for my action!", "shutdown");

////////////////////////////////////////
// Expressions

// AddExpression(id,			// any positive integer to uniquely identify this expression
//				 flags,			// (see docs) ef_none, ef_deprecated, ef_return_number, ef_return_string,
//								// ef_return_any, ef_variadic_parameters (one return flag must be specified)
//				 list_name,		// currently ignored, but set as if appeared in event wizard
//				 category,		// category in expressions panel
//				 exp_name,		// the expression name after the dot, e.g. "foo" for "myobject.foo" - also the runtime function name
//				 description);	// description in expressions panel

// example

var expressionIndex = -1;

AddExpression(expressionIndex++, ef_return_number, "Leet expression", "My category", "MyExpression", "Return the number 1337.");

AddExpression(expressionIndex++, ef_return_string, "GamerInfo.username", "requestGamerInfo", "GamerInfoUsername", "Get GamerInfo.username");
AddExpression(expressionIndex++, ef_return_string, "GamerInfo.uuid", "requestGamerInfo", "GamerInfoUuid", "Get GamerInfo.uuid");

AddExpression(expressionIndex++, ef_return_number, "Products.length", "requestProducts", "ProductsLength", "Get Products Length");

AddExpression(expressionIndex++, ef_return_number, "Receipts.length", "requestReceipts", "ReceiptsLength", "Get Receipts Length");

AddNumberParam("Index", "Receipt index");
AddExpression(expressionIndex++, ef_return_string, "GetReceiptsIdentififer", "requestReceipts", "GetReceiptsIdentififer", "Get Receipts Identifier");

AddNumberParam("Index", "Receipt index");
AddExpression(expressionIndex++, ef_return_string, "GetReceiptsGeneratedDate", "requestReceipts", "GetReceiptsGeneratedDate", "Get Receipts Generated Date");

AddNumberParam("Index", "Receipt index");
AddExpression(expressionIndex++, ef_return_number, "GetReceiptsLocalPrice", "requestReceipts", "GetReceiptsLocalPrice", "Get Receipts Local Price");

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
	new cr.Property(ept_integer, 	"My property",		77,		"An example property.")
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