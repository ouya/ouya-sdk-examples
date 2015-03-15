deadzone = 0.25;

AXIS_LS_X = "0";
AXIS_LS_Y = "1";
AXIS_RS_X = "11";
AXIS_RS_Y = "14";
AXIS_L2 = "17";
AXIS_R2 = "18";
BUTTON_O = "96";
BUTTON_U = "99";
BUTTON_Y = "100";
BUTTON_A = "97";
BUTTON_L1 = "102";
BUTTON_R1 = "103";
BUTTON_L3 = "106";
BUTTON_R3 = "107";
BUTTON_DPAD_UP = "19";
BUTTON_DPAD_DOWN = "20";
BUTTON_DPAD_RIGHT = "22";
BUTTON_DPAD_LEFT = "21";
BUTTON_MENU = "82";

draw_text_colour(150, 100, "Hello from Game Maker!", c_white, c_white, c_white, c_white, 1);

draw_text_colour(175, 140, text_message, c_white, c_white, c_white, c_white, 1);

if (button_index == 0)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 150;
y = 200;
scaleX = 1.75;
scaleY = 0.5;
draw_sprite_ext(spriteIndex, -1, x+75, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Request Products", c_white, c_white, c_white, c_white, 1);

if (button_index == 1)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 500;
scaleX = 1.75;
draw_sprite_ext(spriteIndex, -1, x+75, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Request Purchase", c_white, c_white, c_white, c_white, 1);

if (button_index == 2)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 800;
scaleX = 1.75;
draw_sprite_ext(spriteIndex, -1, x+75, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Request Receipts", c_white, c_white, c_white, c_white, 1);

if (button_index == 3)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 1100;
scaleX = 1.75;
draw_sprite_ext(spriteIndex, -1, x+75, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Request GamerInfo", c_white, c_white, c_white, c_white, 1);

if (button_index == 4)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 1400;
scaleX = 1;
draw_sprite_ext(spriteIndex, -1, x+25, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Exit", c_white, c_white, c_white, c_white, 1);

if (button_index == 5)
{
    spriteIndex = 0;
}
else
{
    spriteIndex = 1;
}
x = 1650;
scaleX = 1;
draw_sprite_ext(spriteIndex, -1, x+25, y, scaleX, scaleY, 0, c_white, 1);
draw_text_colour(x, y, "Pause", c_white, c_white, c_white, c_white, 1);

controllers_connected = OuyaSDK_IsAnyConnected();
if (controllers_active != controllers_connected)
{
    controllers_active = controllers_connected;
    if (controllers_active) {
        text_message = "Status: Controller detected!";
    } else {
        text_message = "Status: No controllers are connected!";
    }
}


if (OuyaSDK_GetAnyButtonUp(BUTTON_MENU))
{
    button_index = 5;
    text_message = "Status: Pause detected";
}

if (OuyaSDK_GetAnyButtonUp(BUTTON_DPAD_RIGHT))
{
    if (button_index < 4)
    {
        button_index = button_index + 1;
    }
}

if (OuyaSDK_GetAnyButtonUp(BUTTON_DPAD_LEFT))
{
    if (button_index > 0)
    {
        button_index = button_index - 1;
    }
}

if (OuyaSDK_GetAnyButtonUp(BUTTON_O))
{
    if (button_index == 0)
    {
        text_message = "Status: Requesting products...";
        products_index = 0;
        products_length = 0;
        OuyaSDK_RequestProducts(strPurchasables);
    }
    
    if (button_index == 1)
    {
        text_message = "Status: Requesting purchase...";
        if (products_index < array_length_1d(product_ids) &&
            products_length > 0)
        {
            OuyaSDK_RequestPurchase(product_ids[products_index]);
        }
        else
        {
            text_message = "Status: Request products to select a purchase...";
        }
    }    
    
    if (button_index == 2)
    {
        text_message = "Status: Requesting receipts...";
        receipts_length = 0;
        OuyaSDK_RequestReceipts();
    }    
    
    if (button_index == 3)
    {
        text_message = "Status: Requesting gamer info...";
        OuyaSDK_RequestGamerInfo();
    }    

    if (button_index == 4)
    {
        text_message = "Status: Exiting game...";
        game_end();
    }
    
    if (button_index == 5)
    {
        text_message = "Status: Pause detected...";
    }
}

if (button_index == 0 ||
    button_index == 1)
{
    if (OuyaSDK_GetAnyButtonUp(BUTTON_DPAD_UP))
    {
        if (products_index > 0) {
            --products_index;
        }
    }
    if (OuyaSDK_GetAnyButtonUp(BUTTON_DPAD_DOWN))
    {
        if ((products_index+1) < products_length) {
            ++products_index;
        }
    }
    x = 150;
    y = 260;
    for (index = 0; index < products_length; ++index)
    {
        if (index == products_index) {
            draw_text_colour(x, y, "* "+products[index], c_white, c_white, c_white, c_white, 1);
        } else {
            draw_text_colour(x, y, products[index], c_white, c_white, c_white, c_white, 1);
        }
        y += 25;
    }
}

if (button_index == 2)
{
    x = 150;
    y = 260;
    for (index = 0; index < receipts_length; ++index)
    {
        draw_text_colour(x, y, receipts[index], c_white, c_white, c_white, c_white, 1);
        y += 25;
    }
}

asyncResult = OuyaSDK_GetAsyncResult();
if (asyncResult != undefined &&
    asyncResult != "") {
    text_message = "Status: "+asyncResult;
    asyncMethod = OuyaSDK_GetAsyncMethod();
    if (asyncMethod != undefined &&
        asyncMethod != "") {
        text_message = "Status: Method="+asyncMethod+" json="+asyncResult;
        if (asyncMethod == "onSuccessRequestGamerInfo") {
            var uuid = OuyaSDK_GetAsyncDataString("uuid");
            var username = OuyaSDK_GetAsyncDataString("username");            
            text_message = "Status: RequestGamerInfo uuid="+uuid+" username="+username;
        }
        else if (asyncMethod == "onFailureRequestGamerInfo") {
            var errorCode = OuyaSDK_GetAsyncDataString("errorCode");
            var errorMessage = OuyaSDK_GetAsyncDataString("errorMessage");            
            text_message = "Status: RequestGamerInfo errorCode="+errorCode+" errorMessage="+errorMessage;
        }
        else if (asyncMethod == "onSuccessRequestProducts") {
            var count = OuyaSDK_GetAsyncDataArrayCount();
            products_length = count;
            text_message = "Status: RequestProducts count="+string(count);
            if (count > 0) {
                text_message = "Status: RequestProducts count="+string(count);
            }
            for (var index = 0; index < products_length; ++index)
            {
                var identifier = OuyaSDK_GetAsyncDataArrayString(string(index), "identifier");
                var localPrice = OuyaSDK_GetAsyncDataArrayDouble(string(index), "localPrice");
                products[index] = "Product identifier="+identifier+" localPrice="+string(localPrice);
                product_ids[index] = identifier;
            }
        }
        else if (asyncMethod == "onFailureRequestProducts") {
            var errorCode = OuyaSDK_GetAsyncDataString("errorCode");
            var errorMessage = OuyaSDK_GetAsyncDataString("errorMessage");            
            text_message = "Status: RequestProducts errorCode="+errorCode+" errorMessage="+errorMessage;
        }
        else if (asyncMethod == "onSuccessRequestPurchase") {
            var identifier = OuyaSDK_GetAsyncDataString("identifier");
            text_message = "Status: RequestPurchase identifier="+identifier;
        }
        else if (asyncMethod == "onFailureRequestPurchase") {
            var errorCode = OuyaSDK_GetAsyncDataString("errorCode");
            var errorMessage = OuyaSDK_GetAsyncDataString("errorMessage");            
            text_message = "Status: RequestPurchase errorCode="+errorCode+" errorMessage="+errorMessage;
        }
        else if (asyncMethod == "onCancelRequestPurchase") {
            text_message = "Status: RequestPurchase cancelled!";
        }
        else if (asyncMethod == "onSuccessRequestReceipts") {
            var count = OuyaSDK_GetAsyncDataArrayCount();
            receipts_length = count;
            text_message = "Status: RequestReceipts count="+string(count);
            if (count > 0) {
                text_message = "Status: RequestReceipts count="+string(count);
            }
            for (var index = 0; index < receipts_length; ++index)
            {
                var identifier = OuyaSDK_GetAsyncDataArrayString(string(index), "identifier");
                var localPrice = OuyaSDK_GetAsyncDataArrayDouble(string(index), "localPrice");
                receipts[index] = "Receipt identifier="+identifier+" localPrice="+string(localPrice);
            }
        }
        else if (asyncMethod == "onFailureRequestReceipts") {
            var errorCode = OuyaSDK_GetAsyncDataString("errorCode");
            var errorMessage = OuyaSDK_GetAsyncDataString("errorMessage");            
            text_message = "Status: RequestReceipts errorCode="+errorCode+" errorMessage="+errorMessage;
        }
        else if (asyncMethod == "onCancelRequestReceipts") {
            text_message = "Status: RequestReceipts cancelled!";
        }
    }
    OuyaSDK_PopAsyncResult();
}

