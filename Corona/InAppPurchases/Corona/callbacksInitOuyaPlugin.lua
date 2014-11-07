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
-- callbacksRequestInitOuyaPlugin.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
json = require "json"

local callbacksInitOuyaPlugin = {}

callbacksInitOuyaPlugin.onSuccess = function ()
	globals.txtStatus.text = "onSuccessRequestInitOuyaPlugin";
end

callbacksInitOuyaPlugin.onFailure = function (errorCode, errorMessage)
	globals.txtStatus.text = "onFailureInitOuyaPlugin";
	if errorCode == nil then
        print("onFailureRequestInitOuyaPlugin: errorCode=(nil)");
	else
        print("onFailureRequestInitOuyaPlugin: errorCode=" .. errorCode);
	end
	
	if errorMessage == nil then
        print("onFailureRequestInitOuyaPlugin: errorMessage=(nil)");
	else
        print("onFailureRequestInitOuyaPlugin: errorMessage=" .. errorMessage);
	end
end

return callbacksInitOuyaPlugin;