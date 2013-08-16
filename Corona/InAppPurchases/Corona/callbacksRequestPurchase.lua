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
-- callbacksRequestPurchase.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
json = require "json"

local callbacksRequestPurchase = {}

callbacksRequestPurchase.onSuccess = function (jsonData)
	globals.txtStatus.text = "onSuccessRequestPurchase";
	if jsonData == nil then
        print("onSuccessRequestPurchase: (nil)");
	elseif jsonData == "" then
		print("onSuccessRequestPurchase: (empty)");
	else
        print("onSuccessRequestPurchase: jsonData=" .. jsonData);
        local purchase = json.decode(jsonData);
	end
end

callbacksRequestPurchase.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureRequestPurchase";
	if errorCode == nil then
        print("onFailureRequestPurchase: errorCode=(nil)");
	else
        print("onFailureRequestPurchase: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureRequestPurchase: errorMessage=(nil)");
	else
        print("onFailureRequestPurchase: errorMessage=" .. errorMessage);
	end
end

callbacksRequestPurchase.onCancel = function ()
	globals.txtStatus.text = "onCancelRequestPurchase";
	print("onCancelRequestPurchase");
end

return callbacksRequestPurchase;