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
-- helpers.lua
--
-----------------------------------------------------------------------------------------

local helpers = {}

helpers.updateSprite = function (spriteObj, x, y, xScale, yScale, alpha)

	spriteObj.x = x;
	spriteObj.y = y;
	spriteObj.xScale = xScale;
	spriteObj.yScale = yScale;
	spriteObj.alpha = alpha;

end

helpers.createButton = function (x, y, xScale, yScale, text, tx, ty, ts)

	local button =
	{
		x = x,
		y = y,
		xScale = xScale,
		yScale = yScale,
		spriteActive = display.newImage("ButtonActive.png"),
		spriteInactive = display.newImage("ButtonInactive.png"),
		txtLabel = display.newText(text, x + tx, y + ty, "Helvetica", ts),
		tx = tx,
		ty = ty,
		btnDown = nil,
		btnLeft = nil,
		btnRight = nil,
		btnUp = nil
	};
	
	helpers.updateSprite(button.spriteActive, x, y, xScale, yScale, 0);
	helpers.updateSprite(button.spriteInactive, x, y, xScale, yScale, 1);
	
	return button;
	
end

-- Fade out the sprite alpha over 500ms
helpers.spriteFadeOut = function (spriteObj)
	if spriteObj == nil then
		print("Using nil spriteObj in spriteFadeOut");
	else
		spriteObj.alpha = 1;	
		transition.to(spriteObj, { time=500, alpha=0 })
	end
end

-- Fade in the sprite alpha over 500ms
helpers.spriteFadeIn = function (spriteObj)
	if spriteObj == nil then
		print("Using nil spriteObj in spriteFadeIn");
	else
		spriteObj.alpha = 0;
		transition.to(spriteObj, { time=500, alpha=1 })
	end
end

return helpers;