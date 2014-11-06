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

#if UNITY_ANDROID && !UNITY_EDITOR
//#define PERFORMANCE_TEST_MEASURE_LATENCY
using Android.Graphics;
using Android.Graphics.Drawables;
using com.unity3d.player;
using java.io;
using System.Threading;
using tv.ouya.console.api;
using tv.ouya.sdk;
#endif
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OuyaShowUnityInput : MonoBehaviour
#if UNITY_ANDROID && !UNITY_EDITOR
    ,
    OuyaSDK.IJoystickCalibrationListener,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuAppearingListener
#endif
{
    public static int PlayerNum = 0;

    private GUISkin m_guiSkin = null;
    private Texture2D m_backgroundBlack = null;

    #region Model reference fields

    public Material ControllerMaterial;

    public MeshRenderer RendererAxisLeft = null;
    public MeshRenderer RendererAxisRight = null;
    public MeshRenderer RendererButtonA = null;
    public MeshRenderer RendererButtonO = null;
    public MeshRenderer RendererButtonU = null;
    public MeshRenderer RendererButtonY = null;
    public MeshRenderer RendererDpadDown = null;
    public MeshRenderer RendererDpadLeft = null;
    public MeshRenderer RendererDpadRight = null;
    public MeshRenderer RendererDpadUp = null;
    public MeshRenderer RendererLB = null;
    public MeshRenderer RendererLT = null;
    public MeshRenderer RendererRB = null;
    public MeshRenderer RendererRT = null;

    #endregion

    private string m_axis1 = string.Empty;
    private string m_axis2 = string.Empty;
    private string m_axis3 = string.Empty;
    private string m_axis4 = string.Empty;
    private string m_axis5 = string.Empty;
    private string m_axis6 = string.Empty;

    private string m_button1 = string.Empty;
    private string m_button2 = string.Empty;
    private string m_button3 = string.Empty;
    private string m_button4 = string.Empty;
    private string m_button5 = string.Empty;
    private string m_button6 = string.Empty;
    private string m_button7 = string.Empty;
    private string m_button8 = string.Empty;
    private string m_button9 = string.Empty;
    private string m_button10 = string.Empty;
    private string m_button11 = string.Empty;
    private string m_button12 = string.Empty;
    private string m_button13 = string.Empty;

    private string m_controller1 = string.Empty;
    private string m_controller2 = string.Empty;
    private string m_controller3 = string.Empty;
    private string m_controller4 = string.Empty;
    private string m_controller5 = string.Empty;
    private string m_controller6 = string.Empty;
    private string m_controller7 = string.Empty;
    private string m_controller8 = string.Empty;

    #region Thumbstick plots

    public Camera ThumbstickPlotCamera = null;

    #endregion

    private DateTime m_menuDetected = DateTime.MinValue;

    public void OuyaMenuAppearing()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnPause()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnResume()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaOnJoystickCalibration()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

#if UNITY_ANDROID && !UNITY_EDITOR

    private OuyaUnityActivity m_activity = null;

    private int[] Buttons =
    {
        OuyaController.BUTTON_O,
        OuyaController.BUTTON_U,
        OuyaController.BUTTON_Y,
        OuyaController.BUTTON_A,
        OuyaController.BUTTON_L1,
        OuyaController.BUTTON_L3,
        OuyaController.BUTTON_R1,
        OuyaController.BUTTON_R3,
        OuyaController.BUTTON_DPAD_DOWN,
        OuyaController.BUTTON_DPAD_LEFT,
        OuyaController.BUTTON_DPAD_RIGHT,
        OuyaController.BUTTON_DPAD_UP,
        OuyaController.BUTTON_MENU,
    };

    private void Awake()
    {
        m_activity = new OuyaUnityActivity(UnityPlayer.currentActivity);

#if PERFORMANCE_TEST_MEASURE_LATENCY
        ThreadStart ts = new ThreadStart(TestWorker);
        Thread thread = new Thread(ts);
        thread.Start();
#endif

        OuyaSDK.registerJoystickCalibrationListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);

        m_backgroundBlack = new Texture2D(1, 1);
        m_backgroundBlack.SetPixel(0, 0, Color.black);
        m_backgroundBlack.Apply();

        m_guiSkin = ScriptableObject.CreateInstance<GUISkin>();
        m_guiSkin.label.normal.textColor = Color.white;
        m_guiSkin.label.normal.background = m_backgroundBlack;

        StringBuilder sb = new StringBuilder();
        foreach (int button in Buttons)
        {
            Setup(button);

            string label;
            SetLabel(out label, button);

            if (!string.IsNullOrEmpty(label))
            {
                sb.Append(label);
                sb.Append(" ");
            }
            m_label = "Hello from Unity: " + sb.ToString();
        }
    }

