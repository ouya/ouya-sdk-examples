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
		
		inputLeftStickX = 0,
		inputLeftStickY = 0,
		inputRightStickX = 0,
		inputRightStickY = 0,
	
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
	createController(1, 150, 500, 2, 2),
	createController(2, 150, 1200, 2, 2),
	createController(3, 850, 500, 2, 2),
	createController(4, 850, 1200, 2, 2)
};

local function spriteFadeOut (spriteObj)
	spriteObj.alpha = 1;
	transition.to(spriteObj, { time=500, alpha=0 })
end

local function spriteFadeIn (spriteObj)
	spriteObj.alpha = 0;
	transition.to(spriteObj, { time=500, alpha=1 })
end

local function updateControllerAxis(spriteObj)
	    -- Fetch the input device's axes.
    local inputAxes = inputDevices[spriteObj.player]:getAxes()
    if #inputAxes > 0 then
        -- Print all available axes to the log.
        for axisIndex = 1, #inputAxes do
            print(inputAxes[axisIndex].descriptor)
        end
    else
        -- Device does not have any axes.
        print(inputDevices[spriteObj.player].descriptor .. ": No axes found.")
    end
end

-- Called when a key event has been received.
local function onKeyEvent( event )
	--print("===== onKeyEvent ======")
	
    if (nil == controllers) then
    	print("controllers not found")
    	return false;
    else
    	--print("controllers found")
    end
    
    --print("Key '" .. event.keyName .. "' has key code: " .. tostring(event.nativeKeyCode))
	
	if event.device then
		--print("### device.descriptor = " .. tostring(event.device.descriptor))
		--print("### device.type = " .. tostring(event.device.type))
		--print("### device.productName = " .. tostring(event.device.productName))
		--print("### device.aliasName = " .. tostring(event.device.aliasName))
		--print("### device.androidDeviceId = " .. tostring(event.device.androidDeviceId))
		--print("### device.permanentStringId = " .. tostring(event.device.permanentStringId))
		--print("### device.canVibrate = " .. tostring(event.device.canVibrate))
		--print("### device.isConnected = " .. tostring(event.device.isConnected))
	else
		print("### device = nil")
		return false;
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
    	--print("controller found")
    end
    
    
	--System Button / Pause Menu
    if (event.keyName == "menu") then
    	print ("menu button detected")
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

-- Called when an axis event has been received.
local function onAxisEvent( event )
	--print("===== onKeyEvent ======")
	
    if (nil == controllers) then
    	print("controllers not found")
    	return false;
    else
    	--print("controllers found")
    end
    
	if event.device then
		--print( event.axis.descriptor .. ": Normalized Value = " .. tostring(event.normalizedValue) )
		--print("### device.descriptor = " .. tostring(event.device.descriptor))
		--print("### device.type = " .. tostring(event.device.type))
		--print("### device.productName = " .. tostring(event.device.productName))
		--print("### device.aliasName = " .. tostring(event.device.aliasName))
		--print("### device.androidDeviceId = " .. tostring(event.device.androidDeviceId))
		--print("### device.permanentStringId = " .. tostring(event.device.permanentStringId))
		--print("### device.canVibrate = " .. tostring(event.device.canVibrate))
		--print("### device.isConnected = " .. tostring(event.device.isConnected))
	else
		print("### device = nil")
		return false;
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
    	--print("controller found")
    end
    
	
	strAxisLX = event.device.descriptor ..  ": Axis 1"
	strAxisLY = event.device.descriptor ..  ": Axis 2"
	strAxisRX = event.device.descriptor ..  ": Axis 4"
	strAxisRY = event.device.descriptor ..  ": Axis 5"
	strAxisLT = event.device.descriptor ..  ": Axis 3"
	strAxisRT = event.device.descriptor ..  ": Axis 6"
	
	AXIS_SCALER = 15;
	
	valAxis = event.normalizedValue;
	if (math.abs(valAxis) < 0.3) then
		valAxis = 0;
	elseif (valAxis < 0) then
		valAxis = (valAxis + 0.3) / 0.7;
	elseif (valAxis > 0) then
		valAxis = (valAxis - 0.3) / 0.7;
	end
	
	--rotate input by N degrees to match image
     local degrees = 135;
     local radians = degrees / 180 * 3.14;
     local valCos = math.cos(radians);
     local valSin = math.sin(radians);
	
	if (tostring(event.axis.descriptor) == strAxisLX) then
		controller.inputLeftStickX = valAxis;
		
		local tempX = controller.inputLeftStickX;
		local tempY = controller.inputLeftStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.leftStickInactive.x = newX;
		controller.leftStickInactive.y = newY;
		controller.leftStickActive.x = newX;
		controller.leftStickActive.y = newY;
			
		--print("LX found x=" .. tostring(controller.x))
	end
	if (tostring(event.axis.descriptor) == strAxisLY) then
		controller.inputLeftStickY = valAxis;
		
		local tempX = controller.inputLeftStickX;
		local tempY = controller.inputLeftStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.leftStickInactive.x = newX;
		controller.leftStickInactive.y = newY;
		controller.leftStickActive.x = newX;
		controller.leftStickActive.y = newY;
		
		--print("LY found")
	end
	if (tostring(event.axis.descriptor) == strAxisRX) then
		controller.inputRightStickX = valAxis;
		
		local tempX = controller.inputRightStickX;
		local tempY = controller.inputRightStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.rightStickInactive.x = newX;
		controller.rightStickInactive.y = newY;
		controller.rightStickActive.x = newX;
		controller.rightStickActive.y = newY;
		--print("RX found")
	end
	if (tostring(event.axis.descriptor) == strAxisRY) then
		controller.inputRightStickY = valAxis;
		
		local tempX = controller.inputRightStickX;
		local tempY = controller.inputRightStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.rightStickInactive.x = newX;
		controller.rightStickInactive.y = newY;
		controller.rightStickActive.x = newX;
		controller.rightStickActive.y = newY;
		--print("RY found")
	end
	if (tostring(event.axis.descriptor) == strAxisLT) then
		--print("LT found")
	end
	if (tostring(event.axis.descriptor) == strAxisRT) then
		--print("RT found")
	end
	
	
--print("what=" .. tostring(event.axis.descriptor))
	
end

-- Add the key event listener.
Runtime:addEventListener( "key", onKeyEvent )

-- Add the axis event listener.
Runtime:addEventListener( "axis", onAxisEvent )