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
-- callbacksRequestProducts.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
json = require "json"

local callbacksRequestProducts = {}

callbacksRequestProducts.onSuccess = function (jsonData)
	globals.txtStatus.text = "onSuccessRequestProducts";
	if jsonData == nil then
        print("onSuccessRequestProducts: (nil)");
	elseif jsonData == "" then
		print("onSuccessRequestProducts: (empty)");
	else
        --print("onSuccessRequestProducts: jsonData=" .. jsonData);
        globals.getProducts = json.decode(jsonData);
		ui.displayProductList();
	end
end

callbacksRequestProducts.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureRequestProducts";
	if errorCode == nil then
        print("onFailureRequestProducts: errorCode=(nil)");
	else
        print("onFailureRequestProducts: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureRequestProducts: errorMessage=(nil)");
	else
        print("onFailureRequestProducts: errorMessage=" .. errorMessage);
	end
end

callbacksRequestProducts.onCancel = function ()
	globals.txtStatus.text = "onCancelRequestProducts";
	print("onCancelRequestProducts");
end

return callbacksRequestProducts;