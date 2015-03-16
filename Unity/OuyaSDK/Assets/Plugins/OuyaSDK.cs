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
using com.unity3d.player;
using org.json;
using tv.ouya.console.api;
using tv.ouya.console.api.content;
using tv.ouya.sdk;
#endif
using UnityEngine;

public static class OuyaSDK
{
    public const string VERSION = "1.2.1494.1";

#if UNITY_ANDROID && !UNITY_EDITOR
    
    private static OuyaUnityPlugin m_ouyaUnityPlugin = null;

    static OuyaSDK()
    {
        // attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
        AndroidJNI.AttachCurrentThread();

        m_ouyaUnityPlugin = new OuyaUnityPlugin(UnityPlayer.currentActivity);
    }

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

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API void clearAxes()
        public static extern void clearAxes();

        [DllImport("lib-ouya-ndk")]
        // EXPORT_API void clearButtons()
        public static extern void clearButtons();
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR

    public class OuyaInput
    {
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
            }
        }

        public static void ClearButtonStates()
        {
            NdkWrapper.clearButtonStates();
        }

        public static void ClearAxes()
        {
            NdkWrapper.clearAxes();
        }

        public static void ClearButtons()
        {
            NdkWrapper.clearButtons();
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

        public static float GetAxisRaw(int playerNum, int axis)
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

#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    /// <summary>
    /// Cache joysticks
    /// </summary>
    public static string[] Joysticks = null;

    /// <summary>
    /// Query joysticks every N seconds
    /// </summary>
    private static DateTime m_timerJoysticks = DateTime.MinValue;

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
#if !UNITY_ANDROID || UNITY_EDITOR
        return;
#else
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

            string[] joysticks = null;
            List<string> devices = new List<string>();
            for (int deviceId = 0; deviceId < OuyaController.MAX_CONTROLLERS; ++deviceId)
            {
                string deviceName = getDeviceName(deviceId);
                //Debug.Log(string.Format("Device={0} name={1}", deviceId, deviceName));
                devices.Add(deviceName);
            }
            joysticks = devices.ToArray();

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
    /// Initialized by OuyaGameObject
    /// </summary>
    public static void initOuyaPlugin(string jsonData)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.initOuyaPlugin(jsonData);
#endif
    }

    public static bool isIAPInitComplete()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.isInitialized();
#else
        return false;
