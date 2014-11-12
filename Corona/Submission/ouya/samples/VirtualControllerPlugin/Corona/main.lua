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
-- main.lua
--
-----------------------------------------------------------------------------------------

local ouya = require("plugin.ouya") -- load the ouya plugin

callbacksInitOuyaPlugin = require "callbacksInitOuyaPlugin"
globals = require "globals"
helpers = require "helpers"
inputs = require "inputs"
local json = require "json"
plugin_ouya = require "plugin_ouya"
ui = require "ui"

if nil ~= plugin_ouya and nil ~= plugin_ouya.initOuyaPlugin then
	local data = {
	[1] = {
	    ["key"] = "tv.ouya.developer_id",
	    ["value"] = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0"
	}};
	local jsonData = json.encode(data);
	--print (jsonData);
	plugin_ouya.initOuyaPlugin(callbacksInitOuyaPlugin.onSuccess, callbacksInitOuyaPlugin.onFailure, jsonData);
end

globals.controllers =
{
	ui.createController(1, -100, 500, 2, 2),
	ui.createController(2, -100, 1200, 2, 2),
	ui.createController(3, 1000, 500, 2, 2),
	ui.createController(4, 1000, 1200, 2, 2)
};