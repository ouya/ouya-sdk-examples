axisScaler = 4;

if (gamepad_is_connected(varPlayer))
{
    draw_sprite(5, -1, x, y); // draw controller behind
    //show_debug_message("connected varPlayer: " + string(varPlayer));
}
else
{
    draw_sprite(17, -1, x, y); // draw controller behind
    //show_debug_message("disconnected varPlayer: " + string(varPlayer));
}

if (gamepad_button_check(varPlayer, gp_face3)) //BUTTON_Y
{
    draw_sprite(2, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face4)) //BUTTON_A
{
    draw_sprite(3, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face2)) //BUTTON_U
{
    draw_sprite(4, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padd)) //DPAD_DOWN
{   
    draw_sprite(6, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padl)) //DPAD_LEFT
{
    draw_sprite(7, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padr)) //DPAD_RIGHT
{
    draw_sprite(8, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padu)) //DPAD_UP
{
    draw_sprite(9, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderl)) //LEFT_BUMPER
{
    draw_sprite(11, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderlb)) //LEFT_TRIGGER
{
    draw_sprite(12, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face1)) //BUTTON_O
{
    draw_sprite(13, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderr)) //RIGHT_BUMPER
{
    draw_sprite(15, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderrb)) //RIGHT_TRIGGER
{
    draw_sprite(16, -1, x, y);
}

//rotate input by N degrees to match image
varDegrees = 135;
varRadians = varDegrees / 180.0 * 3.14;
varCos = cos(varRadians);
varSin = sin(varRadians);
    
varX = gamepad_axis_value(varPlayer, gp_axislh);
varY = gamepad_axis_value(varPlayer, gp_axislv);

//left stick
if (gamepad_button_check(varPlayer, gp_stickl)) //LEFT_STICK
{
    draw_sprite(0, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}
else
{
    draw_sprite(10, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}

//rotate by same degrees
varX = gamepad_axis_value(varPlayer, gp_axisrh);
varY = gamepad_axis_value(varPlayer, gp_axisrv);
    
//right stick
if (gamepad_button_check(varPlayer, gp_stickr)) //RIGHT_STICK
{
    draw_sprite(1, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}
else
{
    draw_sprite(14, -1, x + axisScaler * (varX * varCos - varY * varSin), y + axisScaler * (varX * varSin + varY * varCos));
}
