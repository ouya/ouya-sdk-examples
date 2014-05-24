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
using Android.Graphics;
using Android.Graphics.Drawables;
using java.io;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;

public class OuyaShowUnityInput : MonoBehaviour,
    OuyaSDK.IJoystickCalibrationListener,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{

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

    private bool m_showCursor = true;

    private string m_axis1 = string.Empty;
    private string m_axis2 = string.Empty;
    private string m_axis3 = string.Empty;
    private string m_axis4 = string.Empty;
    private string m_axis5 = string.Empty;
    private string m_axis6 = string.Empty;
    private string m_axis7 = string.Empty;
    private string m_axis8 = string.Empty;
    private string m_axis9 = string.Empty;
    private string m_axis10 = string.Empty;
    private string m_axis11 = string.Empty;
    private string m_axis12 = string.Empty;
    private string m_axis13 = string.Empty;
    private string m_axis14 = string.Empty;
    private string m_axis15 = string.Empty;
    private string m_axis16 = string.Empty;
    private string m_axis17 = string.Empty;
    private string m_axis18 = string.Empty;
    private string m_axis19 = string.Empty;
    private string m_axis20 = string.Empty;

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
    private string m_button14 = string.Empty;
    private string m_button15 = string.Empty;
    private string m_button16 = string.Empty;
    private string m_button17 = string.Empty;
    private string m_button18 = string.Empty;
    private string m_button19 = string.Empty;
    private string m_button20 = string.Empty;

    private string m_controller1 = string.Empty;
    private string m_controller2 = string.Empty;
    private string m_controller3 = string.Empty;
    private string m_controller4 = string.Empty;
    private string m_controller5 = string.Empty;
    private string m_controller6 = string.Empty;
    private string m_controller7 = string.Empty;
    private string m_controller8 = string.Empty;

    private string m_device = string.Empty;

    #region Thumbstick plots

    public Camera ThumbstickPlotCamera = null;

    #endregion

    private DateTime m_menuDetected = DateTime.MinValue;

    void Awake()
    {
        OuyaSDK.registerJoystickCalibrationListener(this);
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);

        m_backgroundBlack = new Texture2D(1, 1);
        m_backgroundBlack.SetPixel(0, 0, Color.black);
        m_backgroundBlack.Apply();

        m_guiSkin = ScriptableObject.CreateInstance<GUISkin>();
        m_guiSkin.label.normal.textColor = Color.white;
        m_guiSkin.label.normal.background = m_backgroundBlack;

#if UNITY_ANDROID && !UNITY_EDITOR

        int[] buttons =
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

        StringBuilder sb = new StringBuilder();
        foreach (int button in buttons)
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
#endif
    }

    void OnDestroy()
    {
        OuyaSDK.unregisterJoystickCalibrationListener(this);
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    private void Start()
    {
        UpdatePlayerButtons();
        Input.ResetInputAxes();
    }

    public void SetPlayer1()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player1;
        UpdatePlayerButtons();
    }
    public void SetPlayer2()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player2;
        UpdatePlayerButtons();
    }
    public void SetPlayer3()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player3;
        UpdatePlayerButtons();
    }
    public void SetPlayer4()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player4;
        UpdatePlayerButtons();
    }
    public void SetPlayer5()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player5;
        UpdatePlayerButtons();
    }
    public void SetPlayer6()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player6;
        UpdatePlayerButtons();
    }
    public void SetPlayer7()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player7;
        UpdatePlayerButtons();
    }
    public void SetPlayer8()
    {
        OuyaExampleCommon.Player = OuyaSDK.OuyaPlayer.player8;
        UpdatePlayerButtons();
    }

    public void OuyaMenuButtonUp()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        m_showCursor = !m_showCursor;
        Debug.Log(string.Format("OuyaMenuButtonUp: m_showCursor: {0}", m_showCursor));
        OuyaSDK.OuyaJava.JavaShowCursor(m_showCursor);
    }

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

    #region Presentation

#if UNITY_ANDROID && !UNITY_EDITOR
    private void UpdateLegacyAxis(out string field, OuyaSDK.KeyEnum axis)
    {
        field = axis.ToString() + "=" + OuyaExampleCommon.GetAxis(axis, OuyaExampleCommon.Player);
    }
    private void UpdateLegacyButton(out string field, OuyaSDK.KeyEnum button)
    {
        field = button.ToString() + "=" + OuyaExampleCommon.GetButton(button, OuyaExampleCommon.Player);
    }
