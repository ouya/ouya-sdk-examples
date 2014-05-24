/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// Unity JNI reference: http://docs.unity3d.com/Documentation/ScriptReference/AndroidJNI.html
// JNI Spec: http://docs.oracle.com/javase/1.5.0/docs/guide/jni/spec/jniTOC.html
// Android Plugins: http://docs.unity3d.com/Documentation/Manual/Plugins.html#AndroidPlugins 
// Unity Android Plugin Guide: http://docs.unity3d.com/Documentation/Manual/PluginsForAndroid.html

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;

public static class OuyaSDK
{
    public const string VERSION = "1.0.12.5";

#if UNITY_ANDROID && !UNITY_EDITOR

    public const int MAX_CONTROLLERS = 4;

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
#endif

#if UNITY_ANDROID && !UNITY_EDITOR

    public class OuyaInput
    {
        #region Private API

        private static List<Dictionary<int, float>> m_axisStates = new List<Dictionary<int, float>>();
        private static List<Dictionary<int, bool>> m_buttonStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonDownStates = new List<Dictionary<int, bool>>();
        private static List<Dictionary<int, bool>> m_buttonUpStates = new List<Dictionary<int, bool>>();

        static OuyaInput()
        {
            for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
            {
                m_axisStates.Add(new Dictionary<int, float>());
                m_buttonStates.Add(new Dictionary<int, bool>());
                m_buttonDownStates.Add(new Dictionary<int, bool>());
                m_buttonUpStates.Add(new Dictionary<int, bool>());
            }
        }

        private static float GetState(int axis, Dictionary<int, float> dictionary)
        {
            if (dictionary.ContainsKey(axis))
            {
                return dictionary[axis];
            }
            else
            {
                return 0f;
            }
        }

        private static bool GetState(int button, Dictionary<int, bool> dictionary)
        {
            if (dictionary.ContainsKey(button))
            {
                return dictionary[button];
            }
            else
            {
                return false;
            }
        }

        private static bool GetState(bool isPressed, int button, Dictionary<int, bool> dictionary)
        {
            if (dictionary.ContainsKey(button))
            {
                return isPressed == dictionary[button];
            }
            else
            {
                return false;
            }
        }

        public static void UpdateInputFrame()
        {
            for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
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
                buttonState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_UP);
                buttonState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                buttonState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                buttonState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                buttonState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressed(deviceId, OuyaController.BUTTON_MENU);

