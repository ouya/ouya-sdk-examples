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

if (!OuyaSDK_IsAnyConnected())
{
    text_message = "Status: No controllers are connected!";
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
    }
    
    if (button_index == 1)
    {
        text_message = "Status: Requesting purchase...";
    }    
    
    if (button_index == 2)
    {
        text_message = "Status: Requesting receipts...";
    }    
    
    if (button_index == 3)
    {
        text_message = "Status: Requesting gamer info...";
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

