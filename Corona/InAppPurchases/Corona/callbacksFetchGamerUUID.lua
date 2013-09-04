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
-- callbacksFetchGamerUUID.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"

local callbacksFetchGamerUUID = {}

callbacksFetchGamerUUID.onSuccess = function (gamerUUID)
	globals.txtStatus.text = "onSuccessFetchGamerUUID";
	if gamerUUID == nil then
        print("onSuccessFetchGamerUUID: (nil)");
        globals.txtGamerUUID.text = "Gamer UUID: (nil)";
	elseif gamerUUID == "" then
        print("onSuccessFetchGamerUUID: (empty)");
        globals.txtGamerUUID.text = "Gamer UUID: (empty)";
	else
        print("onSuccessFetchGamerUUID: " .. gamerUUID);
        globals.txtGamerUUID.text = "Gamer UUID: " .. gamerUUID;
	end
end

callbacksFetchGamerUUID.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureFetchGamerUUID";
	if errorCode == nil then
        print("onFailureFetchGamerUUID: errorCode=(nil)");
	else
        print("onFailureFetchGamerUUID: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureFetchGamerUUID: errorMessage=(nil)");
	else
        print("onFailureFetchGamerUUID: errorMessage=" .. errorMessage);
	end
end
callbacksFetchGamerUUID.onCancel = function ()
	globals.txtStatus.text = "onCancelFetchGamerUUID";
	print("onCancelFetchGamerUUID");
end

return callbacksFetchGamerUUID;