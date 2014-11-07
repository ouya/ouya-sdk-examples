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
-- ui.lua
--
-----------------------------------------------------------------------------------------

helpers = require "helpers"

local ui = {}

display.newText("Virtual Controllers for Corona", 500, 200, "Helvetica", 48);

ui.createController = function (player, x, y, xScale, yScale)
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
        controller = display.newImage("controller.png"),
		buttonA = display.newImage("a.png"),
		buttonMenu = display.newImage("menu.png"),
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
        rightStickActive = display.newImage("thumbr.png"),
        
        checkForName = true;
        txtLabel = display.newText("Unknown", x - 100, y + 250, "Helvetica", 48);
	};
	
	helpers.updateSprite(spriteObj.buttonA, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.buttonMenu, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.buttonO, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.buttonU, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.buttonY, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.controller, x, y, xScale, yScale, 1);
	helpers.updateSprite(spriteObj.dpadDown, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.dpadLeft, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.dpadRight, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.dpadUp, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.leftBumper, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.leftTrigger, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.leftStickActive, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.leftStickInactive, x, y, xScale, yScale, 1);
	helpers.updateSprite(spriteObj.rightBumper, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.rightTrigger, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.rightStickActive, x, y, xScale, yScale, 0);
	helpers.updateSprite(spriteObj.rightStickInactive, x, y, xScale, yScale, 1);

	return spriteObj;
	
end

return ui;