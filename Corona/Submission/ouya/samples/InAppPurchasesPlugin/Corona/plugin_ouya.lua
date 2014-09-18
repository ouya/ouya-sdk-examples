-- Copyright (C) 2012, 2013 OUYA, Inc.
--
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
--
--    http://www.apache.org/licenses/LICENSE-2.0
--
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.

-----------------------------------------------------------------------------------------
--
-- plugin_ouya.lua
--
-----------------------------------------------------------------------------------------


-- This script is called when require("plugin.ouya") is called.
print "plugin.ouya is loading plugin_ouya";

-- Create the Ouya Lua library object.
local library = require("CoronaLibrary")
local plugin_ouya = library:new{ name = "ouya", publisherId = "tv.ouya" }

plugin_ouya.developerId = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

plugin_ouya.initialized = false;

plugin_ouya.initialize = function ()
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	if plugin_ouya.initialized == false then
			plugin_ouya.ouyaSetDeveloperId(plugin_ouya.developerId);
			print "plugin_ouya is initialized";
	end
end

-- Set IAP Developer ID
plugin_ouya.ouyaSetDeveloperId = function(developerId)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end

	if plugin_ouya.initialized == true then
		return;
	end
	
	--print ("plugin_ouya.ouyaSetDeveloperId (" .. developerId .. ")");
	ouyaSDK.ouyaSetDeveloperId(developerId);
	plugin_ouya.initialized = true;
end

-- Invoke IAP Request Gamer Info and provide callbacks
plugin_ouya.asyncLuaOuyaRequestGamerInfo = function(onSuccess, onFailure, onCancel)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaRequestGamerInfo");
	ouyaSDK.asyncLuaOuyaRequestGamerInfo(onSuccess, onFailure, onCancel);
end

-- Invoke IAP Request Products, provide callbacks, and a table of product identifiers
plugin_ouya.asyncLuaOuyaRequestProducts = function(onSuccess, onFailure, onCancel, products)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaRequestProducts");
	ouyaSDK.asyncLuaOuyaRequestProducts(onSuccess, onFailure, onCancel, products);	
end

-- Invoke IAP Request Purchase, provide callbacks, and purchasable identifier
plugin_ouya.asyncLuaOuyaRequestPurchase = function(onSuccess, onFailure, onCancel, purchasable)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaRequestPurchase (" .. purchasable .. ")");
	ouyaSDK.asyncLuaOuyaRequestPurchase(onSuccess, onFailure, onCancel, purchasable);
end

-- Invoke IAP Request Receipts and provide callbacks
plugin_ouya.asyncLuaOuyaRequestReceipts = function(onSuccess, onFailure, onCancel)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaRequestReceipts");
	ouyaSDK.asyncLuaOuyaRequestReceipts(onSuccess, onFailure, onCancel)
end

-- subscribe to input events with callbacks
plugin_ouya.asyncLuaOuyaInitInput = function(onGenericMotionEvent, onKeyDown, onKeyUp)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaInitInput");
	ouyaSDK.asyncLuaOuyaInitInput(onGenericMotionEvent, onKeyDown, onKeyUp)
end

-- access the controller name
plugin_ouya.asyncLuaOuyaGetControllerName = function(onGetControllerName, playerNum)
	if ouyaSDK == nil then
		print "ouyaSDK named java functions are not initialized";
		return;
	end
	plugin_ouya.initialize();
	
	print ("plugin_ouya.asyncLuaOuyaGetControllerName");
	ouyaSDK.asyncLuaOuyaGetControllerName(onGetControllerName, playerNum)
end

-- Return the Ouya library from the require() 
return plugin_ouya