#endif

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
                            m_controller1 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player1 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 1:
                            m_controller2 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player2 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 2:
                            m_controller3 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player3 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 3:
                            m_controller4 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player4 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 4:
                            m_controller5 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player5 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 5:
                            m_controller6 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player6 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 6:
                            m_controller7 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player7 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 7:
                            m_controller8 = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player8 ? string.Format("*{0}", joyName) : joyName;
                            break;

                    }
                }

#if UNITY_ANDROID && !UNITY_EDITOR

                UpdateLegacyAxis(out m_axis1, OuyaSDK.KeyEnum.AXIS_LSTICK_X);
                UpdateLegacyAxis(out m_axis2, OuyaSDK.KeyEnum.AXIS_LSTICK_Y);
                UpdateLegacyAxis(out m_axis3, OuyaSDK.KeyEnum.AXIS_RSTICK_X);
                UpdateLegacyAxis(out m_axis4, OuyaSDK.KeyEnum.AXIS_RSTICK_Y);
                UpdateLegacyAxis(out m_axis5, OuyaSDK.KeyEnum.AXIS_LT);
                UpdateLegacyAxis(out m_axis6, OuyaSDK.KeyEnum.AXIS_RT);

                UpdateLegacyButton(out m_button1, OuyaSDK.KeyEnum.BUTTON_O);
                UpdateLegacyButton(out m_button2, OuyaSDK.KeyEnum.BUTTON_U);
                UpdateLegacyButton(out m_button3, OuyaSDK.KeyEnum.BUTTON_Y);
                UpdateLegacyButton(out m_button4, OuyaSDK.KeyEnum.BUTTON_A);
                UpdateLegacyButton(out m_button5, OuyaSDK.KeyEnum.BUTTON_LB);
                UpdateLegacyButton(out m_button6, OuyaSDK.KeyEnum.BUTTON_L3);
                UpdateLegacyButton(out m_button7, OuyaSDK.KeyEnum.BUTTON_RB);
                UpdateLegacyButton(out m_button8, OuyaSDK.KeyEnum.BUTTON_R3);
                UpdateLegacyButton(out m_button9, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN);
                UpdateLegacyButton(out m_button10, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT);
                UpdateLegacyButton(out m_button11, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT);
                UpdateLegacyButton(out m_button12, OuyaSDK.KeyEnum.BUTTON_DPAD_UP);
                UpdateLegacyButton(out m_button13, OuyaSDK.KeyEnum.BUTTON_SYSTEM);
#else
                m_button1 = OuyaExampleCommon.GetButton(0, OuyaExampleCommon.Player).ToString();
                m_button2 = OuyaExampleCommon.GetButton(1, OuyaExampleCommon.Player).ToString();
                m_button3 = OuyaExampleCommon.GetButton(2, OuyaExampleCommon.Player).ToString();
                m_button4 = OuyaExampleCommon.GetButton(3, OuyaExampleCommon.Player).ToString();
                m_button5 = OuyaExampleCommon.GetButton(4, OuyaExampleCommon.Player).ToString();
                m_button6 = OuyaExampleCommon.GetButton(5, OuyaExampleCommon.Player).ToString();
                m_button7 = OuyaExampleCommon.GetButton(6, OuyaExampleCommon.Player).ToString();
                m_button8 = OuyaExampleCommon.GetButton(7, OuyaExampleCommon.Player).ToString();
                m_button9 = OuyaExampleCommon.GetButton(8, OuyaExampleCommon.Player).ToString();
                m_button10 = OuyaExampleCommon.GetButton(9, OuyaExampleCommon.Player).ToString();
                m_button11 = OuyaExampleCommon.GetButton(10, OuyaExampleCommon.Player).ToString();
                m_button12 = OuyaExampleCommon.GetButton(11, OuyaExampleCommon.Player).ToString();
                m_button13 = OuyaExampleCommon.GetButton(12, OuyaExampleCommon.Player).ToString();
                m_button14 = OuyaExampleCommon.GetButton(13, OuyaExampleCommon.Player).ToString();
                m_button15 = OuyaExampleCommon.GetButton(14, OuyaExampleCommon.Player).ToString();
                m_button16 = OuyaExampleCommon.GetButton(15, OuyaExampleCommon.Player).ToString();
                m_button17 = OuyaExampleCommon.GetButton(16, OuyaExampleCommon.Player).ToString();
                m_button18 = OuyaExampleCommon.GetButton(17, OuyaExampleCommon.Player).ToString();
                m_button19 = OuyaExampleCommon.GetButton(18, OuyaExampleCommon.Player).ToString();
                m_button20 = OuyaExampleCommon.GetButton(19, OuyaExampleCommon.Player).ToString();
