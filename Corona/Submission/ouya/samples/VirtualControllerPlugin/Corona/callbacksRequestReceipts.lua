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
-- callbacksRequestReceipts.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
json = require "json"

local callbacksRequestReceipts = {}

callbacksRequestReceipts.onSuccess = function (jsonData)
	globals.txtStatus.text = "onSuccessRequestReceipts";
	if jsonData == nil then
        print("onSuccessRequestReceipts: (nil)");
	elseif jsonData == "" then
		print("onSuccessRequestReceipts: (empty)");
	else
        print("onSuccessRequestReceipts: jsonData=" .. jsonData);
        globals.getReceipts = json.decode(jsonData);
		ui.displayReceiptList();
	end
end

callbacksRequestReceipts.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureRequestReceipts";
	if errorCode == nil then
        print("onFailureRequestReceipts: errorCode=(nil)");
	else
        print("onFailureRequestReceipts: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureRequestReceipts: errorMessage=(nil)");
	else
        print("onFailureRequestReceipts: errorMessage=" .. errorMessage);
	end
end

callbacksRequestReceipts.onCancel = function ()
	globals.txtStatus.text = "onCancelRequestReceipts";
	print("onCancelRequestReceipts");
end

return callbacksRequestReceipts;