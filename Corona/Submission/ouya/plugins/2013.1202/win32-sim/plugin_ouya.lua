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


print "plugin.ouya is not supported on this platform";

-- Create the Ouya Lua library object.
local plugin_ouya = {}

plugin_ouya.initialize = function ()
end

-- Set IAP Developer ID
plugin_ouya.ouyaSetDeveloperId = function(developerId)
end

-- Invoke IAP Fetch Gamer UUID and provide callbacks
plugin_ouya.asyncLuaOuyaFetchGamerUUID = function(onSuccess, onFailure, onCancel)
end

-- Invoke IAP Request Products, provide callbacks, and a table of product identifiers
plugin_ouya.asyncLuaOuyaRequestProducts = function(onSuccess, onFailure, onCancel, products)	
end

-- Invoke IAP Request Purchase, provide callbacks, and purchasable identifier
plugin_ouya.asyncLuaOuyaRequestPurchase = function(onSuccess, onFailure, onCancel, purchasable)
end

-- Invoke IAP Request Receipts and provide callbacks
plugin_ouya.asyncLuaOuyaRequestReceipts = function(onSuccess, onFailure, onCancel)
end

-- Return the Ouya library from the require() 
return plugin_ouya