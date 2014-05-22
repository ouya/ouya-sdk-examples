using System;
using System.Collections.Generic;

namespace OuyaSdk
{
    public class VirtualController
    {
        /// <summary>
        /// The controller index
        /// </summary>
        public int Index = 0;

        /// <summary>
        /// Axis States
        /// </summary>
        public Dictionary<int, float> Axis = new Dictionary<int, float>();

        /// <summary>
        /// Button States
        /// </summary>
        public Dictionary<int, bool> Button = new Dictionary<int, bool>();

        /// <summary>
        /// The Menu button was detected
        /// </summary>
        public DateTime TimerMenuDetected = DateTime.MinValue;

        // check if a button is pressed
        public bool IsPressed(int keyCode)
        {
	        if (Button.ContainsKey(keyCode))
	        {
                return Button[keyCode];
	        }
	        return false;
        }

        public float GetAxis(int axis)
        {
            if (Axis.ContainsKey(axis))
            {
                return Axis[axis];
            }
            return 0f;
        }
    }
}