#endif

                m_device = SystemInfo.deviceModel;
            }
        }
        catch (System.Exception)
        {
        }
    }

#if !UNITY_ANDROID || UNITY_EDITOR
    void Update()
    {
            m_axis1 = Input.GetAxisRaw(string.Format("Joy{0} Axis 1", (int) OuyaExampleCommon.Player)).ToString("F5");
            m_axis2 = Input.GetAxisRaw(string.Format("Joy{0} Axis 2", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis3 = Input.GetAxisRaw(string.Format("Joy{0} Axis 3", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis4 = Input.GetAxisRaw(string.Format("Joy{0} Axis 4", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis5 = Input.GetAxisRaw(string.Format("Joy{0} Axis 5", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis6 = Input.GetAxisRaw(string.Format("Joy{0} Axis 6", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis7 = Input.GetAxisRaw(string.Format("Joy{0} Axis 7", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis8 = Input.GetAxisRaw(string.Format("Joy{0} Axis 8", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis9 = Input.GetAxisRaw(string.Format("Joy{0} Axis 9", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis10 = Input.GetAxisRaw(string.Format("Joy{0} Axis 10", (int)OuyaExampleCommon.Player)).ToString("F5");
#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
            m_axis11 = Input.GetAxisRaw(string.Format("Joy{0} Axis 11", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis12 = Input.GetAxisRaw(string.Format("Joy{0} Axis 12", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis13 = Input.GetAxisRaw(string.Format("Joy{0} Axis 13", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis14 = Input.GetAxisRaw(string.Format("Joy{0} Axis 14", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis15 = Input.GetAxisRaw(string.Format("Joy{0} Axis 15", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis16 = Input.GetAxisRaw(string.Format("Joy{0} Axis 16", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis17 = Input.GetAxisRaw(string.Format("Joy{0} Axis 17", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis18 = Input.GetAxisRaw(string.Format("Joy{0} Axis 18", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis19 = Input.GetAxisRaw(string.Format("Joy{0} Axis 19", (int)OuyaExampleCommon.Player)).ToString("F5");
            m_axis20 = Input.GetAxisRaw(string.Format("Joy{0} Axis 20", (int)OuyaExampleCommon.Player)).ToString("F5");
#else
            m_axis11 = "N/A";
            m_axis12 = "N/A";
            m_axis13 = "N/A";
            m_axis14 = "N/A";
            m_axis15 = "N/A";
            m_axis16 = "N/A";
            m_axis17 = "N/A";
            m_axis18 = "N/A";
            m_axis19 = "N/A";
            m_axis20 = "N/A";
#endif
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

        GUILayout.Space(80);

#if !(UNITY_ANDROID && !UNITY_EDITOR)

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Axis:");
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("{0}={1}", 1, m_axis1));
        GUILayout.Label(string.Format("{0}={1}", 2, m_axis2));
        GUILayout.Label(string.Format("{0}={1}", 3, m_axis3));
        GUILayout.Label(string.Format("{0}={1}", 4, m_axis4));
        GUILayout.Label(string.Format("{0}={1}", 5, m_axis5));
        GUILayout.Label(string.Format("{0}={1}", 6, m_axis6));
        GUILayout.Label(string.Format("{0}={1}", 7, m_axis7));
        GUILayout.Label(string.Format("{0}={1}", 8, m_axis8));
        GUILayout.Label(string.Format("{0}={1}", 9, m_axis9));
        GUILayout.Label(string.Format("{0}={1}", 10, m_axis10));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("{0}={1}", 11, m_axis11));
        GUILayout.Label(string.Format("{0}={1}", 12, m_axis12));
        GUILayout.Label(string.Format("{0}={1}", 13, m_axis13));
        GUILayout.Label(string.Format("{0}={1}", 14, m_axis14));
        GUILayout.Label(string.Format("{0}={1}", 15, m_axis15));
        GUILayout.Label(string.Format("{0}={1}", 16, m_axis16));
        GUILayout.Label(string.Format("{0}={1}", 17, m_axis17));
        GUILayout.Label(string.Format("{0}={1}", 18, m_axis18));
        GUILayout.Label(string.Format("{0}={1}", 19, m_axis19));
        GUILayout.Label(string.Format("{0}={1}", 20, m_axis20));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Button:");
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("{0}={1}", 1, m_button1));
        GUILayout.Label(string.Format("{0}={1}", 2, m_button2));
        GUILayout.Label(string.Format("{0}={1}", 3, m_button3));
        GUILayout.Label(string.Format("{0}={1}", 4, m_button4));
        GUILayout.Label(string.Format("{0}={1}", 5, m_button5));
        GUILayout.Label(string.Format("{0}={1}", 6, m_button6));
        GUILayout.Label(string.Format("{0}={1}", 7, m_button7));
        GUILayout.Label(string.Format("{0}={1}", 8, m_button8));
        GUILayout.Label(string.Format("{0}={1}", 9, m_button9));
        GUILayout.Label(string.Format("{0}={1}", 10, m_button10));
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("{0}={1}", 11, m_button11));
        GUILayout.Label(string.Format("{0}={1}", 12, m_button12));
        GUILayout.Label(string.Format("{0}={1}", 13, m_button13));
        GUILayout.Label(string.Format("{0}={1}", 14, m_button14));
        GUILayout.Label(string.Format("{0}={1}", 15, m_button15));
        GUILayout.Label(string.Format("{0}={1}", 16, m_button16));
        GUILayout.Label(string.Format("{0}={1}", 17, m_button17));
        GUILayout.Label(string.Format("{0}={1}", 18, m_button18));
        GUILayout.Label(string.Format("{0}={1}", 19, m_button19));
        GUILayout.Label(string.Format("{0}={1}", 20, m_button20));
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
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

        GUI.skin = null;
        GUILayout.BeginVertical();
        for (int index = 1; index <= 8; ++index)
        {
            if (GUILayout.Button(string.Format("JOY{0}", index), GUILayout.Height(40)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

#endif

#if UNITY_ANDROID && !UNITY_EDITOR

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();

        int deviceId = (int) OuyaExampleCommon.Player - 1;

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
        for (int index = 1; index <= 8; ++index)
        {
            if (GUILayout.Button(string.Format(OuyaExampleCommon.Player == (OuyaSDK.OuyaPlayer)index ? "[JOY{0}]{1}" : "JOY{0}{1}", index, OuyaSDK.OuyaInput.IsControllerConnected(index-1) ? " |=" : " |X"), GUILayout.Height(60)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndVertical();

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

        GUILayout.BeginHorizontal();
        GUILayout.Label("Legacy:");
        GUILayout.BeginVertical();
        GUILayout.Label(m_axis1);
        GUILayout.Label(m_axis2);
        GUILayout.Label(m_axis3);
        GUILayout.Label(m_axis4);
        GUILayout.Label(m_axis5);
        GUILayout.Label(m_axis6);
        GUILayout.Label(m_axis7);
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Legacy:");
        GUILayout.BeginVertical();
        GUILayout.Label(m_button1);
        GUILayout.Label(m_button2);
        GUILayout.Label(m_button3);
        GUILayout.Label(m_button4);
        GUILayout.Label(m_button5);
        GUILayout.Label(m_button6);
        GUILayout.Label(m_button7);
        GUILayout.Label(m_button8);
        GUILayout.Label(m_button9);
        GUILayout.Label(m_button10);
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label(m_button11);
        GUILayout.Label(m_button12);
        GUILayout.Label(m_button13);
        GUILayout.Label(m_button14);
        GUILayout.EndVertical();
        
        GUILayout.FlexibleSpace();

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

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(300));

        GUILayout.FlexibleSpace();

        foreach (Texture2D texture in m_controllerButtons)
        {
            if (null != texture)
            {
                GUILayout.Button(texture);
            }
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
#endif

        GUI.skin = null;
    }

    void UpdatePlayerButtons()
    {
        if (ThumbstickPlotCamera)
        {
            foreach (OuyaPlotMeshThumbstick plot in ThumbstickPlotCamera.GetComponents<OuyaPlotMeshThumbstick>())
            {
                plot.Player = OuyaExampleCommon.Player;
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

    private float GetAxis(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
        return OuyaExampleCommon.GetAxis(keyCode, player);
    }

    private bool GetButton(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
        return OuyaExampleCommon.GetButton(keyCode, player);
    }

    private void UpdateController()
    {
        #region Axis Code

        UpdateHighlight(RendererAxisLeft, Mathf.Abs(GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_X, OuyaExampleCommon.Player)) > 0.25f ||
            Mathf.Abs(GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_Y, OuyaExampleCommon.Player)) > 0.25f);

        RendererAxisLeft.transform.localRotation = Quaternion.Euler(GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_Y, OuyaExampleCommon.Player) * 15, 0, GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_X, OuyaExampleCommon.Player) * 15);

        UpdateHighlight(RendererAxisRight, Mathf.Abs(GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_X, OuyaExampleCommon.Player)) > 0.25f ||
            Mathf.Abs(GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_Y, OuyaExampleCommon.Player)) > 0.25f);

        RendererAxisRight.transform.localRotation = Quaternion.Euler(GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_Y, OuyaExampleCommon.Player) * 15, 0, GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_X, OuyaExampleCommon.Player) * 15);

        RendererLT.transform.localRotation = Quaternion.Euler(GetAxis(OuyaSDK.KeyEnum.BUTTON_LT, OuyaExampleCommon.Player) * -15, 0, 0);

        RendererRT.transform.localRotation = Quaternion.Euler(GetAxis(OuyaSDK.KeyEnum.BUTTON_RT, OuyaExampleCommon.Player) * -15, 0, 0);

        if (GetButton(OuyaSDK.KeyEnum.BUTTON_L3, OuyaExampleCommon.Player))
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 0.75f, -3.344945f), Time.fixedDeltaTime);
        }
        else
        {
            RendererAxisLeft.transform.localPosition = Vector3.Lerp(RendererAxisLeft.transform.localPosition,
                                                                     new Vector3(5.503977f, 1.127527f, -3.344945f), Time.fixedDeltaTime);
        }

        if (GetButton(OuyaSDK.KeyEnum.BUTTON_R3, OuyaExampleCommon.Player))
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
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_O, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererButtonO, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonO, false, true);
        }

        //Check U button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_U, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererButtonU, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonU, false, true);
        }

        //Check Y button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_Y, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererButtonY, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonY, false, true);
        }

        //Check A button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_A, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererButtonA, true, true);
        }
        else
        {
            UpdateHighlight(RendererButtonA, false, true);
        }

        //Check L3 button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_L3, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererAxisLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererAxisLeft, false, true);
        }

        //Check R3 button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_R3, OuyaExampleCommon.Player))
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
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_LB, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererLB, true, true);
        }
        else
        {
            UpdateHighlight(RendererLB, false, true);
        }

        //Check RB button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_RB, OuyaExampleCommon.Player))
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
        if (GetAxis(OuyaSDK.KeyEnum.BUTTON_LT, OuyaExampleCommon.Player) > 0.25f)
        {
            UpdateHighlight(RendererLT, true, true);
        }
        else
        {
            UpdateHighlight(RendererLT, false, true);
        }

        //Check RT button for down state
        if (GetAxis(OuyaSDK.KeyEnum.BUTTON_RT, OuyaExampleCommon.Player) > 0.25f)
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
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_UP, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererDpadUp, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadUp, false, true);
        }

        //Check DPAD DOWN button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererDpadDown, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadDown, false, true);
        }

        //Check DPAD LEFT button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererDpadLeft, true, true);
        }
        else
        {
            UpdateHighlight(RendererDpadLeft, false, true);
        }

        //Check DPAD RIGHT button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, OuyaExampleCommon.Player))
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

#if UNITY_ANDROID && !UNITY_EDITOR

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

#endif

    #endregion
}