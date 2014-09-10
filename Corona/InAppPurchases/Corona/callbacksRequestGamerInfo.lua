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
-- callbacksRequestGamerInfo.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
json = require "json"

local callbacksRequestGamerInfo = {}

callbacksRequestGamerInfo.onSuccess = function (jsonData)
	globals.txtStatus.text = "onSuccessRequestGamerInfo";
	if jsonData == nil then
        print("onSuccessRequestGamerInfo: (nil)");
        globals.txtGamerUUID.text = "Gamer UUID: (nil)";
        globals.txtGamerUsername.text = "Gamer Username: (nil)";
	elseif jsonData == "" then
		print("onSuccessRequestGamerInfo: (empty)");
		globals.txtGamerUUID.text = "Gamer UUID: (empty)";
        globals.txtGamerUsername.text = "Gamer Username: (empty)";
	else
        print("onSuccessRequestGamerInfo: " .. jsonData);
        gamerInfo = json.decode(jsonData);
        globals.txtGamerUUID.text = "Gamer UUID: " .. gamerInfo.uuid;
        globals.txtGamerUsername.text = "Gamer Username: " .. gamerInfo.username;
	end
end

callbacksRequestGamerInfo.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureFetchGamerInfo";
	if errorCode == nil then
        print("onFailureRequestGamerInfo: errorCode=(nil)");
	else
        print("onFailureRequestGamerInfo: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureRequestGamerInfo: errorMessage=(nil)");
	else
        print("onFailureRequestGamerInfo: errorMessage=" .. errorMessage);
	end
end
callbacksRequestGamerInfo.onCancel = function ()
	globals.txtStatus.text = "onCancelRequestGamerInfo";
	print("onCancelRequestGamerInfo");
end

return callbacksRequestGamerInfo;