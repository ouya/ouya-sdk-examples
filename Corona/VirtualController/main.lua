-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

local function updateSprite(sprite, x, y, xScale, yScale)

	sprite.x = x;
	sprite.y = y;
	sprite.xScale = xScale;
	sprite.yScale = yScale;

end

local function createController(player, x, y, xScale, yScale)
	local sprite =
	{
		player = player,
		x = x,
		y = y,
		xScale,
		yScale,
	
		--also the render order	
        controller = display.newImage("cutter.png"),
		buttonA = display.newImage("a.png"),
        buttonO = display.newImage("o.png"),
		buttonU = display.newImage("u.png"),
        buttonY = display.newImage("y.png"),
        dpadDown = display.newImage("dpad_down.png"),
        dpadLeft = display.newImage("dpad_left.png"),
        dpadRight = display.newImage("dpad_right.png"),
        dpadUp = display.newImage("dpad_up.png"),
        leftBumper = display.newImage("lb.png"),
        leftTrigger = display.newImage("lt.png"),
        leftStickInactive = display.newImage("l_stick.png"),
        leftStickActive = display.newImage("thumbl.png"),
        rightBumper = display.newImage("rb.png"),
        rightTrigger = display.newImage("rt.png"),
        rightStickInactive = display.newImage("r_stick.png"),
        rightStickActive = display.newImage("thumbr.png")
	};
	
	updateSprite(sprite.buttonA, x, y, xScale, yScale);
	updateSprite(sprite.buttonO, x, y, xScale, yScale);
	updateSprite(sprite.buttonU, x, y, xScale, yScale);
	updateSprite(sprite.buttonY, x, y, xScale, yScale);
	updateSprite(sprite.controller, x, y, xScale, yScale);
	updateSprite(sprite.dpadDown, x, y, xScale, yScale);
	updateSprite(sprite.dpadLeft, x, y, xScale, yScale);
	updateSprite(sprite.dpadRight, x, y, xScale, yScale);
	updateSprite(sprite.dpadUp, x, y, xScale, yScale);
	updateSprite(sprite.leftBumper, x, y, xScale, yScale);
	updateSprite(sprite.leftTrigger, x, y, xScale, yScale);
	updateSprite(sprite.leftStickActive, x, y, xScale, yScale);
	updateSprite(sprite.leftStickInactive, x, y, xScale, yScale);
	updateSprite(sprite.rightBumper, x, y, xScale, yScale);
	updateSprite(sprite.rightTrigger, x, y, xScale, yScale);
	updateSprite(sprite.rightStickActive, x, y, xScale, yScale);
	updateSprite(sprite.rightStickInactive, x, y, xScale, yScale);

	return sprite;
	
end

local controllers =
{
	player1 = createController(0, 150, 500, 2, 2),
	player2 = createController(1, 150, 1200, 2, 2),
	player3 = createController(2, 850, 500, 2, 2),
	player4 = createController(0, 850, 1200, 2, 2)
};