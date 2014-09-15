-- Copyright (C) 2012, 2013 OUYA, Inc.
--
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
--
--    http://www.apache.org/licenses/LICENSE-2.0
--
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.

-----------------------------------------------------------------------------------------
--
-- inputs.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
helpers = require "helpers"
OuyaController = require "OuyaController"

local inputs = {}


inputs.onGenericMotionEvent = function (playerNum, axis, val)
	--print("inputs.onGenericMotionEvent: playerNum=" .. tostring(playerNum) .. " axis=" .. tostring(axis) .. " val=" .. tostring(val));
	
	if playerNum < 0 or playerNum > OuyaController.MAX_CONTROLLERS then
		return false;
	end

	local index = playerNum + 1;

	local controller = globals.controllers[index]
    if (nil == controller) then
    	print("controller not found")
    	return false;
    else
    	--print("controller found")
    end
    
	local AXIS_SCALER = 10;
	local DEAD_ZONE = 0.3;
	
	local valAxis = val;
	if (math.abs(valAxis) < DEAD_ZONE) then
		valAxis = 0;
	elseif (valAxis < 0) then
		valAxis = (valAxis + DEAD_ZONE) / 0.7;
	elseif (valAxis > 0) then
		valAxis = (valAxis - DEAD_ZONE) / 0.7;
	end
	
	--rotate input by N degrees to match image
     local degrees = 135;
     local radians = degrees / 180 * 3.14;
     local valCos = math.cos(radians);
     local valSin = math.sin(radians);
	
	if (axis == OuyaController.AXIS_LS_X) then
		--print("LX found x=" .. tostring(controller.x))
		
		controller.inputLeftStickX = valAxis;
		
		local tempX = controller.inputLeftStickX;
		local tempY = controller.inputLeftStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.leftStickInactive.x = newX;
		controller.leftStickInactive.y = newY;
		controller.leftStickActive.x = newX;
		controller.leftStickActive.y = newY;
	end
	
	if (axis == OuyaController.AXIS_LS_Y) then
		--print("LY found")
		
		controller.inputLeftStickY = valAxis;
		
		local tempX = controller.inputLeftStickX;
		local tempY = controller.inputLeftStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.leftStickInactive.x = newX;
		controller.leftStickInactive.y = newY;
		controller.leftStickActive.x = newX;
		controller.leftStickActive.y = newY;
	end
	
	if (axis == OuyaController.AXIS_RS_X) then
		--print("RX found")
		
		controller.inputRightStickX = valAxis;
		
		local tempX = controller.inputRightStickX;
		local tempY = controller.inputRightStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.rightStickInactive.x = newX;
		controller.rightStickInactive.y = newY;
		controller.rightStickActive.x = newX;
		controller.rightStickActive.y = newY;
	end
	
	if (axis == OuyaController.AXIS_RS_Y) then
		--print("RY found")
		
		controller.inputRightStickY = valAxis;
		
		local tempX = controller.inputRightStickX;
		local tempY = controller.inputRightStickY;
		
		local newX = controller.x + AXIS_SCALER * (tempX * valCos - tempY * valSin);
		local newY = controller.y + AXIS_SCALER * (tempX * valSin + tempY * valCos);
		
		controller.rightStickInactive.x = newX;
		controller.rightStickInactive.y = newY;
		controller.rightStickActive.x = newX;
		controller.rightStickActive.y = newY;
	end

	if axis == OuyaController.AXIS_L2 then
		if valAxis > DEAD_ZONE then
			--print("LT found")
			helpers.spriteFadeAuto(true, controller.leftTrigger);
		else
			helpers.spriteFadeAuto(false, controller.leftTrigger);
		end
	end
	
	if axis == OuyaController.AXIS_R2 then
		if valAxis > DEAD_ZONE then
			--print("RT found")
			helpers.spriteFadeAuto(true, controller.rightTrigger);
		else
			helpers.spriteFadeAuto(false, controller.rightTrigger);
		end
	end

end

inputs.updateVisibility = function (playerNum, button, phase)
	if playerNum < 0 or playerNum > OuyaController.MAX_CONTROLLERS then
		return false;
	end

	local index = playerNum + 1;

	local controller = globals.controllers[index]
    if (nil == controller) then
    	print("controller not found")
    	return false;
    else
    	--print("controller found")
    end

    -- set the joystick name
    --controller.txtLabel.text = tostring(event.device.displayName);

    --DPADS
    if (button == OuyaController.BUTTON_DPAD_DOWN) then
    	helpers.spriteFadeAuto(phase, controller.dpadDown)
    end
    
    if (button == OuyaController.BUTTON_DPAD_LEFT) then
    	helpers.spriteFadeAuto(phase, controller.dpadLeft)
    end
    
    if (button == OuyaController.BUTTON_DPAD_RIGHT) then
    	helpers.spriteFadeAuto(phase, controller.dpadRight)
    end
    
    if (button == OuyaController.BUTTON_DPAD_UP) then
    	helpers.spriteFadeAuto(phase, controller.dpadUp)
    end
    
    --end of DPADS
    
    --STICK BUTTONS
    if (button == OuyaController.BUTTON_L3) then
    	helpers.spriteFadeAuto(phase, controller.leftStickActive)
    	helpers.spriteFadeAutoInv(phase, controller.leftStickInactive)
    end

	if (button == OuyaController.BUTTON_R3) then
    	helpers.spriteFadeAuto(phase, controller.rightStickActive)
    	helpers.spriteFadeAutoInv(phase, controller.rightStickInactive)
    end
    --end of STICK BUTTONS

    -- BUTTONS
    
    if (button == OuyaController.BUTTON_U) then
    	helpers.spriteFadeAuto(phase, controller.buttonU)
    end
    
    if (button == OuyaController.BUTTON_O) then
    	helpers.spriteFadeAuto(phase, controller.buttonO)
    end
    
    if (button == OuyaController.BUTTON_A) then
    	helpers.spriteFadeAuto(phase, controller.buttonA)
    end
    
    if (button == OuyaController.BUTTON_Y) then
    	helpers.spriteFadeAuto(phase, controller.buttonY)
    end
    
    if (button == OuyaController.BUTTON_L1) then
    	helpers.spriteFadeAuto(phase, controller.leftBumper)
    end
    
    if (button == OuyaController.BUTTON_R1) then
    	helpers.spriteFadeAuto(phase, controller.rightBumper)
    end

    if (button == OuyaController.BUTTON_MENU) then
    	if (phase) then
    		controller.buttonMenu.alpha = 1;
    		helpers.spriteFadeOut(controller.buttonMenu);
    	end
    end
    
    -- End of BUTTONS
