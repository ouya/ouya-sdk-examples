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

using System.Collections.Generic;
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;

public class OuyaSceneGame : MonoBehaviour
{
    public string NextScene = "SceneMain";
    public string SplashScene = "SceneSplash";

#if UNITY_ANDROID && !UNITY_EDITOR

    /// <summary>
    /// Handle focusing items
    /// </summary>
    private FocusManager m_focusManager = new FocusManager();

    /// <summary>
    /// Buttons
    /// </summary>
    private object m_btnMainMenu = new object();
    private object m_btnSplashScreen = new object();

    public void OnGUI()
    {
        Color oldColor = GUI.backgroundColor;

        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        if (m_focusManager.SelectedButton == m_btnMainMenu)
        {
            GUI.backgroundColor = Color.red;
        }
        if (GUILayout.Button("Back to Main Scene", GUILayout.Height(60)) ||
            (m_focusManager.SelectedButton == m_btnMainMenu &&
            GetButtonUp(OuyaController.BUTTON_O)))
        {
            Application.LoadLevel(NextScene);
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        if (m_focusManager.SelectedButton == m_btnSplashScreen)
        {
            GUI.backgroundColor = Color.red;
        }
        if (GUILayout.Button("Reset to Splash Screen", GUILayout.Height(60)) ||
            (m_focusManager.SelectedButton == m_btnSplashScreen &&
            GetButtonUp(OuyaController.BUTTON_O)))
        {
            Application.LoadLevel(SplashScene);
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    public bool GetButtonUp(int button)
    {
        for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index)
        {
            if (OuyaSDK.OuyaInput.GetButtonUp(index, button))
            {
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        m_focusManager.Mappings[m_btnMainMenu] = new FocusManager.ButtonMapping()
        {
            Down = m_btnSplashScreen
        };
        m_focusManager.Mappings[m_btnSplashScreen] = new FocusManager.ButtonMapping()
        {
            Up = m_btnMainMenu
        };

        // set default selection
        m_focusManager.SelectedButton = m_btnMainMenu;
    }

    private void Update()
    {
        if (GetButtonUp(OuyaController.BUTTON_DPAD_DOWN))
        {
            m_focusManager.FocusDown();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_UP))
        {
            m_focusManager.FocusUp();
        }
    }

    public class FocusManager
    {
        private const int DELAY_MS = 150;

        public object SelectedButton = null;

        private void SetSelection(object selection)
        {
            if (null != selection)
            {
                SelectedButton = selection;
            }
        }

        public class ButtonMapping
        {
            public object Up = null;
            public object Down = null;
        }

        public Dictionary<object, ButtonMapping> Mappings = new Dictionary<object, ButtonMapping>();

        public void FocusDown()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Down);
            }
        }
        public void FocusUp()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Up);
            }
        }
    }
#endif
}