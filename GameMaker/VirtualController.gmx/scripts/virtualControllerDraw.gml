//show_debug_message("varPlayer: " + string(varPlayer));

axisScaler = 4;

draw_sprite(5, -1, x, y); // draw controller behind

if (gamepad_button_check(varPlayer, gp_face3))
{
    draw_sprite(2, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face4))
{
    draw_sprite(3, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face2))
{
    draw_sprite(4, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padd))
{   
    draw_sprite(6, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padl))
{
    draw_sprite(7, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padr))
{
    draw_sprite(8, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_padu))
{
    draw_sprite(9, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderl))
{
    draw_sprite(11, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderlb))
{
    draw_sprite(12, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_face1))
{
    draw_sprite(13, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderl))
{
    draw_sprite(15, -1, x, y);
}

if (gamepad_button_check(varPlayer, gp_shoulderrb))
{
    draw_sprite(16, -1, x, y);
}

//left stick
if (gamepad_button_check(varPlayer, gp_stickl))
{
    draw_sprite(0, -1, x + axisScaler * gamepad_axis_value(varPlayer, gp_axislh), y + axisScaler * gamepad_axis_value(varPlayer, gp_axislv));
}
else
{
    draw_sprite(10, -1, x + axisScaler * gamepad_axis_value(varPlayer, gp_axislh), y + axisScaler * gamepad_axis_value(varPlayer, gp_axislv));
}

//right stick
if (gamepad_button_check(varPlayer, gp_stickr))
{
    draw_sprite(1, -1, x + axisScaler * gamepad_axis_value(varPlayer, gp_axisrh), y + axisScaler * gamepad_axis_value(varPlayer, gp_axisrv));
}
else
{
    draw_sprite(14, -1, x + axisScaler * gamepad_axis_value(varPlayer, gp_axisrh), y + axisScaler * gamepad_axis_value(varPlayer, gp_axisrv));
}
