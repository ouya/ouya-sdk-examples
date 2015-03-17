using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using System;

namespace TV.Ouya.Console.Api{
    public partial class OuyaController
    {
        public static int AXIS_LS_X = 0;
        public static int AXIS_LS_Y = 1;
        public static int AXIS_RS_X = 11;
        public static int AXIS_RS_Y = 14;
        public static int AXIS_L2 = 17;
        public static int AXIS_R2 = 18;

        public static int BUTTON_O = 96;
        public static int BUTTON_U = 99;
        public static int BUTTON_Y = 100;
        public static int BUTTON_A = 97;
        public static int BUTTON_L1 = 102;
        public static int BUTTON_R1 = 103;
        public static int BUTTON_L3 = 106;
        public static int BUTTON_R3 = 107;
        public static int BUTTON_DPAD_UP = 19;
        public static int BUTTON_DPAD_DOWN = 20;
        public static int BUTTON_DPAD_RIGHT = 22;
        public static int BUTTON_DPAD_LEFT = 21;
        public static int BUTTON_MENU = 82;

        public static int MAX_CONTROLLERS = 4;
    }
}
