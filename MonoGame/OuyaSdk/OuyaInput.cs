using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Android.Util;

namespace OuyaSdk
{
    public class OuyaInput
    {
        public class NdkWrapper
        {
            [DllImport("lib-ouya-ndk")]
            // EXPORT_API float getAxis(int deviceId, int axis)
            public static extern float getAxis(int deviceId, int axis);

            [DllImport("lib-ouya-ndk")]
            // EXPORT_API bool isPressed(int deviceId, int keyCode)
            public static extern bool isPressed(int deviceId, int keyCode);

            [DllImport("lib-ouya-ndk")]
            // EXPORT_API bool isPressedDown(int deviceId, int keyCode)
            public static extern bool isPressedDown(int deviceId, int keyCode);

            [DllImport("lib-ouya-ndk")]
            // EXPORT_API bool isPressedUp(int deviceId, int keyCode)
            public static extern bool isPressedUp(int deviceId, int keyCode);

            [DllImport("lib-ouya-ndk")]
            // EXPORT_API void clearButtonStates()
            public static extern void clearButtonStates();
        }

        private const string TAG = "OuyaInput";

        #region Private API

        private static object m_lockObject = new object();
        private static List<Dictionary<int, float>> m_axisStates = new List<Dictionary<int, float>>();
        private static List<Dictionary<int, bool>> m_buttonStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonDownStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonUpStates = new List<Dictionary<int, bool>>();

        static OuyaInput()
        {
            for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
            {
                m_axisStates.Add(new Dictionary<int, float>());
                m_buttonStates.Add(new Dictionary<int, bool>());
                m_buttonDownStates.Add(new Dictionary<int, bool>());
                m_buttonUpStates.Add(new Dictionary<int, bool>());
            }
        }

