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

-- Fade out the sprite alpha over 500ms
helpers.spriteFadeOut = function (spriteObj)
	spriteObj.alpha = 1;
	transition.to(spriteObj, { time=500, alpha=0 })
end

-- Fade in the sprite alpha over 500ms
helpers.spriteFadeIn = function (spriteObj)
	spriteObj.alpha = 0;
	transition.to(spriteObj, { time=500, alpha=1 })
end

-- Auto fade the sprite based on the phase, fade In if down, fade out if up
helpers.spriteFadeAuto = function (phase, spriteObj)
    	if (phase == "down") then
    		helpers.spriteFadeIn(spriteObj)
    	elseif (phase == "up") then
    		helpers.spriteFadeOut(spriteObj)    	
    	end
end

-- Invert auto fade the sprite based on the phase, fade In if up, fade out if down
helpers.spriteFadeAutoInv = function (phase, spriteObj)
    	if (phase == "up") then
    		helpers.spriteFadeIn(spriteObj)
    	elseif (phase == "down") then
    		helpers.spriteFadeOut(spriteObj)    	
    	end
end

return helpers;