                buttonDownState[OuyaController.BUTTON_O] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_O);
                buttonDownState[OuyaController.BUTTON_U] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_U);
                buttonDownState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_Y);
                buttonDownState[OuyaController.BUTTON_A] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_A);
                buttonDownState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_L1);
                buttonDownState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_R1);
                buttonDownState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_L3);
                buttonDownState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_R3);
                buttonDownState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_UP);
                buttonDownState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                buttonDownState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                buttonDownState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                buttonDownState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedDown(deviceId, OuyaController.BUTTON_MENU);

                buttonUpState[OuyaController.BUTTON_O] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_O);
                buttonUpState[OuyaController.BUTTON_U] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_U);
                buttonUpState[OuyaController.BUTTON_Y] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_Y);
                buttonUpState[OuyaController.BUTTON_A] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_A);
                buttonUpState[OuyaController.BUTTON_L1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L1);
                buttonUpState[OuyaController.BUTTON_R1] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R1);
                buttonUpState[OuyaController.BUTTON_L3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_L3);
                buttonUpState[OuyaController.BUTTON_R3] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_R3);
                buttonUpState[OuyaController.BUTTON_DPAD_UP] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_UP);
                buttonUpState[OuyaController.BUTTON_DPAD_DOWN] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_DOWN);
                buttonUpState[OuyaController.BUTTON_DPAD_RIGHT] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_RIGHT);
                buttonUpState[OuyaController.BUTTON_DPAD_LEFT] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_DPAD_LEFT);
                buttonUpState[OuyaController.BUTTON_MENU] = NdkWrapper.isPressedUp(deviceId, OuyaController.BUTTON_MENU);

                #endregion

                //debugOuyaController(deviceId);

            }

            NdkWrapper.clearButtonStates();
        }

        private static void debugOuyaController(int deviceId, int button)
        {
            if (GetButtonDown(deviceId, button))
            {
                Debug.Log("Device=" + deviceId + " GetButtonDown: " + button);
            }

            if (GetButtonUp(deviceId, button))
            {
                Debug.Log("Device=" + deviceId + " GetButtonUp: " + button);
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

        public static bool IsControllerConnected(int playerNum)
        {
            if (playerNum >= 0 &&
                null != OuyaSDK.Joysticks &&
                playerNum < OuyaSDK.Joysticks.Length)
            {
                return (null != OuyaSDK.Joysticks[playerNum]);
            }
            else
            {
                return false;
            }
        }

        public static float GetAxis(int playerNum, int axis)
        {
            return GetState(axis, m_axisStates[playerNum]);
        }

        public static bool GetButton(int playerNum, int button)
        {
            return GetState(button, m_buttonStates[playerNum]);
        }

        public static bool GetButtonDown(int playerNum, int button)
        {
            return GetState(button, m_buttonDownStates[playerNum]);
        }

        public static bool GetButtonUp(int playerNum, int button)
        {
            return GetState(button, m_buttonUpStates[playerNum]);
        }

        #endregion
    }

#endif

    /// <summary>
    /// Cache joysticks
    /// </summary>
    public static string[] Joysticks = null;

    /// <summary>
    /// Query joysticks every N seconds
    /// </summary>
    private static DateTime m_timerJoysticks = DateTime.MinValue;

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string getDeviceName(int deviceId)
    {
        OuyaController ouyaController = OuyaController.getControllerByPlayer(deviceId);
        if (null != ouyaController)
        {
            return ouyaController.getDeviceName();
        }
        return null;
    }
#endif

    /// <summary>
    /// Update joysticks with a timer
    /// </summary>
    public static void UpdateJoysticks()
    {
#if !UNITY_WP8
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

#if UNITY_ANDROID && !UNITY_EDITOR
            string[] joysticks = null;
            List<string> devices = new List<string>();
            for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
            {
                string deviceName = getDeviceName(deviceId);
                //Debug.Log(string.Format("Device={0} name={1}", deviceId, deviceName));
                devices.Add(deviceName);
            }
            joysticks = devices.ToArray();
#else
            string[] joysticks = Input.GetJoystickNames();
#endif

            // look for changes
            bool detectedChange = false;

            if (null == Joysticks)
            {
                detectedChange = true;
            }
            else if (joysticks.Length != Joysticks.Length)
            {
                detectedChange = true;
            }
            else
            {
                for (int index = 0; index < joysticks.Length; ++index)
                {
                    if (joysticks[index] != Joysticks[index])
                    {
                        detectedChange = true;
                        break;
                    }
                }
            }

            Joysticks = joysticks;

            if (detectedChange)
            {
                foreach (OuyaSDK.IJoystickCalibrationListener listener in OuyaSDK.getJoystickCalibrationListeners())
                {
                    //Debug.Log("OuyaGameObject: Invoke OuyaOnJoystickCalibration");
                    listener.OuyaOnJoystickCalibration();
                }
            }
        }
#endif
    }

    /// <summary>
    /// The developer ID assigned by OuyaGameObject
    /// </summary>
    static private string m_developerId = string.Empty;

    /// <summary>
    /// Inidicates IAP has been setup and is ready for processing
    /// </summary>
    private static bool m_iapInitComplete = false;

    /// <summary>
    /// Check if the device is an OUYA
    /// </summary>
    /// <returns></returns>
    public static bool IsOUYA()
    {
        if (SystemInfo.deviceModel.ToUpper().Contains("OUYA"))
        {
            return true;
        }

        return false;
    }

    #region Controllers

    #region Key Codes

    public const int KEYCODE_BUTTON_A = 96;
    public const int KEYCODE_BUTTON_B = 97;
    public const int KEYCODE_BUTTON_X = 99;
    public const int KEYCODE_BUTTON_Y = 100;
    public const int KEYCODE_BUTTON_L1 = 102;
    public const int KEYCODE_BUTTON_L2 = 104;
    public const int KEYCODE_BUTTON_R1 = 103;
    public const int KEYCODE_BUTTON_R2 = 105;
    public const int KEYCODE_BUTTON_L3 = 106;
    public const int KEYCODE_BUTTON_R3 = 107;
    public const int KEYCODE_BUTTON_SYSTEM = 108;
    public const int AXIS_X = 0;
    public const int AXIS_Y = 1;
    public const int AXIS_Z = 11;
    public const int AXIS_RZ = 14;
    public const int KEYCODE_DPAD_UP = 19;
    public const int KEYCODE_DPAD_DOWN = 20;
    public const int KEYCODE_DPAD_LEFT = 21;
    public const int KEYCODE_DPAD_RIGHT = 22;
    public const int KEYCODE_DPAD_CENTER = 23;

    //For EDITOR / PC / MAC
    public const int KEYCODE_BUTTON_SELECT = -1;
    public const int KEYCODE_BUTTON_START = -2;
    public const int KEYCODE_BUTTON_ESCAPE = 41;
    #endregion

    #region OUYA controller

    public const int BUTTON_O = KEYCODE_BUTTON_A;
    public const int BUTTON_U = KEYCODE_BUTTON_X;
    public const int BUTTON_Y = KEYCODE_BUTTON_Y;
    public const int BUTTON_A = KEYCODE_BUTTON_B;

    public const int BUTTON_LB = KEYCODE_BUTTON_L1;
    public const int BUTTON_LT = KEYCODE_BUTTON_L2;
    public const int BUTTON_RB = KEYCODE_BUTTON_R1;
    public const int BUTTON_RT = KEYCODE_BUTTON_R2;
    public const int BUTTON_L3 = KEYCODE_BUTTON_L3;
    public const int BUTTON_R3 = KEYCODE_BUTTON_R3;

    public const int BUTTON_SYSTEM = KEYCODE_BUTTON_SYSTEM;
    // for EDITOR / PC / MAC
    public const int BUTTON_START = KEYCODE_BUTTON_START;
    public const int BUTTON_SELECT = KEYCODE_BUTTON_SELECT;
    public const int BUTTON_ESCAPE = KEYCODE_BUTTON_ESCAPE;
    public const int AXIS_LSTICK_X = AXIS_X;
    public const int AXIS_LSTICK_Y = AXIS_Y;
    public const int AXIS_RSTICK_X = AXIS_Z;
    public const int AXIS_RSTICK_Y = AXIS_RZ;

    public const int BUTTON_DPAD_UP = KEYCODE_DPAD_UP;
    public const int BUTTON_DPAD_RIGHT = KEYCODE_DPAD_RIGHT;
    public const int BUTTON_DPAD_DOWN = KEYCODE_DPAD_DOWN;
    public const int BUTTON_DPAD_LEFT = KEYCODE_DPAD_LEFT;
    public const int BUTTON_DPAD_CENTER = KEYCODE_DPAD_CENTER;

    public enum KeyEnum
    {
        NONE = -1,
        BUTTON_O = OuyaSDK.BUTTON_O,
        BUTTON_U = OuyaSDK.BUTTON_U,
        BUTTON_Y = OuyaSDK.KEYCODE_BUTTON_Y,
        BUTTON_A = OuyaSDK.BUTTON_A,
        BUTTON_LB = OuyaSDK.BUTTON_LB,
        AXIS_LT = OuyaSDK.KEYCODE_BUTTON_L2,
        BUTTON_LT = OuyaSDK.KEYCODE_BUTTON_L2,
        BUTTON_RB = OuyaSDK.BUTTON_RB,
        AXIS_RT = OuyaSDK.BUTTON_RT,
        BUTTON_RT = OuyaSDK.BUTTON_RT,
        BUTTON_L3 = OuyaSDK.BUTTON_L3,
        BUTTON_R3 = OuyaSDK.BUTTON_R3,
        BUTTON_SYSTEM = OuyaSDK.BUTTON_SYSTEM,
        BUTTON_START = OuyaSDK.BUTTON_START,
        BUTTON_SELECT = OuyaSDK.BUTTON_SELECT,
        BUTTON_ESCAPE = OuyaSDK.BUTTON_ESCAPE,
        AXIS_LSTICK_X = OuyaSDK.AXIS_LSTICK_X,
        AXIS_LSTICK_Y = OuyaSDK.AXIS_LSTICK_Y,
        AXIS_RSTICK_X = OuyaSDK.AXIS_RSTICK_X,
        AXIS_RSTICK_Y = OuyaSDK.AXIS_RSTICK_Y,
        BUTTON_DPAD_UP = OuyaSDK.BUTTON_DPAD_UP,
        BUTTON_DPAD_RIGHT = OuyaSDK.BUTTON_DPAD_RIGHT,
        BUTTON_DPAD_DOWN = OuyaSDK.BUTTON_DPAD_DOWN,
        BUTTON_DPAD_LEFT = OuyaSDK.BUTTON_DPAD_LEFT,
        BUTTON_DPAD_CENTER = OuyaSDK.BUTTON_DPAD_CENTER,

        BUTTON_BACK,

        HARMONIX_ROCK_BAND_GUITAR_GREEN,
        HARMONIX_ROCK_BAND_GUITAR_RED,
        HARMONIX_ROCK_BAND_GUITAR_YELLOW,
        HARMONIX_ROCK_BAND_GUITAR_BLUE,
        HARMONIX_ROCK_BAND_GUITAR_ORANGE,
        HARMONIX_ROCK_BAND_GUITAR_LOWER,
        HARMONIX_ROCK_BAND_GUITAR_WHAMMI,
        HARMONIX_ROCK_BAND_GUITAR_PICKUP,
        HARMONIX_ROCK_BAND_GUITAR_STRUM,

        HARMONIX_ROCK_BAND_DRUMKIT_GREEN,
        HARMONIX_ROCK_BAND_DRUMKIT_RED,
        HARMONIX_ROCK_BAND_DRUMKIT_YELLOW,
        HARMONIX_ROCK_BAND_DRUMKIT_BLUE,
        HARMONIX_ROCK_BAND_DRUMKIT_ORANGE,
        HARMONIX_ROCK_BAND_DRUMKIT_A,
        HARMONIX_ROCK_BAND_DRUMKIT_B,
        HARMONIX_ROCK_BAND_DRUMKIT_X,
        HARMONIX_ROCK_BAND_DRUMKIT_Y,
    }

    public enum AxisEnum
    {
        NONE = -1,
        AXIS_LSTICK_X,
        AXIS_LSTICK_Y,
        AXIS_RSTICK_X,
        AXIS_RSTICK_Y,
        AXIS_LTRIGGER,
        AXIS_RTRIGGER,
    }

    public enum OuyaPlayer
    {
        player1=1,
        player2=2,
        player3=3,
        player4=4,
        player5=5,
        player6=6,
        player7=7,
        player8=8,
        player9=9,
        player10=10,
        player11=11,
        none=0,
    }

    #endregion

    #endregion

    /// <summary>
    /// Initialized by OuyaGameObject
    /// </summary>
    /// <param name="developerId"></param>
    public static void initialize(string developerId)
    {
        m_developerId = developerId;
        OuyaSDK.OuyaJava.JavaSetDeveloperId();

        OuyaSDK.OuyaJava.JavaUnityInitialized();
    }

    /// <summary>
    /// Access the developer id
    /// </summary>
    /// <returns></returns>
    public static string getDeveloperId()
    {
        return m_developerId;
    }

    public static bool isIAPInitComplete()
    {
        return m_iapInitComplete;
    }

    public static void setIAPInitComplete()
    {
        m_iapInitComplete = true;
    }

    #region Mirror Java API

    public class GenericListener<T>
    {
        public delegate void SuccessDelegate(T data);

        public SuccessDelegate onSuccess = null;

        public delegate void FailureDelegate(int errorCode, String errorMessage);

        public FailureDelegate onFailure = null;
    }

    public class CancelIgnoringIapResponseListener<T> : GenericListener<T>
    {
    }

    public static void fetchGamerInfo()
    {
        OuyaSDK.OuyaJava.JavaFetchGamerInfo();
    }

    public static void showCursor(bool flag)
    {
        OuyaSDK.OuyaJava.JavaShowCursor(flag);
    }

    public static void putGameData(string key, string val)
    {
        OuyaSDK.OuyaJava.JavaPutGameData(key, val);
    }

    public static string getGameData(string key)
    {
        return OuyaSDK.OuyaJava.JavaGetGameData(key);
    }

    public static void requestProductList(List<Purchasable> purchasables)
    {
        foreach (Purchasable purchasable in purchasables)
        {
            //Debug.Log(string.Format("Unity Adding: {0}", purchasable.getProductId()));
            OuyaSDK.OuyaJava.JavaAddGetProduct(purchasable);
        }

        OuyaSDK.OuyaJava.JavaGetProductsAsync();
    }

    public static void requestPurchase(Purchasable purchasable)
	{
        OuyaSDK.OuyaJava.JavaRequestPurchaseAsync(purchasable);
    }

    public static void requestReceiptList()
    {
        OuyaSDK.OuyaJava.JavaGetReceiptsAsync();
    }

    public class InputButtonListener<T> : GenericListener<T>
    {
    }

    public class InputAxisListener<T> : GenericListener<T>
    {
    }

    public class DeviceListener<T> : GenericListener<T>
    {
    }


    #endregion

    #region Data containers

    [Serializable]
    public class GamerInfo
    {
        public string uuid = string.Empty;
        public string username = string.Empty;
    }

    [Serializable]
    public class Purchasable
    {
        public string productId = string.Empty;
    }

    [Serializable]
    public class Product
    {
        public string currencyCode = string.Empty;
        public string description = string.Empty;
        public string identifier = string.Empty;
        public float localPrice = 0f;
        public string name = string.Empty;
        public float originalPrice = 0f;
        public float percentOff = 0f;
        public int priceInCents = 0;
        public int productVersionToBundle = 0;
        public string developerName = string.Empty;
    }

    [Serializable]
    public class Receipt
    {
        public string currency = string.Empty;
        public string gamer = string.Empty;
        public DateTime generatedDate = DateTime.MinValue;
        public string identifier = string.Empty;
        public float localPrice = 0f;
        public int priceInCents = 0;
        public DateTime purchaseDate = DateTime.MinValue;
        public string uuid = string.Empty;
    }
    
    #endregion

    #region Joystick Callibration Listeners

    public interface IJoystickCalibrationListener
    {
        void OuyaOnJoystickCalibration();
    }
    private static List<IJoystickCalibrationListener> m_joystickCalibrationListeners = new List<IJoystickCalibrationListener>();
    public static List<IJoystickCalibrationListener> getJoystickCalibrationListeners()
    {
        return m_joystickCalibrationListeners;
    }
    public static void registerJoystickCalibrationListener(IJoystickCalibrationListener listener)
    {
        if (!m_joystickCalibrationListeners.Contains(listener))
        {
            m_joystickCalibrationListeners.Add(listener);
        }
    }
    public static void unregisterJoystickCalibrationListener(IJoystickCalibrationListener listener)
    {
        if (m_joystickCalibrationListeners.Contains(listener))
        {
            m_joystickCalibrationListeners.Remove(listener);
        }
    }

    #endregion

    #region Menu Button Up Listeners

    public interface IMenuButtonUpListener
    {
        void OuyaMenuButtonUp();
    }
    private static List<IMenuButtonUpListener> m_menuButtonUpListeners = new List<IMenuButtonUpListener>();
    public static List<IMenuButtonUpListener> getMenuButtonUpListeners()
    {
        return m_menuButtonUpListeners;
    }
    public static void registerMenuButtonUpListener(IMenuButtonUpListener listener)
    {
        if (!m_menuButtonUpListeners.Contains(listener))
        {
            m_menuButtonUpListeners.Add(listener);
        }
    }
    public static void unregisterMenuButtonUpListener(IMenuButtonUpListener listener)
    {
        if (m_menuButtonUpListeners.Contains(listener))
        {
            m_menuButtonUpListeners.Remove(listener);
        }
    }

    #endregion

    #region Menu Appearing Listeners

    public interface IMenuAppearingListener
    {
        void OuyaMenuAppearing();
    }
    private static List<IMenuAppearingListener> m_menuAppearingListeners = new List<IMenuAppearingListener>();
    public static List<IMenuAppearingListener> getMenuAppearingListeners()
    {
        return m_menuAppearingListeners;
    }
    public static void registerMenuAppearingListener(IMenuAppearingListener listener)
    {
        if (!m_menuAppearingListeners.Contains(listener))
        {
            m_menuAppearingListeners.Add(listener);
        }
    }
    public static void unregisterMenuAppearingListener(IMenuAppearingListener listener)
    {
        if (m_menuAppearingListeners.Contains(listener))
        {
            m_menuAppearingListeners.Remove(listener);
        }
    }

    #endregion

    #region Pause Listeners

    public interface IPauseListener
    {
        void OuyaOnPause();
    }
    private static List<IPauseListener> m_pauseListeners = new List<IPauseListener>();
    public static List<IPauseListener> getPauseListeners()
    {
        return m_pauseListeners;
    }
    public static void registerPauseListener(IPauseListener listener)
    {
        if (!m_pauseListeners.Contains(listener))
        {
            m_pauseListeners.Add(listener);
        }
    }
    public static void unregisterPauseListener(IPauseListener listener)
    {
        if (m_pauseListeners.Contains(listener))
        {
            m_pauseListeners.Remove(listener);
        }
    }

    #endregion

    #region Resume Listeners

    public interface IResumeListener
    {
        void OuyaOnResume();
    }
    private static List<IResumeListener> m_resumeListeners = new List<IResumeListener>();
    public static List<IResumeListener> getResumeListeners()
    {
        return m_resumeListeners;
    }
    public static void registerResumeListener(IResumeListener listener)
    {
        if (!m_resumeListeners.Contains(listener))
        {
            m_resumeListeners.Add(listener);
        }
    }
    public static void unregisterResumeListener(IResumeListener listener)
    {
        if (m_resumeListeners.Contains(listener))
        {
            m_resumeListeners.Remove(listener);
        }
    }

    #endregion

    #region Fetch Gamer UUID Listener

    public interface IFetchGamerInfoListener
    {
        void OuyaFetchGamerInfoOnSuccess(string uuid, string username);
        void OuyaFetchGamerInfoOnFailure(int errorCode, string errorMessage);
        void OuyaFetchGamerInfoOnCancel();
    }
    private static List<IFetchGamerInfoListener> m_FetchGamerInfoListeners = new List<IFetchGamerInfoListener>();
    public static List<IFetchGamerInfoListener> getFetchGamerInfoListeners()
    {
        return m_FetchGamerInfoListeners;
    }
    public static void registerFetchGamerInfoListener(IFetchGamerInfoListener listener)
    {
        if (!m_FetchGamerInfoListeners.Contains(listener))
        {
            m_FetchGamerInfoListeners.Add(listener);
        }
    }
    public static void unregisterFetchGamerInfoListener(IFetchGamerInfoListener listener)
    {
        if (m_FetchGamerInfoListeners.Contains(listener))
        {
            m_FetchGamerInfoListeners.Remove(listener);
        }
    }

    #endregion

    #region Get GetProducts Listeners

    public interface IGetProductsListener
    {
        void OuyaGetProductsOnSuccess(List<OuyaSDK.Product> products);
        void OuyaGetProductsOnFailure(int errorCode, string errorMessage);
        void OuyaGetProductsOnCancel();
    }
    private static List<IGetProductsListener> m_getProductsListeners = new List<IGetProductsListener>();
    public static List<IGetProductsListener> getGetProductsListeners()
    {
        return m_getProductsListeners;
    }
    public static void registerGetProductsListener(IGetProductsListener listener)
    {
        if (!m_getProductsListeners.Contains(listener))
        {
            m_getProductsListeners.Add(listener);
        }
    }
    public static void unregisterGetProductsListener(IGetProductsListener listener)
    {
        if (m_getProductsListeners.Contains(listener))
        {
            m_getProductsListeners.Remove(listener);
        }
    }

    #endregion

    #region Purchase Listener

    public interface IPurchaseListener
    {
        void OuyaPurchaseOnSuccess(OuyaSDK.Product product);
        void OuyaPurchaseOnFailure(int errorCode, string errorMessage);
        void OuyaPurchaseOnCancel();
    }
    private static List<IPurchaseListener> m_purchaseListeners = new List<IPurchaseListener>();
    public static List<IPurchaseListener> getPurchaseListeners()
    {
        return m_purchaseListeners;
    }
    public static void registerPurchaseListener(IPurchaseListener listener)
    {
        if (!m_purchaseListeners.Contains(listener))
        {
            m_purchaseListeners.Add(listener);
        }
    }
    public static void unregisterPurchaseListener(IPurchaseListener listener)
    {
        if (m_purchaseListeners.Contains(listener))
        {
            m_purchaseListeners.Remove(listener);
        }
    }

    #endregion

    #region Get GetReceipts Listeners

    public interface IGetReceiptsListener
    {
        void OuyaGetReceiptsOnSuccess(List<Receipt> receipts);
        void OuyaGetReceiptsOnFailure(int errorCode, string errorMessage);
        void OuyaGetReceiptsOnCancel();
    }
    private static List<IGetReceiptsListener> m_getReceiptsListeners = new List<IGetReceiptsListener>();
    public static List<IGetReceiptsListener> getGetReceiptsListeners()
    {
        return m_getReceiptsListeners;
    }
    public static void registerGetReceiptsListener(IGetReceiptsListener listener)
    {
        if (!m_getReceiptsListeners.Contains(listener))
        {
            m_getReceiptsListeners.Add(listener);
        }
    }
    public static void unregisterGetReceiptsListener(IGetReceiptsListener listener)
    {
        if (m_getReceiptsListeners.Contains(listener))
        {
            m_getReceiptsListeners.Remove(listener);
        }
    }

    #endregion

    #region Java Interface

    public class OuyaJava
    {
        private const string JAVA_CLASS = "tv.ouya.sdk.OuyaUnityPlugin";

        public static void JavaInit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
            AndroidJNI.AttachCurrentThread();

            // first we try to find our main activity..
            IntPtr cls_Activity = AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
            IntPtr fid_Activity = AndroidJNI.GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
            IntPtr obj_Activity = AndroidJNI.GetStaticObjectField(cls_Activity, fid_Activity);
            Debug.Log("obj_Activity = " + obj_Activity);

            // create a JavaClass object...
            IntPtr cls_JavaClass = AndroidJNI.FindClass("tv/ouya/sdk/OuyaUnityPlugin");
            IntPtr mid_JavaClass = AndroidJNI.GetMethodID(cls_JavaClass, "<init>", "(Landroid/app/Activity;)V");
            IntPtr obj_JavaClass = AndroidJNI.NewObject(cls_JavaClass, mid_JavaClass, new jvalue[] { new jvalue() { l = obj_Activity } });
            Debug.Log("JavaClass object = " + obj_JavaClass);
#endif
        }

        public static void JavaSetDeveloperId()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaSetDeveloperId");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic<String>("setDeveloperId", new object[] { m_developerId + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaSetDeveloperId exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaUnityInitialized()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaUnityInitialized");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("unityInitialized");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaUnityInitialized exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaSetResolution(string resolutionId)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaSetResolution");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("setResolution", resolutionId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaSetResolution exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaShowCursor(bool flag)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                //Debug.Log("JavaShowCursor");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("showCursor", new object[] { flag.ToString() });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaShowCursor exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaPutGameData(string key, string val)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaPutGameData");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("putGameData", new object[] { key + "\0", val + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaPutGameData exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static string JavaGetGameData(string key)
        {
            string result = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX

            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("JavaGetGameData");
                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    result = ajc.CallStatic<String>("getGameData", new object[] { key + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaGetGameData exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif

            return result;
        }

        public static void JavaFetchGamerInfo()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaFetchGamerInfo", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("fetchGamerInfo");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaFetchGamerInfo exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaAddGetProduct(Purchasable purchasable)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaAddGetProduct", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("addGetProduct", purchasable.productId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaAddGetProduct exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaDebugGetProductList()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaDebugGetProductList", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("debugGetProductList");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaDebugGetProductList exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaClearGetProductList()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("{0} OuyaSDK.JavaClearGetProductList", DateTime.Now));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("clearGetProductList");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaClearGetProductList exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaGetProductsAsync()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("OuyaSDK.JavaGetProductsAsync");

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("getProductsAsync");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaGetProductsAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaRequestPurchaseAsync(Purchasable purchasable)
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log(string.Format("JavaRequestPurchaseAsync purchasable: {0}", purchasable.productId));

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic<String>("requestPurchaseAsync", new object[] { purchasable.productId + "\0" });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaRequestPurchaseAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }

        public static void JavaGetReceiptsAsync()
        {
#if UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
            // again, make sure the thread is attached..
            AndroidJNI.AttachCurrentThread();

            AndroidJNI.PushLocalFrame(0);

            try
            {
                Debug.Log("OuyaSDK.JavaGetReceiptsAsync");

                using (AndroidJavaClass ajc = new AndroidJavaClass(JAVA_CLASS))
                {
                    ajc.CallStatic("getReceiptsAsync");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("OuyaSDK.JavaGetReceiptsAsync exception={0}", ex));
            }
            finally
            {
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }
#endif
        }
    }

    #endregion
}