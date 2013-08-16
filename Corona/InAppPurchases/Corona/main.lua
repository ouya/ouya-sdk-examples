-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

local DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";

json = require "json"

if nil ~= myTests then
	print ("LUA ouyaSetDeveloperId (" .. DEVELOPER_ID .. ")");
	myTests.ouyaSetDeveloperId(DEVELOPER_ID);
end

local function updateSprite(spriteObj, x, y, xScale, yScale, alpha)

	spriteObj.x = x;
	spriteObj.y = y;
	spriteObj.xScale = xScale;
	spriteObj.yScale = yScale;
	spriteObj.alpha = alpha;

end

local function createButton (x, y, xScale, yScale, text, tx, ty, ts)

	local button =
	{
		x = x,
		y = y,
		xScale = xScale,
		yScale = yScale,
		spriteActive = display.newImage("ButtonActive.png"),
		spriteInactive = display.newImage("ButtonInactive.png"),
		txtLabel = display.newText(text, x + tx, y + ty, "Helvetica", ts),
		tx = tx,
		ty = ty,
		btnDown = nil,
		btnLeft = nil,
		btnRight = nil,
		btnUp = nil
	};
	
	updateSprite(button.spriteActive, x, y, xScale, yScale, 0);
	updateSprite(button.spriteInactive, x, y, xScale, yScale, 1);
	
	return button;
	
end

-- Fade out the sprite alpha over 500ms
local function spriteFadeOut (spriteObj)
	if spriteObj == nil then
		print("Using nil spriteObj in spriteFadeOut");
	else
		spriteObj.alpha = 1;	
		transition.to(spriteObj, { time=500, alpha=0 })
	end
end

-- Fade in the sprite alpha over 500ms
local function spriteFadeIn (spriteObj)
	if spriteObj == nil then
		print("Using nil spriteObj in spriteFadeIn");
	else
		spriteObj.alpha = 0;
		transition.to(spriteObj, { time=500, alpha=1 })
	end
end

local function setButtonFocus (btnNext)
	
	if focusButton ~= nil and focusButton ~= btnNext then
		spriteFadeOut(focusButton.spriteActive);
		spriteFadeIn(focusButton.spriteInactive);
	end
	
	if btnNext ~= nil then
		focusButton = btnNext;
	end
	
	if focusButton ~= nil then
		spriteFadeOut(focusButton.spriteInactive);
		spriteFadeIn(focusButton.spriteActive);
	end
	
end

local function clearProductText()
	print ("clear product text: " .. #productTextList);
	for i=1,#productTextList do
		local txtProduct = productTextList[i];
		print ("deleting text=" .. txtProduct.text);
		display.remove(txtProduct);
	end
	productTextList = { };
end

local function displayProductList()
	clearProductText();
	if getProducts ~= nil then
		for i=1,#getProducts do
				print ("onSuccessRequestProducts: identifier=" .. getProducts[i].identifier .. " name=" .. getProducts[i].name .. " priceInCents=" .. getProducts[i].priceInCents);
				local label = getProducts[i].identifier .. " name=" .. getProducts[i].name .. " priceInCents=" .. getProducts[i].priceInCents;
				local txtProduct = display.newText(label, centerX - 200, 425 + i * 30, "Helvetica", 24);
				productTextList[#productTextList + 1] = txtProduct;
	    end
	end
end

centerX = display.contentCenterX;

txtHello = display.newText("Hello from Corona SDK", centerX - 500, 200, "Helvetica", 24);
txtStatus = display.newText("", centerX, 200, "Helvetica", 24);
txtGamerUUID = display.newText("Gamer UUID: (unknown)", centerX - 300, 240, "Helvetica", 24);
txtInstructions = display.newText("Use DPAD to switch between buttons | Press O to click the button", centerX - 300, 325, "Helvetica", 24);

btnProducts = createButton(centerX - 400, 400, 1.5, 0.5, "Get Products", -70, -25, 24);
btnPurchase = createButton(centerX - 175, 400, 1.75, 0.5, "Request Purchase", -100, -25, 24);
btnReceipts = createButton(centerX + 50, 400, 1.5, 0.5, "Get Receipts", -70, -25, 24);
btnFetch = createButton(centerX + 275, 400, 1.75, 0.5, "Fetch Gamer UUID", -100, -25, 24);
btnPause = createButton(centerX + 450, 400, 0.75, 0.5, "Pause", -35, -25, 24);

btnProducts.btnRight = btnPurchase;
btnPurchase.btnLeft = btnProducts;
btnPurchase.btnRight = btnReceipts;
btnReceipts.btnLeft = btnPurchase;
btnReceipts.btnRight = btnFetch;
btnFetch.btnLeft = btnReceipts;
btnPause.btnLeft = btnFetch;

setButtonFocus (btnProducts);

getProducts = { };
productTextList = { };
displayProductList();

function onSuccessFetchGamerUUID(gamerUUID)
	txtStatus.text = "onSuccessFetchGamerUUID";
	if gamerUUID == nil then
        print("onSuccessFetchGamerUUID: (nil)");
        txtGamerUUID.text = "Gamer UUID: (nil)";
	elseif gamerUUID == "" then
        print("onSuccessFetchGamerUUID: (empty)");
        txtGamerUUID.text = "Gamer UUID: (empty)";
	else
        print("onSuccessFetchGamerUUID: " .. gamerUUID);
        txtGamerUUID.text = "Gamer UUID: " .. gamerUUID;
	end
end
function onFailureFetchGamerUUID(errorCode, errorMessage)
	txtStatus.text = "onFailureFetchGamerUUID";
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
function onCancelFetchGamerUUID()
	txtStatus.text = "onCancelFetchGamerUUID";
	print("onCancelFetchGamerUUID");
end

function onSuccessRequestProducts(jsonData)
	txtStatus.text = "onSuccessRequestProducts";
	if jsonData == nil then
        print("onSuccessRequestProducts: (nil)");
	elseif jsonData == "" then
		print("onSuccessRequestProducts: (empty)");
	else
        print("onSuccessRequestProducts: jsonData=" .. jsonData);
        getProducts = json.decode(jsonData);
		displayProductList();
	end
end
function onFailureRequestProducts(errorCode, errorMessage)
	txtStatus.text = "onFailureRequestProducts";
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
function onCancelRequestProducts()
	txtStatus.text = "onCancelRequestProducts";
	print("onCancelRequestProducts");
end

function onSuccessRequestPurchase(product)
	txtStatus.text = "onSuccessRequestPurchase";
	if product == nil then
        print("onSuccessRequestPurchase: (nil)");
	else
        print("onSuccessRequestPurchase: "); -- .. product.identifier);
	end
end
function onFailureRequestPurchase(errorCode, errorMessage)
	txtStatus.text = "onFailureRequestPurchase";
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
function onCancelRequestPurchase()
	txtStatus.text = "onCancelRequestPurchase";
	print("onCancelRequestPurchase");
end

function onSuccessRequestReceipts(receipts)
	txtStatus.text = "onSuccessRequestReceipts";
	if receipts == nil then
        print("onSuccessRequestReceipts: (nil)");
	else
        print("onSuccessRequestReceipts: "); -- .. receipts.length);
	end
