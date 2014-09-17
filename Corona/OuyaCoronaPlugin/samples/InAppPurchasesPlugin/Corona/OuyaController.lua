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
-- OuyaController.lua
--
-----------------------------------------------------------------------------------------

local OuyaController = {}


OuyaController.AXIS_LS_X = 0;
OuyaController.AXIS_LS_Y = 1;
OuyaController.AXIS_RS_X = 11;
OuyaController.AXIS_RS_Y = 14;
OuyaController.AXIS_L2 = 17;
OuyaController.AXIS_R2 = 18;

OuyaController.BUTTON_O = 96;
OuyaController.BUTTON_U = 99;
OuyaController.BUTTON_Y = 100;
OuyaController.BUTTON_A = 97;
OuyaController.BUTTON_L1 = 102;
OuyaController.BUTTON_R1 = 103;
OuyaController.BUTTON_L3 = 106;
OuyaController.BUTTON_R3 = 107;
OuyaController.BUTTON_DPAD_UP = 19;
OuyaController.BUTTON_DPAD_DOWN = 20;
OuyaController.BUTTON_DPAD_RIGHT = 22;
OuyaController.BUTTON_DPAD_LEFT = 21;
OuyaController.BUTTON_MENU = 82;

OuyaController.MAX_CONTROLLERS = 4;

return OuyaController;