-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

local function updateSprite(spriteObj, x, y, xScale, yScale, alpha)

	spriteObj.x = x;
	spriteObj.y = y;
	spriteObj.xScale = xScale;
	spriteObj.yScale = yScale;
	spriteObj.alpha = alpha;

end

local function createController(player, x, y, xScale, yScale)
	local spriteObj =
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
	
	updateSprite(spriteObj.buttonA, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.buttonO, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.buttonU, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.buttonY, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.controller, x, y, xScale, yScale, 1);
	updateSprite(spriteObj.dpadDown, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.dpadLeft, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.dpadRight, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.dpadUp, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.leftBumper, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.leftTrigger, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.leftStickActive, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.leftStickInactive, x, y, xScale, yScale, 1);
	updateSprite(spriteObj.rightBumper, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.rightTrigger, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.rightStickActive, x, y, xScale, yScale, 0);
	updateSprite(spriteObj.rightStickInactive, x, y, xScale, yScale, 1);

	return spriteObj;
	
end

controllers =
{
	createController(0, 150, 500, 2, 2),
	createController(1, 150, 1200, 2, 2),
	createController(2, 850, 500, 2, 2),
	createController(0, 850, 1200, 2, 2)
};

local function spriteFadeOut (spriteObj)
	spriteObj.alpha = 1;
	transition.to(spriteObj, { time=500, alpha=0 })
end

local function spriteFadeIn (spriteObj)
	spriteObj.alpha = 0;
	transition.to(spriteObj, { time=500, alpha=1 })
end

-- Called when a key event has been received.
local function onKeyEvent( event )
	print("===== onKeyEvent ======")
	
    print("Key '" .. event.keyName .. "' has key code: " .. tostring(event.nativeKeyCode))
	
	if event.device then
		print("### device.descriptor = " .. tostring(event.device.descriptor))
		print("### device.type = " .. tostring(event.device.type))
		print("### device.productName = " .. tostring(event.device.productName))
		print("### device.aliasName = " .. tostring(event.device.aliasName))
		print("### device.androidDeviceId = " .. tostring(event.device.androidDeviceId))
		print("### device.permanentStringId = " .. tostring(event.device.permanentStringId))
		print("### device.canVibrate = " .. tostring(event.device.canVibrate))
		print("### device.isConnected = " .. tostring(event.device.isConnected))
	else
		print("### device = nil")
	end
	
    if (nil == controllers) then
    	print("controllers not found")
    	return false;
    else
    	print("controllers found")
    end
	
	index = 1
	
	if (tostring(event.device.descriptor) == "Joystick 1") then
		index = 1;
	elseif (tostring(event.device.descriptor) == "Joystick 2") then
		index = 2;
	elseif (tostring(event.device.descriptor) == "Joystick 3") then
		index = 3;
	elseif (tostring(event.device.descriptor) == "Joystick 4") then
		index = 4;
	end

	
	controller = controllers[index]
    if (nil == controller) then
    	print("controller not found")
    	return false;
    else
    	print("controller found")
    end
	
	--DPADS
    if (event.keyName == "down") then
    	spriteFadeOut(controller.dpadDown)
    end
    
    if (event.keyName == "left") then
    	spriteFadeOut(controller.dpadLeft)
    end
    
    if (event.keyName == "right") then
    	spriteFadeOut(controller.dpadRight)
    end
    
    if (event.keyName == "up") then
    	spriteFadeOut(controller.dpadUp)
    end
    
    --end of DPADS
    
    if (event.keyName == "leftJoystickButton") then
    	spriteFadeOut(controller.leftStickActive)
    	spriteFadeIn(controller.leftStickInactive)
    end

	-- BUTTONS

    if (event.keyName == "rightJoystickButton") then
    	spriteFadeOut(controller.rightStickActive)
    	spriteFadeIn(controller.rightStickInactive)
    end
    
    if (event.keyName == "buttonX") then
    	spriteFadeOut(controller.buttonU)
    end
    
    if (event.keyName == "buttonA") then
    	spriteFadeOut(controller.buttonO)
    end
    
    if (event.keyName == "buttonB") then
    	spriteFadeOut(controller.buttonA)
    end
    
    if (event.keyName == "buttonY") then
    	spriteFadeOut(controller.buttonY)
    end
    
    if (event.keyName == "leftShoulderButton1") then
    	spriteFadeOut(controller.leftBumper)
    end
    
    if (event.keyName == "leftShoulderButton2") then
    	spriteFadeOut(controller.leftTrigger)
    end
    
    if (event.keyName == "rightShoulderButton1") then
    	spriteFadeOut(controller.rightBumper)
    end
    
    if (event.keyName == "rightShoulderButton2") then
    	spriteFadeOut(controller.rightTrigger)
    end
    
    -- End of BUTTONS
	
	return false
end


-- Add the key event listener.
Runtime:addEventListener( "key", onKeyEvent )