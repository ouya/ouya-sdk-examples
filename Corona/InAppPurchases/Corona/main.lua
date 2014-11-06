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

callbacksInitOuyaPlugin = require "callbacksInitOuyaPlugin"
globals = require "globals"
helpers = require "helpers"
inputs = require "inputs"
local json = require "json"
ui = require "ui"

if nil ~= ouyaSDK and nil ~= ouyaSDK.initOuyaPlugin then
	local data = {
	[1] = {
	    ["key"] = "tv.ouya.developer_id",
	    ["value"] = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0"
	}};
	local jsonData = json.encode(data);
	--print (jsonData);
	ouyaSDK.initOuyaPlugin(callbacksInitOuyaPlugin.onSuccess, callbacksInitOuyaPlugin.onFailure, jsonData);
end

globals.centerX = display.contentCenterX;

globals.txtHello = display.newText("Hello from Corona SDK", globals.centerX - 500, 200, "Helvetica", 24);
globals.txtStatus = display.newText("", globals.centerX, 200, "Helvetica", 24);
globals.txtGamerUsername = display.newText("Gamer Username: (unknown)", globals.centerX - 300, 240, "Helvetica", 24);
globals.txtGamerUUID = display.newText("Gamer UUID: (unknown)", globals.centerX - 300, 270, "Helvetica", 24);
globals.txtInstructions = display.newText("Use DPAD to switch between buttons | Press O to click the button", globals.centerX - 300, 325, "Helvetica", 24);

globals.btnProducts = helpers.createButton(globals.centerX - 400, 400, 1.5, 0.5, "Get Products", 0, 0, 24);
globals.btnPurchase = helpers.createButton(globals.centerX - 175, 400, 1.75, 0.5, "Request Purchase", 0, 0, 24);
globals.btnReceipts = helpers.createButton(globals.centerX + 50, 400, 1.5, 0.5, "Get Receipts", 0, 0, 24);
globals.btnGamerInfo = helpers.createButton(globals.centerX + 275, 400, 1.75, 0.5, "Request Gamer Info", 0, 0, 24);
globals.btnPause = helpers.createButton(globals.centerX + 450, 400, 0.75, 0.5, "Pause", 0, 0, 24);

globals.btnProducts.btnRight = globals.btnPurchase;
globals.btnPurchase.btnLeft = globals.btnProducts;
globals.btnPurchase.btnRight = globals.btnReceipts;
globals.btnReceipts.btnLeft = globals.btnPurchase;
globals.btnReceipts.btnRight = globals.btnGamerInfo;
globals.btnGamerInfo.btnLeft = globals.btnReceipts;
globals.btnPause.btnLeft = globals.btnGamerInfo;

ui.setButtonFocus (globals.btnProducts);