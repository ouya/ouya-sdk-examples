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

using UnityEngine;

public class OuyaShowUnityInput : MonoBehaviour,
    OuyaSDK.IJoystickCalibrationListener,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{
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

    #region Thumbstick plots

    public Camera ThumbstickPlotCamera = null;

    #endregion

    #region New Input API - in progress

    private bool m_useSDKForInput = false;

    #endregion

    void Awake()
    {
        OuyaSDK.registerJoystickCalibrationListener(this);
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
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

    private void UpdateLabels()
    {
        try
        {
            OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
            if (nguiHandler != null &&
                null != OuyaSDK.Joysticks)
            {
                for (int i = 0; i < OuyaSDK.Joysticks.Length || i < 8; i++)
                {
                    string joyName = "N/A";
                    if (i < OuyaSDK.Joysticks.Length)
                    {
                        joyName = OuyaSDK.Joysticks[i];
                    }
                    switch (i)
                    {
                        case 0:
                            nguiHandler.controller1.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player1 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 1:
                            nguiHandler.controller2.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player2 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 2:
                            nguiHandler.controller3.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player3 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 3:
                            nguiHandler.controller4.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player4 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 4:
                            nguiHandler.controller5.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player5 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 5:
                            nguiHandler.controller6.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player6 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 6:
                            nguiHandler.controller7.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player7 ? string.Format("*{0}", joyName) : joyName;
                            break;
                        case 7:
                            nguiHandler.controller8.text = OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player8 ? string.Format("*{0}", joyName) : joyName;
                            break;

                    }
                }

                nguiHandler.button1.text = OuyaExampleCommon.GetButton(0, OuyaExampleCommon.Player).ToString();
                nguiHandler.button2.text = OuyaExampleCommon.GetButton(1, OuyaExampleCommon.Player).ToString();
                nguiHandler.button3.text = OuyaExampleCommon.GetButton(2, OuyaExampleCommon.Player).ToString();
                nguiHandler.button4.text = OuyaExampleCommon.GetButton(3, OuyaExampleCommon.Player).ToString();
                nguiHandler.button5.text = OuyaExampleCommon.GetButton(4, OuyaExampleCommon.Player).ToString();
                nguiHandler.button6.text = OuyaExampleCommon.GetButton(5, OuyaExampleCommon.Player).ToString();
                nguiHandler.button7.text = OuyaExampleCommon.GetButton(6, OuyaExampleCommon.Player).ToString();
                nguiHandler.button8.text = OuyaExampleCommon.GetButton(7, OuyaExampleCommon.Player).ToString();
                nguiHandler.button9.text = OuyaExampleCommon.GetButton(8, OuyaExampleCommon.Player).ToString();
                nguiHandler.button10.text = OuyaExampleCommon.GetButton(9, OuyaExampleCommon.Player).ToString();
                nguiHandler.button11.text = OuyaExampleCommon.GetButton(10, OuyaExampleCommon.Player).ToString();
                nguiHandler.button12.text = OuyaExampleCommon.GetButton(11, OuyaExampleCommon.Player).ToString();
                nguiHandler.button13.text = OuyaExampleCommon.GetButton(12, OuyaExampleCommon.Player).ToString();
                nguiHandler.button14.text = OuyaExampleCommon.GetButton(13, OuyaExampleCommon.Player).ToString();
                nguiHandler.button15.text = OuyaExampleCommon.GetButton(14, OuyaExampleCommon.Player).ToString();
                nguiHandler.button16.text = OuyaExampleCommon.GetButton(15, OuyaExampleCommon.Player).ToString();
                nguiHandler.button17.text = OuyaExampleCommon.GetButton(16, OuyaExampleCommon.Player).ToString();
                nguiHandler.button18.text = OuyaExampleCommon.GetButton(17, OuyaExampleCommon.Player).ToString();
                nguiHandler.button19.text = OuyaExampleCommon.GetButton(18, OuyaExampleCommon.Player).ToString();
                nguiHandler.button20.text = OuyaExampleCommon.GetButton(19, OuyaExampleCommon.Player).ToString();

                //nguiHandler.rawOut.text = OuyaGameObject.InputData;
                nguiHandler.device.text = SystemInfo.deviceModel;
            }
        }
        catch (System.Exception)
        {
        }
    }

    void Update()
    {
        OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
        if (nguiHandler != null)
        {
            // Input.GetAxis("Joy1 Axis1");
            nguiHandler.axis1.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 1", (int) OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis2.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 2", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis3.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 3", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis4.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 4", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis5.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 5", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis6.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 6", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis7.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 7", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis8.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 8", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis9.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 9", (int)OuyaExampleCommon.Player)).ToString("F5");
            nguiHandler.axis10.text = Input.GetAxisRaw(string.Format("Joy{0} Axis 10", (int)OuyaExampleCommon.Player)).ToString("F5");
        }
    }

    void FixedUpdate()
    {
        UpdateController();
        UpdateLabels();
    }

    void OnGUI()
    {
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.Space(600);
        for (int index = 1; index <= 4; ++index)
        {
            if (GUILayout.Button(string.Format("JOY{0}", index), GUILayout.MaxHeight(40)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(600);
        for (int index = 5; index <= 8; ++index)
        {
            if (GUILayout.Button(string.Format("JOY{0}", index), GUILayout.MaxHeight(40)))
            {
                OuyaExampleCommon.Player = (OuyaSDK.OuyaPlayer)index;
                UpdatePlayerButtons();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(600);
        m_useSDKForInput = GUILayout.Toggle(m_useSDKForInput, "Use OuyaSDK Mappings and Unity Input", GUILayout.MinHeight(45));
        GUILayout.EndHorizontal();
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

        OuyaNGUIHandler nguiHandler = GameObject.Find("_NGUIHandler").GetComponent<OuyaNGUIHandler>();
        if (nguiHandler != null)
        {
            nguiHandler.player1.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player1);
            nguiHandler.player2.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player2);
            nguiHandler.player3.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player3);
            nguiHandler.player4.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player4);
            nguiHandler.player5.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player5);
            nguiHandler.player6.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player6);
            nguiHandler.player7.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player7);
            nguiHandler.player8.SendMessage("OnHover", OuyaExampleCommon.Player == OuyaSDK.OuyaPlayer.player8);

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
        // Check if we want the *new* SDK input or the example common
        if (m_useSDKForInput)
        {
            // Get the Unity Axis Name for the Unity API
            string axisName = OuyaSDK.GetUnityAxisName(keyCode, player);

            // Check if the axis name is valid
            if (!string.IsNullOrEmpty(axisName))
            {
                //use the Unity API to get the axis value, raw or otherwise
                float axisValue = Input.GetAxisRaw(axisName);
                //check if the axis should be inverted
                if (OuyaSDK.GetAxisInverted(keyCode, player))
                {
                    return -axisValue;
                }
                else
                {
                    return axisValue;
                }
            }
        }
        // moving the common code into the sdk via above
        else
        {
            return OuyaExampleCommon.GetAxis(keyCode, player);
        }
        return 0f;
    }

    private bool GetButton(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
        // Check if we want the *new* SDK input or the example common
        if (m_useSDKForInput)
        {
            // Get the Unity KeyCode for the Unity API
            KeyCode unityKeyCode = OuyaSDK.GetUnityKeyCode(keyCode, player);

            // Check if the KeyCode is valid
            if (unityKeyCode != (KeyCode)(-1))
            {
                //use the Unity API to get the button value
                bool buttonState = Input.GetKey(unityKeyCode);
                return buttonState;
            }
        }
        // moving the common code into the sdk via aboveUs
        else
        {
            return OuyaExampleCommon.GetButton(keyCode, player);
        }
        return false;
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
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_LT, OuyaExampleCommon.Player))
        {
            UpdateHighlight(RendererLT, true, true);
        }
        else
        {
            UpdateHighlight(RendererLT, false, true);
        }

        //Check RT button for down state
        if (GetButton(OuyaSDK.KeyEnum.BUTTON_RT, OuyaExampleCommon.Player))
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
}