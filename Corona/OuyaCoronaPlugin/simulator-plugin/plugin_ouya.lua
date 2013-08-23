-- This script is called when require("plugin.ouya") is called.


-- Create the Ouya Lua library object.
local library = require("CoronaLibrary")
local ouya = library:new{ name = "ouya", publisherId = "tv.ouya" }

-- Set IAP Developer ID
ouya.ouyaSetDeveloperId = function(developerId)
	ouyaSDK.ouyaSetDeveloperId(developerId);
end

-- Invoke IAP Fetch Gamer UUID and provide callbacks
ouya.asyncLuaOuyaFetchGamerUUID = function(onSuccess, onFailure, onCancel)
	ouyaSDK.asyncLuaOuyaFetchGamerUUID(onSuccess, onFailure, onCancel);
end

-- Invoke IAP Request Products, provide callbacks, and a table of product identifiers
ouya.asyncLuaOuyaRequestProducts = function(onSuccess, onFailure, onCancel, products)
	ouyaSDK.asyncLuaOuyaRequestProducts(onSuccess, onFailure, onCancel, products);	
end

-- Invoke IAP Request Purchase, provide callbacks, and purchasable identifier
ouya.asyncLuaOuyaRequestPurchase = function(onSuccess, onFailure, onCancel, purchasable)
	ouyaSDK.asyncLuaOuyaRequestPurchase(onSuccess, onFailure, onCancel, purchasable);
end

-- Invoke IAP Request Receipts and provide callbacks
ouya.asyncLuaOuyaRequestReceipts = function(onSuccess, onFailure, onCancel)
	ouyaSDK.asyncLuaOuyaRequestReceipts(onSuccess, onFailure, onCancel)
end

-- Return the Ouya library from the require() 
return ouya
