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

draw_text(150, 100, "Hello from Game Maker!");

draw_sprite(0, -1, 150, 200);
draw_text(150, 200, "Request Products");

draw_sprite(0, -1, 500, 200);
draw_text(150, 200, "Request Purchase");

draw_sprite(0, -1, 800, 200);
draw_text(150, 200, "Request Receipts");

draw_sprite(0, -1, 1100, 200);
draw_text(150, 200, "Request GamerInfo");

draw_sprite(0, -1, 1400, 200);
draw_text(150, 200, "Pause");

draw_sprite(0, -1, 1650, 200);
draw_text(150, 200, "Exit");

/*
if (varPlayer == undefined)
{
    varPlayer = "0";
}

if (OuyaSDK_IsConnected(varPlayer))
{
    //show_debug_message("connected varPlayer: " + string(varPlayer));
}
*/
