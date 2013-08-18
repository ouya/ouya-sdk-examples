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
-- main.lua
--
-----------------------------------------------------------------------------------------

globals = require "globals"
helpers = require "helpers"
inputs = require "inputs"
ui = require "ui"

globals.controllers =
{
	ui.createController(1, 150, 500, 2, 2),
	ui.createController(2, 150, 1200, 2, 2),
	ui.createController(3, 850, 500, 2, 2),
	ui.createController(4, 850, 1200, 2, 2)
};

-- Add the key event listener.
Runtime:addEventListener( "key", inputs.onKeyEvent )

-- Add the axis event listener.
Runtime:addEventListener( "axis", inputs.onAxisEvent )