end
function onFailureRequestReceipts(errorCode, errorMessage)
	txtStatus.text = "onFailureRequestReceipts";
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
function onCancelRequestReceipts()
	txtStatus.text = "onCancelRequestReceipts";
	print("onCancelRequestReceipts");
end

-- Called when a key event has been received.
local function onKeyEvent( event )
	--print("===== onKeyEvent ======")
	
	if event.device == nil then
		print("### device = nil")
		return false;
	end
	    
	--System Button / Pause Menu
    if (event.keyName == "menu" and event.phase == "up") then
		txtStatus.text = "Pause Detected";
    	setButtonFocus(btnPause);
    end
	
	--DPADS
	if (event.keyName == "left" and event.phase == "down") then
		setButtonFocus(focusButton.btnLeft);
    end
    
    if (event.keyName == "right" and event.phase == "down") then
		setButtonFocus(focusButton.btnRight);
    end
        
    --end of DPADS
    
	-- BUTTONS
    
    if (event.keyName == "buttonA" and event.phase == "down") then -- OUYA BUTTON_O
    	if focusButton == btnFetch then
    		txtStatus.text = "Fetching Gamer UUID...";
    		txtGamerUUID.text = "Gamer UUID:";
    		print "Invoking asyncLuaOuyaFetchGamerUUID(onSuccessFetchGamerUUID, onFailureFetchGamerUUID, onCancelFetchGamerUUID)...";
    		myTests.asyncLuaOuyaFetchGamerUUID(onSuccessFetchGamerUUID, onFailureFetchGamerUUID, onCancelFetchGamerUUID);
    	elseif focusButton == btnProducts then
    		getProducts = { };
    		displayProductList();
    		txtStatus.text = "Requesting products...";
    		local products =  { "long_sword", "sharp_axe", "cool_level", "awesome_sauce", "__DECLINED__THIS_PURCHASE" };
    		print "Invoking asyncLuaOuyaRequestProducts(onSuccessRequestProducts, onFailureRequestProducts, onCancelRequestProducts, products)...";
    		myTests.asyncLuaOuyaRequestProducts(onSuccessRequestProducts, onFailureRequestProducts, onCancelRequestProducts, products);
    	elseif focusButton == btnPurchase then
    		txtStatus.text = "Requesting purchase...";
    		local purchasable = "long_sword";
    		print "Invoking asyncLuaOuyaRequestPurchase(onSuccessRequestPurchase, onFailureRequestPurchase, onCancelRequestPurchase, purchasable)...";
    		myTests.asyncLuaOuyaRequestPurchase(onSuccessRequestPurchase, onFailureRequestPurchase, onCancelRequestPurchase, purchasable);	
    	elseif focusButton == btnReceipts then
    		txtStatus.text = "Requesting receipts...";
    		print "Invoking asyncLuaOuyaRequestReceipts(onSuccessRequestReceipts, onFailureRequestReceipts, onCancelRequestReceipts)...";
    		myTests.asyncLuaOuyaRequestReceipts(onSuccessRequestReceipts, onFailureRequestReceipts, onCancelRequestReceipts);	
    	end
    end
       
    -- End of BUTTONS
	
	return false
end

-- Add the key event listener.
Runtime:addEventListener( "key", onKeyEvent )