#if PERFORMANCE_TEST_MEASURE_LATENCY
    private bool m_waitForExit = true;
    void OnApplicationQuit()
    {
        m_waitForExit = false;
    }
#endif
    private void OnDestroy()
    {
#if PERFORMANCE_TEST_MEASURE_LATENCY
        m_waitForExit = false;
#endif
        OuyaSDK.unregisterJoystickCalibrationListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    private void Start()
    {
        UpdatePlayerButtons();
    }

    #region Presentation

    private void UpdateLabelAxis(out string field, int axis)
    {
        field = DebugInput.DebugGetAxisName(axis) + "=" + OuyaSDK.OuyaInput.GetAxis(PlayerNum, axis);
    }

    private void UpdateLabelButton(out string field, int button)
    {
        field = DebugInput.DebugGetButtonName(button) + "=" + OuyaSDK.OuyaInput.GetButton(PlayerNum, button);
    }

    private void UpdateLabels()
    {
        try
        {
            if (null != OuyaSDK.Joysticks)
            {
                for (int i = 0; i < OuyaSDK.Joysticks.Length || i < 8; i++)
                {
                    string joyName = "N/A";
                    if (i < OuyaSDK.Joysticks.Length)
                    {
                        joyName = OuyaSDK.Joysticks[i];
                    }
                    if (null == joyName)
                    {
                        joyName = "N/A";
                    }
                    switch (i)
                    {
                        case 0:
                            m_controller1 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 1:
                            m_controller2 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 2:
                            m_controller3 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 3:
                            m_controller4 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 4:
                            m_controller5 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 5:
                            m_controller6 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 6:
                            m_controller7 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 7:
                            m_controller8 = PlayerNum == i ? string.Format("*{0}", joyName) : joyName;
                            break;

                    }
                }

                UpdateLabelAxis(out m_axis1, OuyaController.AXIS_LS_X);
                UpdateLabelAxis(out m_axis2, OuyaController.AXIS_LS_Y);
                UpdateLabelAxis(out m_axis3, OuyaController.AXIS_RS_X);
                UpdateLabelAxis(out m_axis4, OuyaController.AXIS_RS_Y);
                UpdateLabelAxis(out m_axis5, OuyaController.AXIS_L2);
                UpdateLabelAxis(out m_axis6, OuyaController.AXIS_R2);

                UpdateLabelButton(out m_button1, OuyaController.BUTTON_O);
                UpdateLabelButton(out m_button2, OuyaController.BUTTON_U);
                UpdateLabelButton(out m_button3, OuyaController.BUTTON_Y);
                UpdateLabelButton(out m_button4, OuyaController.BUTTON_A);
                UpdateLabelButton(out m_button5, OuyaController.BUTTON_L1);
                UpdateLabelButton(out m_button6, OuyaController.BUTTON_R1);
                UpdateLabelButton(out m_button7, OuyaController.BUTTON_L3);
                UpdateLabelButton(out m_button8, OuyaController.BUTTON_R3);
                UpdateLabelButton(out m_button9, OuyaController.BUTTON_DPAD_DOWN);
                UpdateLabelButton(out m_button10, OuyaController.BUTTON_DPAD_LEFT);
                UpdateLabelButton(out m_button11, OuyaController.BUTTON_DPAD_RIGHT);
                UpdateLabelButton(out m_button12, OuyaController.BUTTON_DPAD_UP);
                UpdateLabelButton(out m_button13, OuyaController.BUTTON_MENU);
            }
        }
        catch (System.Exception)
        {
        }
    }

#if PERFORMANCE_TEST_MEASURE_LATENCY
    private void TestWorker()
    {
        while (m_waitForExit)
        {
            foreach (int button in Buttons)
            {
                if (OuyaSDK.OuyaInput.GetButtonDown(0, button))
                {
                    m_activity.debugDisplayKeyDownElapsed();
                }

                if (OuyaSDK.OuyaInput.GetButtonUp(0, button))
                {
                    m_activity.debugDisplayKeyUpElapsed();
                }
            }
            Thread.Sleep(0);
        }
    }
#endif

    void FixedUpdate()
    {
        UpdateController();
        UpdateLabels();
    }

    void OnGUI()
    {
        GUI.skin = m_guiSkin;

        GUILayout.Space(40);

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("isRunningOnOUYASupportedHardware: {0}",
            OuyaSDK.isRunningOnOUYASupportedHardware()),
            GUILayout.Width(300));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();

        int deviceId = PlayerNum;

        GUILayout.BeginVertical();
        GUILayout.Label("OUYA Axises:");
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_LS_X", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_LS_X)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_LS_Y", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_LS_Y)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_RS_X", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_RS_X)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_RS_Y", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_RS_Y)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_L2", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_L2)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.AXIS_R2", OuyaSDK.OuyaInput.GetAxis(deviceId, OuyaController.AXIS_R2)));
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        GUILayout.Label("OUYA Buttons:");
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_O", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_O)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_U", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_U)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_Y", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_Y)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_A", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_A)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_L1", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_L1)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_R1", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_R1)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_L3", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_L3)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_R3", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_R3)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_DPAD_UP", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_DPAD_UP)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_DPAD_DOWN", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_DPAD_DOWN)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_DPAD_RIGHT", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_DPAD_RIGHT)));
        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_DPAD_LEFT", OuyaSDK.OuyaInput.GetButton(deviceId, OuyaController.BUTTON_DPAD_LEFT)));

        if (OuyaSDK.OuyaInput.GetButtonDown(deviceId, OuyaController.BUTTON_MENU))
        {
            m_menuDetected = DateTime.Now + TimeSpan.FromSeconds(1);
        }

        GUILayout.Label(string.Format("{0}={1}", "OuyaController.BUTTON_MENU", DateTime.Now < m_menuDetected));
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUI.skin = null;
        GUILayout.BeginVertical();
        for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index)
        {
            if (GUILayout.Button(string.Format(PlayerNum == index ? "[JOY{0}]{1}" : "JOY{0}{1}", index, OuyaSDK.OuyaInput.IsControllerConnected(index) ? " |=" : " |X"), GUILayout.Height(60)))
            {
                PlayerNum = index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Hide Cursor"))
        {
            OuyaController.showCursor(false);
        }

        GUILayout.FlexibleSpace();
        
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();

        GUI.skin = m_guiSkin;
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        GUILayout.Label("Controllers:");
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("{0}={1}", 1, m_controller1));
        GUILayout.Label(string.Format("{0}={1}", 2, m_controller2));
        GUILayout.Label(string.Format("{0}={1}", 3, m_controller3));
        GUILayout.Label(string.Format("{0}={1}", 4, m_controller4));
        GUILayout.Label(string.Format("{0}={1}", 5, m_controller5));
        GUILayout.Label(string.Format("{0}={1}", 6, m_controller6));
        GUILayout.Label(string.Format("{0}={1}", 7, m_controller7));
        GUILayout.Label(string.Format("{0}={1}", 8, m_controller8));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();

        GUILayout.Label(m_label);

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        int buttonId = 0;

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        for (; buttonId < 7 && buttonId < m_controllerButtons.Count; ++buttonId)
        {
            Texture2D texture = m_controllerButtons[buttonId];
            if (null != texture)
            {
                GUILayout.Button(texture);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        for (; buttonId < m_controllerButtons.Count; ++buttonId)
        {
            Texture2D texture = m_controllerButtons[buttonId];
            if (null != texture)
            {
                GUILayout.Button(texture);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        GUI.skin = null;
    }

    void UpdatePlayerButtons()
    {
        if (ThumbstickPlotCamera)
        {
            OuyaPlotAxisButton plotAxisButton = ThumbstickPlotCamera.GetComponent<OuyaPlotAxisButton>();
            plotAxisButton.PlayerNum = PlayerNum;

            OuyaPlotMeshThumbstick[] items = ThumbstickPlotCamera.GetComponents<OuyaPlotMeshThumbstick>();
            for (int index = 0; index < items.Length; ++index)
            {
                OuyaPlotMeshThumbstick plot = items[index];
                switch (index)
                {
                    case 0:
                        plot.AxisX = OuyaController.AXIS_LS_X;
                        plot.AxisY = OuyaController.AXIS_LS_Y;
                        break;
                    case 1:
                        plot.AxisX = OuyaController.AXIS_RS_X;
                        plot.AxisY = OuyaController.AXIS_RS_Y;
                        break;
                }
                plot.PlayerNum = PlayerNum;
            }
        }
    }

    #endregion

    #region Extra

    private void UpdateHighlight(MeshRenderer mr, bool highlight, bool instant = false)
    {
        float time = Time.deltaTime * 10;
        if (instant) { time = 1000; }

        if (highlight)
        {
            Color c = new Color(0, 10, 0, 1);
            mr.material.color = Color.Lerp(mr.material.color, c, time);
        }
        else
        {
            mr.material.color = Color.Lerp(mr.material.color, Color.white, time);
        }
    }

    private void UpdateController()
    {
        #region Axis Code

        UpdateHighlight(RendererAxisLeft, Mathf.Abs(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_LS_X)) > 0.25f ||
            Mathf.Abs(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_LS_Y)) > 0.25f);

        RendererAxisLeft.transform.localRotation = Quaternion.Euler(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_LS_Y) * 15, 0,
            OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_LS_X) * 15);

        UpdateHighlight(RendererAxisRight, Mathf.Abs(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_RS_X)) > 0.25f ||
            Mathf.Abs(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_RS_Y)) > 0.25f);

        RendererAxisRight.transform.localRotation = Quaternion.Euler(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_RS_Y) * 15, 0,
            OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_RS_X) * 15);

        RendererLT.transform.localRotation = Quaternion.Euler(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_L2) * -15, 0, 0);

        RendererRT.transform.localRotation = Quaternion.Euler(OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_R2) * -15, 0, 0);

        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_L3))
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 0.75f, -3.344945f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 1.127527f, -3.344945f), Time.fixedDeltaTime);
        }

        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_R3))
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 0.75f, -1.354063f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisRight.transform.localPosition = Vector3.Lerp(RendererAxisRight.transform.localPosition,
                                                                     new Vector3(-2.707688f, 1.11295f, -1.354063f), Time.fixedDeltaTime);
        }

        #endregion


        #region Button Code

        #region BUTTONS O - A
        //Check O button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_O))
        {
            UpdateHighlight(RendererButtonO, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonO, false, true);
        }

        //Check U button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_U))
        {
            UpdateHighlight(RendererButtonU, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonU, false, true);
        }

        //Check Y button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_Y))
        {
            UpdateHighlight(RendererButtonY, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonY, false, true);
        }

        //Check A button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_A))
        {
            UpdateHighlight(RendererButtonA, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonA, false, true);
        }

        //Check L3 button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_L3))
        {
            UpdateHighlight(RendererAxisLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererAxisLeft, false, true);
        }

        //Check R3 button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_R3))
        {
            UpdateHighlight(RendererAxisRight, true, true);
        }
        else
        {
            UpdateHighlight(RendererAxisRight, false, true);
        }
        #endregion

        #region Bumpers
        //Check LB button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_L1))
        {
            UpdateHighlight(RendererLB, true, true);
        }
        else
        {
            UpdateHighlight(RendererLB, false, true);
        }

        //Check RB button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_R1))
        {
            UpdateHighlight(RendererRB, true, true);
        }
        else
        {
            UpdateHighlight(RendererRB, false, true);
        }
        #endregion

        #region triggers
        //Check LT button for down state
        if (OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_L2) > 0.25f)
        {
            UpdateHighlight(RendererLT, true, true);
        }
        else
        {
            UpdateHighlight(RendererLT, false, true);
        }

        //Check RT button for down state
        if (OuyaSDK.OuyaInput.GetAxis(PlayerNum, OuyaController.AXIS_R2) > 0.25f)
        {
            UpdateHighlight(RendererRT, true, true);
        }
        else
        {
            UpdateHighlight(RendererRT, false, true);
        }
        #endregion

        #region DPAD
        //Check DPAD UP button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_DPAD_UP))
        {
            UpdateHighlight(RendererDpadUp, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadUp, false, true);
        }

        //Check DPAD DOWN button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_DPAD_DOWN))
        {
            UpdateHighlight(RendererDpadDown, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadDown, false, true);
        }

        //Check DPAD LEFT button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_DPAD_LEFT))
        {
            UpdateHighlight(RendererDpadLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadLeft, false, true);
        }

        //Check DPAD RIGHT button for down state
        if (OuyaSDK.OuyaInput.GetButton(PlayerNum, OuyaController.BUTTON_DPAD_RIGHT))
        {
            UpdateHighlight(RendererDpadRight, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadRight, false, true);
        }
        #endregion

        #endregion
    }

    #endregion

    #region Controller Button Images

    private List<Texture2D> m_controllerButtons = new List<Texture2D>();

    private string m_label = string.Empty;

    private void SetDrawable(out Texture2D button, int keyCode)
    {
        OuyaController.ButtonData buttonData;
        buttonData = OuyaController.getButtonData(keyCode);
        if (null == buttonData)
        {
            button = null;
            return;
        }

        if (null == buttonData.buttonDrawable)
        {
            button = null;
            return;
        }

        BitmapDrawable drawable = (BitmapDrawable)buttonData.buttonDrawable;
        if (null == drawable)
        {
            button = null;
            return;
        }

        Bitmap bitmap = drawable.getBitmap();
        if (null == bitmap)
        {
            button = null;
            return;
        }

        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
        if (stream.size() == 0)
        {
            button = null;
        }
        else
        {
            button = new Texture2D(0, 0);
            button.LoadImage(stream.toByteArray());
        }
        stream.close();
    }

    private void SetLabel(out string label, int keyCode)
    {
        OuyaController.ButtonData buttonData;
        buttonData = OuyaController.getButtonData(keyCode);
        if (null == buttonData)
        {
            label = null;
            return;
        }
        label = buttonData.buttonName;
    }

    void Setup(int keyCode)
    {
        Texture2D button;
        SetDrawable(out button, keyCode);
        if (null != button)
        {
            m_controllerButtons.Add(button);
        }
    }

    #endregion

#endif
}