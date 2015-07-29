axisScaler = 4;
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

if (OuyaSDK_IsConnected(varPlayer))
{
    draw_sprite(5, -1, x, y); // draw controller behind
    //show_debug_message("connected varPlayer: " + string(varPlayer));
}
else
{
    draw_sprite(17, -1, x, y); // draw controller behind
    //show_debug_message("disconnected varPlayer: " + string(varPlayer));
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_U))
{
    draw_sprite(2, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_Y))
{
    draw_sprite(3, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_A))
{
    draw_sprite(4, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_DPAD_DOWN))
{   
    draw_sprite(6, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_DPAD_LEFT))
{
    draw_sprite(7, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_DPAD_RIGHT))
{
    draw_sprite(8, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_DPAD_UP))
{
    draw_sprite(9, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_L1))
{
    draw_sprite(11, -1, x, y);
}

if (OuyaSDK_GetAxis(varPlayer, AXIS_L2) > deadzone)
{
    draw_sprite(12, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_O))
{
    draw_sprite(13, -1, x, y);
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_R1))
{
    draw_sprite(15, -1, x, y);
}

if (OuyaSDK_GetAxis(varPlayer, AXIS_R2) > deadzone)
{
    draw_sprite(16, -1, x, y);
}

if (OuyaSDK_GetButtonUp(varPlayer, BUTTON_MENU))
{
    menuDetected = current_time + 1000;
    show_debug_message("Menu button detected time="+string(menuDetected));
}

if (menuDetected > current_time)
{
    draw_sprite(18, -1, x, y);
}

//rotate input by N degrees to match image
varDegrees = 135;
varRadians = varDegrees / 180.0 * 3.14;
varCos = cos(varRadians);
varSin = sin(varRadians);
  
varX = OuyaSDK_GetAxis(varPlayer, AXIS_LS_X);
if (varX == undefined)
{
    varX = 0;
}

varY = OuyaSDK_GetAxis(varPlayer, AXIS_LS_Y);
if (varY == undefined)
{
    varY = 0;
}

if (OuyaSDK_GetButton(varPlayer, BUTTON_L3))
{
    draw_sprite(0, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}
else
{
    draw_sprite(10, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}

//rotate by same degrees
varX = OuyaSDK_GetAxis(varPlayer, AXIS_RS_X);
if (varX == undefined)
{
    varX = 0;
}

varY = OuyaSDK_GetAxis(varPlayer, AXIS_RS_Y);
if (varY == undefined)
{
    varY = 0;
}
    
if (OuyaSDK_GetButton(varPlayer, BUTTON_R3))
{
    draw_sprite(1, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}
else
{
    draw_sprite(14, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}