        private static float GetState(int axis, Dictionary<int, float> dictionary)
        {
            float result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(axis))
                {
                    result = dictionary[axis];
                }
                else
                {
                    result = 0f;
                }
            }
            return result;
        }

        private static bool GetState(int button, Dictionary<int, bool> dictionary)
        {
            bool result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(button))
                {
                    result = dictionary[button];
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private static bool GetState(bool isPressed, int button, Dictionary<int, bool> dictionary)
        {
            bool result;
            lock (m_lockObject)
            {
                if (dictionary.ContainsKey(button))
                {
                    result = isPressed == dictionary[button];
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public static void UpdateInputFrame()
        {
            lock (m_lockObject)
            {
                for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
                {
                    #region Track Axis States

                    Dictionary<int, float> axisState = m_axisStates[deviceId];
                    axisState[OuyaController.AXIS_LS_X] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_LS_X);
                    axisState[OuyaController.AXIS_LS_Y] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_LS_Y);
                    axisState[OuyaController.AXIS_RS_X] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_RS_X);
                    axisState[OuyaController.AXIS_RS_Y] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_RS_Y);
                    axisState[OuyaController.AXIS_L2] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_L2);
                    axisState[OuyaController.AXIS_R2] = NdkWrapper.getAxis(deviceId, OuyaController.AXIS_R2);

                    #endregion

                    #region Track Button Up / Down States

                    Dictionary<int, bool> buttonState = m_buttonStates[deviceId];
                    Dictionary<int, bool> buttonDownState = m_buttonDownStates[deviceId];
                    Dictionary<int, bool> buttonUpState = m_buttonUpStates[deviceId];

                    buttonState[OuyaController.BUTTON_O] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_O);
                    buttonState[OuyaController.BUTTON_U] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_U);
                    buttonState[OuyaController.BUTTON_Y] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_Y);
                    buttonState[OuyaController.BUTTON_A] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_A);
                    buttonState[OuyaController.BUTTON_L1] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_L1);
                    buttonState[OuyaController.BUTTON_R1] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_R1);
                    buttonState[OuyaController.BUTTON_L3] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_L3);
                    buttonState[OuyaController.BUTTON_R3] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_R3);
                    buttonState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressed(deviceId,
                        OuyaController.BUTTON_DPAD_UP);
                    buttonState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressed(deviceId,
                        OuyaController.BUTTON_DPAD_DOWN);
                    buttonState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressed(deviceId,
                        OuyaController.BUTTON_DPAD_RIGHT);
                    buttonState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressed(deviceId,
                        OuyaController.BUTTON_DPAD_LEFT);
                    buttonState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_MENU);

                    buttonDownState[OuyaController.BUTTON_O] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_O);
                    buttonDownState[OuyaController.BUTTON_U] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_U);
                    buttonDownState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_Y);
                    buttonDownState[OuyaController.BUTTON_A] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_A);
                    buttonDownState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_L1);
                    buttonDownState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_R1);
                    buttonDownState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_L3);
                    buttonDownState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_R3);
                    buttonDownState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_DPAD_UP);
                    buttonDownState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_DPAD_DOWN);
                    buttonDownState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_DPAD_RIGHT);
                    buttonDownState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_DPAD_LEFT);
                    buttonDownState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedDown(deviceId,
                        OuyaController.BUTTON_MENU);

                    buttonUpState[OuyaController.BUTTON_O] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_O);
                    buttonUpState[OuyaController.BUTTON_U] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_U);
                    buttonUpState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_Y);
                    buttonUpState[OuyaController.BUTTON_A] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_A);
                    buttonUpState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L1);
                    buttonUpState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R1);
                    buttonUpState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L3);
                    buttonUpState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R3);
                    buttonUpState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedUp(deviceId,
                        OuyaController.BUTTON_DPAD_UP);
                    buttonUpState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedUp(deviceId,
                        OuyaController.BUTTON_DPAD_DOWN);
                    buttonUpState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedUp(deviceId,
                        OuyaController.BUTTON_DPAD_RIGHT);
                    buttonUpState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedUp(deviceId,
                        OuyaController.BUTTON_DPAD_LEFT);
                    buttonUpState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedUp(deviceId,
                        OuyaController.BUTTON_MENU);

                    #endregion

                    //debugOuyaController(deviceId);
                }
            }
        }

        public static void ClearButtonStates()
        {
            NdkWrapper.clearButtonStates();
        }

        private static void debugOuyaController(int deviceId, int button)
        {
            if (GetButtonDown(deviceId, button))
            {
                Log.Info(TAG, "Device=" + deviceId + " GetButtonDown: " + button);
            }

            if (GetButtonUp(deviceId, button))
            {
                Log.Info(TAG, "Device=" + deviceId + " GetButtonUp: " + button);
            }
        }

        private static void debugOuyaController(int deviceId)
        {
            debugOuyaController(deviceId, OuyaController.BUTTON_O);
            debugOuyaController(deviceId, OuyaController.BUTTON_U);
            debugOuyaController(deviceId, OuyaController.BUTTON_Y);
            debugOuyaController(deviceId, OuyaController.BUTTON_A);
            debugOuyaController(deviceId, OuyaController.BUTTON_L1);
            debugOuyaController(deviceId, OuyaController.BUTTON_R1);
            debugOuyaController(deviceId, OuyaController.BUTTON_L3);
            debugOuyaController(deviceId, OuyaController.BUTTON_R3);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_UP);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_DOWN);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
            debugOuyaController(deviceId, OuyaController.BUTTON_DPAD_LEFT);
            debugOuyaController(deviceId, OuyaController.BUTTON_MENU);
        }

        #endregion

        #region Public API

        public static float GetAxis(int playerNum, int axis)
        {
            if (playerNum >= 0 &&
                null != m_axisStates &&
                playerNum < m_axisStates.Count)
            {
                return GetState(axis, m_axisStates[playerNum]);
            }
            else
            {
                return 0f;
            }
        }

        public static bool GetButton(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonStates &&
                playerNum < m_buttonStates.Count)
            {
                return GetState(button, m_buttonStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static bool GetButtonDown(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonDownStates &&
                playerNum < m_buttonDownStates.Count)
            {
                return GetState(button, m_buttonDownStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static bool GetButtonUp(int playerNum, int button)
        {
            if (playerNum >= 0 &&
                null != m_buttonUpStates &&
                playerNum < m_buttonUpStates.Count)
            {
                return GetState(button, m_buttonUpStates[playerNum]);
            }
            else
            {
                return false;
            }
        }

        #endregion    
    }
}