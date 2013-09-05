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
-- inputs.lua
--
-----------------------------------------------------------------------------------------

callbacksFetchGamerUUID = require "callbacksFetchGamerUUID"
callbacksRequestProducts = require "callbacksRequestProducts"
callbacksRequestPurchase = require "callbacksRequestPurchase"
callbacksRequestReceipts = require "callbacksRequestReceipts"
globals = require "globals"
plugin_ouya = require "plugin_ouya"
ui = require "ui"

local inputs = {}

-- Called when a key event has been received.
inputs.onKeyEvent = function (event)
	--print("===== onKeyEvent ======")
	
	if event.device == nil then
		print("### device = nil")
		return false;
	end
	    
	--System Button / Pause Menu
    if (event.keyName == "menu" and event.phase == "up") then
		globals.txtStatus.text = "Pause Detected";
    	ui.setButtonFocus(globals.btnPause);
    end
	
	--DPADS
	if (event.keyName == "left" and event.phase == "down") then
		ui.setButtonFocus(globals.focusButton.btnLeft);
    end
    
    if (event.keyName == "right" and event.phase == "down") then
		ui.setButtonFocus(globals.focusButton.btnRight);
    end
    
    if (globals.focusButton == globals.btnProducts or globals.focusButton == globals.btnPurchase) then
	    if (event.keyName == "up" and event.phase == "down") then
			if #globals.getProducts > 1 then
				globals.selectedProduct = (globals.selectedProduct + #globals.getProducts - 1) % #globals.getProducts;
				ui.displayProductList();
			end
	    end
	    
	    if (event.keyName == "down" and event.phase == "down") then
			if #globals.getProducts > 1 then
				globals.selectedProduct = (globals.selectedProduct + 1) % #globals.getProducts;
				ui.displayProductList();
			end
	    end
    end
        
    --end of DPADS
    
	-- BUTTONS
    
    if (event.keyName == "buttonA" and event.phase == "down" and nil ~= plugin_ouya) then -- OUYA BUTTON_O
    	if globals.focusButton == globals.btnFetch then
    		globals.txtStatus.text = "Fetching Gamer UUID...";
    		globals.txtGamerUUID.text = "Gamer UUID:";
    		print "Accessing plugin_ouya...";
    		plugin_ouya.asyncLuaOuyaFetchGamerUUID(callbacksFetchGamerUUID.onSuccess, callbacksFetchGamerUUID.onFailure, callbacksFetchGamerUUID.onCancel);
    	elseif globals.focusButton == globals.btnProducts then
    		globals.getProducts = { };
    		ui.displayProductList();
    		globals.txtStatus.text = "Requesting products...";
    		local products =  { "long_sword", "sharp_axe", "cool_level", "awesome_sauce", "__DECLINED__THIS_PURCHASE" };
			print "Accessing plugin_ouya...";
    		plugin_ouya.asyncLuaOuyaRequestProducts(callbacksRequestProducts.onSuccess, callbacksRequestProducts.onFailure, callbacksRequestProducts.onCancel, products);
    	elseif globals.focusButton == globals.btnPurchase then
    		if #globals.getProducts > 1 and globals.selectedProduct < #globals.getProducts then
	    		globals.txtStatus.text = "Requesting purchase: " .. globals.getProducts[globals.selectedProduct + 1].name;
	    		local purchasable = globals.getProducts[globals.selectedProduct + 1].identifier;
				print "Accessing plugin_ouya...";
	    		plugin_ouya.asyncLuaOuyaRequestPurchase(callbacksRequestPurchase.onSuccess, callbacksRequestPurchase.onFailure, callbacksRequestPurchase.onCancel, purchasable);
    		else
    			globals.txtStatus.text = "Select a product for purchase...";
    		end
    	elseif globals.focusButton == globals.btnReceipts then
    		globals.getReceipts = { };
    		ui.displayReceiptList();
    		globals.txtStatus.text = "Requesting receipts...";
    		print "Accessing plugin_ouya...";
    		plugin_ouya.asyncLuaOuyaRequestReceipts(callbacksRequestReceipts.onSuccess, callbacksRequestReceipts.onFailure, callbacksRequestReceipts.onCancel);	
    	end
    end
       
    -- End of BUTTONS
	
	return false
end

return inputs;