end

inputs.onKeyDown = function (playerNum, button)
	--print("inputs.onKeyDown: playerNum=" .. tostring(playerNum) .. " button=" .. tostring(button));
	inputs.updateVisibility(playerNum, button, true);
end

inputs.onKeyUp = function (playerNum, button)
	--print("inputs.onKeyUp: playerNum=" .. tostring(playerNum) .. " button=" .. tostring(button));
	inputs.updateVisibility(playerNum, button, false);
end


if nil ~= ouyaSDK then
	ouyaSDK.asyncLuaOuyaInitInput(inputs.onGenericMotionEvent, inputs.onKeyDown, inputs.onKeyUp);
end

-- Called when an axis event has been received.
inputs.onAxisEvent = function (event)
	--print("===== onKeyEvent ======")
	return false;	
end

-- Called when a key event has been received.
inputs.onKeyEvent = function (event)
	--print("===== onKeyEvent ======")

	if true then
		return false
	end
	
    if (nil == globals.controllers) then
    	print("controllers not found")
    	return false;
    else
    	--print("controllers found")
    end
    
    --print("Key '" .. event.keyName .. "' has key code: " .. tostring(event.nativeKeyCode) .. " phase: " .. event.phase);
	
	if event.device then
		--print("### device.displayName = " .. tostring(event.device.displayName))
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
	
	local index = 1
	
	if (tostring(event.device.descriptor) == "Joystick 1") then
		index = 1;
	elseif (tostring(event.device.descriptor) == "Joystick 2") then
		index = 2;
	elseif (tostring(event.device.descriptor) == "Joystick 3") then
		index = 3;
	elseif (tostring(event.device.descriptor) == "Joystick 4") then
		index = 4;
	end

	
	local controller = globals.controllers[index]
    if (nil == controller) then
    	print("controller not found")
    	return false;
    else
    	--print("controller found")
    end
    
    -- set the joystick name
    controller.txtLabel.text = tostring(event.device.displayName);
    
	--System Button / Pause Menu
    if (event.keyName == "menu") then
    	if (event.phase == "up") then
    		print ("menu button detected")
    	end
    end
	
	--DPADS
    if (event.keyName == "down") then
    	helpers.spriteFadeAuto(event.phase, controller.dpadDown)
    end
    
    if (event.keyName == "left") then
    	helpers.spriteFadeAuto(event.phase, controller.dpadLeft)
    end
    
    if (event.keyName == "right") then
    	helpers.spriteFadeAuto(event.phase, controller.dpadRight)
    end
    
    if (event.keyName == "up") then
    	helpers.spriteFadeAuto(event.phase, controller.dpadUp)
    end
    
    --end of DPADS
    
    if (event.keyName == "leftJoystickButton") then
    	helpers.spriteFadeAuto(event.phase, controller.leftStickActive)
    	helpers.spriteFadeAutoInv(event.phase, controller.leftStickInactive)
    end

	-- BUTTONS

    if (event.keyName == "rightJoystickButton") then
    	helpers.spriteFadeAuto(event.phase, controller.rightStickActive)
    	helpers.spriteFadeAutoInv(event.phase, controller.rightStickInactive)
    end
    
    if (event.keyName == "buttonX") then
    	helpers.spriteFadeAuto(event.phase, controller.buttonU)
    end
    
    if (event.keyName == "buttonA") then
    	helpers.spriteFadeAuto(event.phase, controller.buttonO)
    end
    
    if (event.keyName == "buttonB") then
    	helpers.spriteFadeAuto(event.phase, controller.buttonA)
    end
    
    if (event.keyName == "buttonY") then
    	helpers.spriteFadeAuto(event.phase, controller.buttonY)
    end
    
    if (event.keyName == "leftShoulderButton1") then
    	helpers.spriteFadeAuto(event.phase, controller.leftBumper)
    end
    
    if (event.keyName == "leftShoulderButton2") then
    	helpers.spriteFadeAuto(event.phase, controller.leftTrigger)
    end
    
    if (event.keyName == "rightShoulderButton1") then
    	helpers.spriteFadeAuto(event.phase, controller.rightBumper)
    end
    
    if (event.keyName == "rightShoulderButton2") then
    	helpers.spriteFadeAuto(event.phase, controller.rightTrigger)
    end
    
    -- End of BUTTONS
	
	return false
end

return inputs;