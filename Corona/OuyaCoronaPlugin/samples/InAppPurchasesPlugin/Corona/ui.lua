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
-- ui.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"

local ui = {}

ui.setButtonFocus = function (btnNext)
	
	if globals.focusButton ~= nil and globals.focusButton ~= btnNext then
		helpers.spriteFadeOut(globals.focusButton.spriteActive);
		helpers.spriteFadeIn(globals.focusButton.spriteInactive);
	end
	
	if btnNext ~= nil then
		globals.focusButton = btnNext;
	end
	
	if globals.focusButton ~= nil then
		helpers.spriteFadeOut(globals.focusButton.spriteInactive);
		helpers.spriteFadeIn(globals.focusButton.spriteActive);
	end
	
end

ui.clearProductText = function ()
	print ("clear product text: " .. #globals.productTextList);
	for i=1,#globals.productTextList do
		local txtProduct = globals.productTextList[i];
		print ("deleting text=" .. txtProduct.text);
		display.remove(txtProduct);
	end
	globals.productTextList = { };
end

ui.displayProductList = function ()
	ui.clearProductText();
	ui.clearReceiptText();
	if globals.getProducts ~= nil then
		for i=1,#globals.getProducts do
				print ("displayProductList: identifier=" .. globals.getProducts[i].identifier .. " name=" .. globals.getProducts[i].name .. " localPrice=" .. globals.getProducts[i].localPrice);
				local label = "Product: " .. globals.getProducts[i].identifier .. " name=" .. globals.getProducts[i].name .. " localPrice=" .. globals.getProducts[i].localPrice;
				local txtProduct = display.newText(label, globals.centerX - 200, 425 + i * 30, "Helvetica", 24);
				if (1 + globals.selectedProduct) == i then
					txtProduct:setTextColor(255, 255, 255);
				else
					txtProduct:setTextColor(255, 127, 0);
				end
				globals.productTextList[#globals.productTextList + 1] = txtProduct;
	    end
	end
end

ui.clearReceiptText = function ()
	print ("clear receipt text: " .. #globals.receiptTextList);
	for i=1,#globals.receiptTextList do
		local txtReceipt = globals.receiptTextList[i];
		print ("deleting text=" .. txtReceipt.text);
		display.remove(txtReceipt);
	end
	globals.receiptTextList = { };
end

ui.displayReceiptList = function ()
	ui.clearProductText();
	ui.clearReceiptText();
	if globals.getReceipts ~= nil then
		for i=1,#globals.getReceipts do
				print ("displayReceiptList: identifier=" .. globals.getReceipts[i].identifier .. " localPrice=" .. globals.getReceipts[i].localPrice);
				local label = "Receipt: " .. globals.getReceipts[i].identifier .. " localPrice=" .. globals.getReceipts[i].localPrice;
				local txtReceipt = display.newText(label, globals.centerX - 200, 425 + i * 30, "Helvetica", 24);
				txtReceipt:setTextColor(255, 127, 0);
				globals.receiptTextList[#globals.receiptTextList + 1] = txtReceipt;
	    end
	end
end

return ui;