#endif
    }

    #region Mirror Java API

    public static void requestGamerInfo()
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.requestGamerInfo();
#endif
    }

    public static void putGameData(string key, string val)
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.putGameData(key, val);
#endif
    }

    public static string getGameData(string key)
    {
        if (!isIAPInitComplete())
        {
            return string.Empty;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.getGameData(key);
#else
        return String.Empty;
#endif
    }

    public static void requestProducts(List<Purchasable> purchasables)
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        JSONArray jsonArray = new JSONArray();
        int index = 0;
        foreach (Purchasable purchasable in purchasables)
        {
            jsonArray.put(index, purchasable.productId);
            ++index;
        }
        OuyaUnityPlugin.requestProducts(jsonArray.toString());
        jsonArray.Dispose();
#endif
    }

    public static void requestPurchase(Purchasable purchasable)
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.requestPurchase(purchasable.productId);
#endif
    }

    public static void requestReceipts()
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.requestReceipts();
#endif
    }

    public static bool isRunningOnOUYASupportedHardware()
    {
        if (!isIAPInitComplete())
        {
            return false;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.isRunningOnOUYASupportedHardware();
#else
        return false;
#endif
    }

    /// <summary>
    /// 1f - Safe Area all the way to the edge of the screen
    /// 0f - Safe Area will use the maximum overscan border
    /// </summary>
    /// <param name="percentage"></param>
    public static void setSafeArea(float percentage)
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.setSafeArea(percentage);
#endif        
    }

    /// <summary>
    /// Clear input focus
    /// </summary>
    public static void clearFocus()
    {
        if (!isIAPInitComplete())
        {
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        OuyaUnityPlugin.clearFocus();
#endif
    }

    /// <summary>
    /// Get localized string resource
    /// </summary>
    public static string getStringResource(string key)
    {
        if (!isIAPInitComplete())
        {
            return string.Empty;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        return OuyaUnityPlugin.getStringResource(key);
#else
        return string.Empty;
#endif
    }

    #endregion

    #region Data containers

    [Serializable]
    public class GamerInfo
    {
        public string uuid = string.Empty;
        public string username = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
        public static GamerInfo Parse(JSONObject jsonObject)
        {
            GamerInfo result = new GamerInfo();

            //Debug.Log(jsonData);
            if (jsonObject.has("uuid"))
            {
                result.uuid = jsonObject.getString("uuid");
            }
            if (jsonObject.has("username"))
            {
                result.username = jsonObject.getString("username");
            }
            return result;
        }
#endif
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
        public string developerName = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
        public static Product Parse(JSONObject jsonData)
        {
            Product result = new Product();
            if (jsonData.has("currencyCode"))
            {
                result.currencyCode = jsonData.getString("currencyCode");
            }
            if (jsonData.has("description"))
            {
                result.description = jsonData.getString("description");
            }
            if (jsonData.has("identifier"))
            {
                result.identifier = jsonData.getString("identifier");
            }
            if (jsonData.has("localPrice"))
            {
                result.localPrice = (float)jsonData.getDouble("localPrice");
            }
            if (jsonData.has("name"))
            {
                result.name = jsonData.getString("name");
            }
            if (jsonData.has("originalPrice"))
            {
                result.originalPrice = (float)jsonData.getDouble("originalPrice");
            }
            if (jsonData.has("percentOff"))
            {
                result.percentOff = (float)jsonData.getDouble("percentOff");
            }
            if (jsonData.has("developerName"))
            {
                result.developerName = jsonData.getString("developerName");
            }
            return result;
        }
#endif
    }

    [Serializable]
    public class Receipt
    {
        public string currency = string.Empty;
        public string gamer = string.Empty;
        public DateTime generatedDate = DateTime.MinValue;
        public string identifier = string.Empty;
        public float localPrice = 0f;
        public DateTime purchaseDate = DateTime.MinValue;
        public string uuid = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
        public static Receipt Parse(JSONObject jsonObject)
        {
            Receipt result = new Receipt();

            if (jsonObject.has("identifier"))
            {
                result.identifier = jsonObject.getString("identifier");
            }
            if (jsonObject.has("purchaseDate"))
            {
                DateTime date;
                DateTime.TryParse(jsonObject.getString("purchaseDate"), out date);
                result.purchaseDate = date;
            }
            if (jsonObject.has("gamer"))
            {
                result.gamer = jsonObject.getString("gamer");
            }
            if (jsonObject.has("localPrice"))
            {
                result.localPrice = (float)jsonObject.getDouble("localPrice");
            }
            if (jsonObject.has("uuid"))
            {
                result.uuid = jsonObject.getString("uuid");
            }
            if (jsonObject.has("currency"))
            {
                result.currency = jsonObject.getString("currency");
            }
            if (jsonObject.has("generatedDate"))
            {
                DateTime date;
                DateTime.TryParse(jsonObject.getString("generatedDate"), out date);
                result.generatedDate = date;
            }

            return result;
        }
#endif
    }

    #endregion

#if UNITY_ANDROID && !UNITY_EDITOR

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

    #region Content Initialized Listener

    public interface IContentInitializedListener
    {
        void ContentInitializedOnInitialized();
        void ContentInitializedOnDestroyed();
    }
    private static List<IContentInitializedListener> m_contentInitializedListeners = new List<IContentInitializedListener>();
    public static List<IContentInitializedListener> getContentInitializedListeners()
    {
        return m_contentInitializedListeners;
    }
    public static void registerContentInitializedListener(IContentInitializedListener listener)
    {
        if (!m_contentInitializedListeners.Contains(listener))
        {
            m_contentInitializedListeners.Add(listener);
        }
    }
    public static void unregisterContentInitializedListener(IContentInitializedListener listener)
    {
        if (m_contentInitializedListeners.Contains(listener))
        {
            m_contentInitializedListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Delete Listener

    public interface IContentDeleteListener
    {
        void ContentDeleteOnDeleted(OuyaMod ouyaMod);
        void ContentDeleteOnDeleteFailed(OuyaMod ouyaMod, int code, string reason);
    }
    private static List<IContentDeleteListener> m_contentDeleteListeners = new List<IContentDeleteListener>();
    public static List<IContentDeleteListener> getContentDeleteListeners()
    {
        return m_contentDeleteListeners;
    }
    public static void registerContentDeleteListener(IContentDeleteListener listener)
    {
        if (!m_contentDeleteListeners.Contains(listener))
        {
            m_contentDeleteListeners.Add(listener);
        }
    }
    public static void unregisterContentDeleteListener(IContentDeleteListener listener)
    {
        if (m_contentDeleteListeners.Contains(listener))
        {
            m_contentDeleteListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Download Listener

    public interface IContentDownloadListener
    {
        void ContentDownloadOnComplete(OuyaMod ouyaMod);
        void ContentDownloadOnFailed(OuyaMod ouyaMod);
        void ContentDownloadOnProgress(OuyaMod ouyaMod, int progress);
    }
    private static List<IContentDownloadListener> m_contentDownloadListeners = new List<IContentDownloadListener>();
    public static List<IContentDownloadListener> getContentDownloadListeners()
    {
        return m_contentDownloadListeners;
    }
    public static void registerContentDownloadListener(IContentDownloadListener listener)
    {
        if (!m_contentDownloadListeners.Contains(listener))
        {
            m_contentDownloadListeners.Add(listener);
        }
    }
    public static void unregisterContentDownloadListener(IContentDownloadListener listener)
    {
        if (m_contentDownloadListeners.Contains(listener))
        {
            m_contentDownloadListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Installed Search Listener

    public interface IContentInstalledSearchListener
    {
        void ContentInstalledSearchOnResults(List<OuyaMod> ouyaMods, int count);
        void ContentInstalledSearchOnError(int code, string reason);
    }
    private static List<IContentInstalledSearchListener> m_contentInstalledSearchListeners = new List<IContentInstalledSearchListener>();
    public static List<IContentInstalledSearchListener> getContentInstalledSearchListeners()
    {
        return m_contentInstalledSearchListeners;
    }
    public static void registerContentInstalledSearchListener(IContentInstalledSearchListener listener)
    {
        if (!m_contentInstalledSearchListeners.Contains(listener))
        {
            m_contentInstalledSearchListeners.Add(listener);
        }
    }
    public static void unregisterContentInstalledSearchListener(IContentInstalledSearchListener listener)
    {
        if (m_contentInstalledSearchListeners.Contains(listener))
        {
            m_contentInstalledSearchListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Published Search Listener

    public interface IContentPublishedSearchListener
    {
        void ContentPublishedSearchOnResults(List<OuyaMod> ouyaMods, int count);
        void ContentPublishedSearchOnError(int code, string reason);
    }
    private static List<IContentPublishedSearchListener> m_contentPublishedSearchListeners = new List<IContentPublishedSearchListener>();
    public static List<IContentPublishedSearchListener> getContentPublishedSearchListeners()
    {
        return m_contentPublishedSearchListeners;
    }
    public static void registerContentPublishedSearchListener(IContentPublishedSearchListener listener)
    {
        if (!m_contentPublishedSearchListeners.Contains(listener))
        {
            m_contentPublishedSearchListeners.Add(listener);
        }
    }
    public static void unregisterContentPublishedSearchListener(IContentPublishedSearchListener listener)
    {
        if (m_contentPublishedSearchListeners.Contains(listener))
        {
            m_contentPublishedSearchListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Publish Listener

    public interface IContentPublishListener
    {
        void ContentPublishOnSuccess(OuyaMod ouyaMod);
        void ContentPublishOnError(OuyaMod ouyaMod, int code, string reason);
    }
    private static List<IContentPublishListener> m_contentPublishListeners = new List<IContentPublishListener>();
    public static List<IContentPublishListener> getContentPublishListeners()
    {
        return m_contentPublishListeners;
    }
    public static void registerContentPublishListener(IContentPublishListener listener)
    {
        if (!m_contentPublishListeners.Contains(listener))
        {
            m_contentPublishListeners.Add(listener);
        }
    }
    public static void unregisterContentPublishListener(IContentPublishListener listener)
    {
        if (m_contentPublishListeners.Contains(listener))
        {
            m_contentPublishListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Save Listener

    public interface IContentSaveListener
    {
        void ContentSaveOnSuccess(OuyaMod ouyaMod);
        void ContentSaveOnError(OuyaMod ouyaMod, int code, string reason);
    }
    private static List<IContentSaveListener> m_contentSaveListeners = new List<IContentSaveListener>();
    public static List<IContentSaveListener> getContentSaveListeners()
    {
        return m_contentSaveListeners;
    }
    public static void registerContentSaveListener(IContentSaveListener listener)
    {
        if (!m_contentSaveListeners.Contains(listener))
        {
            m_contentSaveListeners.Add(listener);
        }
    }
    public static void unregisterContentSaveListener(IContentSaveListener listener)
    {
        if (m_contentSaveListeners.Contains(listener))
        {
            m_contentSaveListeners.Remove(listener);
        }
    }

    #endregion

    #region Content Unpublish Listener

    public interface IContentUnpublishListener
    {
        void ContentUnpublishOnSuccess(OuyaMod ouyaMod);
        void ContentUnpublishOnError(OuyaMod ouyaMod, int code, string reason);
    }
    private static List<IContentUnpublishListener> m_contentUnpublishListeners = new List<IContentUnpublishListener>();
    public static List<IContentUnpublishListener> getContentUnpublishListeners()
    {
        return m_contentUnpublishListeners;
    }
    public static void registerContentUnpublishListener(IContentUnpublishListener listener)
    {
        if (!m_contentUnpublishListeners.Contains(listener))
        {
            m_contentUnpublishListeners.Add(listener);
        }
    }
    public static void unregisterContentUnpublishListener(IContentUnpublishListener listener)
    {
        if (m_contentUnpublishListeners.Contains(listener))
        {
            m_contentUnpublishListeners.Remove(listener);
        }
    }

    #endregion

    #region Request Gamer Info Listener

    public interface IRequestGamerInfoListener
    {
        void RequestGamerInfoOnSuccess(GamerInfo gamerInfo);
        void RequestGamerInfoOnFailure(int errorCode, string errorMessage);
        void RequestGamerInfoOnCancel();
    }
    private static List<IRequestGamerInfoListener> m_requestGamerInfoListeners = new List<IRequestGamerInfoListener>();
    public static List<IRequestGamerInfoListener> getRequestGamerInfoListeners()
    {
        return m_requestGamerInfoListeners;
    }
    public static void registerRequestGamerInfoListener(IRequestGamerInfoListener listener)
    {
        if (!m_requestGamerInfoListeners.Contains(listener))
        {
            m_requestGamerInfoListeners.Add(listener);
        }
    }
    public static void unregisterRequestGamerInfoListener(IRequestGamerInfoListener listener)
    {
        if (m_requestGamerInfoListeners.Contains(listener))
        {
            m_requestGamerInfoListeners.Remove(listener);
        }
    }

    #endregion

    #region Request Products Listeners

    public interface IRequestProductsListener
    {
        void RequestProductsOnSuccess(List<OuyaSDK.Product> products);
        void RequestProductsOnFailure(int errorCode, string errorMessage);
        void RequestProductsOnCancel();
    }
    private static List<IRequestProductsListener> m_requestProductsListeners = new List<IRequestProductsListener>();
    public static List<IRequestProductsListener> getRequestProductsListeners()
    {
        return m_requestProductsListeners;
    }
    public static void registerRequestProductsListener(IRequestProductsListener listener)
    {
        if (!m_requestProductsListeners.Contains(listener))
        {
            m_requestProductsListeners.Add(listener);
        }
    }
    public static void unregisterRequestProductsListener(IRequestProductsListener listener)
    {
        if (m_requestProductsListeners.Contains(listener))
        {
            m_requestProductsListeners.Remove(listener);
        }
    }

    #endregion

    #region Request Purchase Listener

    public interface IRequestPurchaseListener
    {
        void RequestPurchaseOnSuccess(OuyaSDK.Product product);
        void RequestPurchaseOnFailure(int errorCode, string errorMessage);
        void RequestPurchaseOnCancel();
    }
    private static List<IRequestPurchaseListener> m_requestPurchaseListeners = new List<IRequestPurchaseListener>();
    public static List<IRequestPurchaseListener> getRequestPurchaseListeners()
    {
        return m_requestPurchaseListeners;
    }
    public static void registerRequestPurchaseListener(IRequestPurchaseListener listener)
    {
        if (!m_requestPurchaseListeners.Contains(listener))
        {
            m_requestPurchaseListeners.Add(listener);
        }
    }
    public static void unregisterRequestPurchaseListener(IRequestPurchaseListener listener)
    {
        if (m_requestPurchaseListeners.Contains(listener))
        {
            m_requestPurchaseListeners.Remove(listener);
        }
    }

    #endregion

    #region Request Receipts Listeners

    public interface IRequestReceiptsListener
    {
        void RequestReceiptsOnSuccess(List<Receipt> receipts);
        void RequestReceiptsOnFailure(int errorCode, string errorMessage);
        void RequestReceiptsOnCancel();
    }
    private static List<IRequestReceiptsListener> m_requestReceiptsListeners = new List<IRequestReceiptsListener>();
    public static List<IRequestReceiptsListener> getRequestReceiptsListeners()
    {
        return m_requestReceiptsListeners;
    }
    public static void registerRequestReceiptsListener(IRequestReceiptsListener listener)
    {
        if (!m_requestReceiptsListeners.Contains(listener))
        {
            m_requestReceiptsListeners.Add(listener);
        }
    }
    public static void unregisterRequestReceiptsListener(IRequestReceiptsListener listener)
    {
        if (m_requestReceiptsListeners.Contains(listener))
        {
            m_requestReceiptsListeners.Remove(listener);
        }
    }

    #endregion

#endif
}