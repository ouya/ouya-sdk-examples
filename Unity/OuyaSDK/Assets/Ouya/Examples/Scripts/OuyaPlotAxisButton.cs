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

using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;

[RequireComponent(typeof(Camera))]
public class OuyaPlotAxisButton : MonoBehaviour
{
    public Material PlotMaterial = null;

    private int TextureSize = 128;

    private Texture2D m_texture = null;
    private Color32[] m_pixels = null;

    private int m_timeIndex = 0;

    private string[] m_strAxises = new string[]
                                       {
                                           "Joy{0} Axis 1",
                                           "Joy{0} Axis 2",
                                           "Joy{0} Axis 3",
                                           "Joy{0} Axis 4",
                                           "Joy{0} Axis 5",
                                           "Joy{0} Axis 6",
                                           "Joy{0} Axis 7",
                                           "Joy{0} Axis 8",
                                           "Joy{0} Axis 9",
                                           "Joy{0} Axis 10",
                                       };

    private string[] m_strButtons = new string[]
                                       {
                                           "Joystick{0}Button0",
                                           "Joystick{0}Button1",
                                           "Joystick{0}Button2",
                                           "Joystick{0}Button3",
                                           "Joystick{0}Button4",
                                           "Joystick{0}Button5",
                                           "Joystick{0}Button6",
                                           "Joystick{0}Button7",
                                           "Joystick{0}Button8",
                                           "Joystick{0}Button9",
                                           "Joystick{0}Button10",
                                           "Joystick{0}Button11",
                                           "Joystick{0}Button12",
                                           "Joystick{0}Button13",
                                           "Joystick{0}Button14",
                                           "Joystick{0}Button15",
                                           "Joystick{0}Button16",
                                           "Joystick{0}Button17",
                                           "Joystick{0}Button18",
                                           "Joystick{0}Button19",
                                       };

    private Dictionary<string, bool> m_inputMap = new Dictionary<string, bool>();

    public void OnEnable()
    {
        m_texture = new Texture2D(TextureSize, TextureSize, TextureFormat.ARGB32, false);
        m_texture.filterMode = FilterMode.Point;
        m_pixels = m_texture.GetPixels32();
        Color32 tansparent = new Color32(0, 0, 0, 0);
        for (int i = 0; i < m_pixels.Length; ++i)
        {
            m_pixels[i] = tansparent;
        }
        m_texture.SetPixels32(m_pixels);
        m_texture.Apply();

        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = m_texture;
        }

        ClearInputMap();
    }

    public void OnDisable()
    {
        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = null;
            PlotMaterial.mainTextureOffset = Vector2.zero;
        }

        if (null != m_texture)
        {
            Object.DestroyImmediate(m_texture, true);
            m_texture = null;
        }
    }

    void ClearInputMap()
    {
        for (int player = 1; player <= 8; ++player)
        {
            foreach (string s in m_strAxises)
            {
                string axisName = string.Format(s, player);
                m_inputMap[axisName] = false;
            }
            foreach (string s in m_strButtons)
            {
                string buttonName = string.Format(s, player);
                m_inputMap[buttonName] = false;
            }
        }
    }

    void CheckForInput()
    {
        foreach (string s in m_strAxises)
        {
            string axisName = string.Format(s, (int)OuyaExampleCommon.Player);
            if (Input.GetAxis(axisName) != 0f)
            {
                m_inputMap[axisName] = true;
            }
        }
        foreach (string s in m_strButtons)
        {
            string buttonName = string.Format(s, (int)OuyaExampleCommon.Player);
            KeyCode key = (KeyCode)(OuyaKeyCode)Enum.Parse(typeof(OuyaKeyCode), buttonName);
            if (Input.GetKey(key))
            {
                m_inputMap[buttonName] = true;
            }
        }
    }

    void UpdateTexture()
    {
        int offset = m_timeIndex*TextureSize;
        int i = 0;
        foreach (string s in m_strAxises)
        {
            int index = i + offset;
            float ratio = i / (float)TextureSize;
            string axisName = string.Format(s, (int)OuyaExampleCommon.Player);
            if (m_inputMap[axisName])
            {
                PrintPixel(index, new Color32(255, (byte)(ratio * 255), 0, 255));
            }
            else
            {
                PrintPixel(index, new Color32(0, 0, 0, 0));
            }
            i += 4;
        }
        foreach (string s in m_strButtons)
        {
            int index = i + offset;
            float ratio = i / (float)TextureSize;
            string buttonName = string.Format(s, (int)OuyaExampleCommon.Player);
            if (m_inputMap[buttonName])
            {
                PrintPixel(index, new Color32(0, (byte)(ratio * 255), 255, 255));
            }
            else
            {
                PrintPixel(index, new Color32(0, 0, 0, 0));
            }
            i += 4;
        }
    }

    void PrintPixel(int index, Color32 color)
    {
        m_pixels[index] = color;
        m_pixels[index+1] = color;
        m_pixels[index+2] = color;
        m_pixels[index+3] = color;
    }

    private DateTime m_inputTimer = DateTime.MinValue;

    public void Update()
    {
        if (m_inputTimer < DateTime.Now)
        {
            m_inputTimer = DateTime.Now + TimeSpan.FromMilliseconds(200);
            CheckForInput();

            UpdateTexture();
            m_timeIndex = (m_timeIndex + 1)%TextureSize;

            ClearInputMap();
        }

        ApplyTexture();
    }

    private DateTime m_renderTimer = DateTime.MinValue;

    public void ApplyTexture()
    {
        if (m_renderTimer < DateTime.Now)
        {
            m_renderTimer = DateTime.Now + TimeSpan.FromMilliseconds(100);
            if (m_texture)
            {
                m_texture.SetPixels32(m_pixels);
                m_texture.Apply();
            }

            if (PlotMaterial)
            {
                PlotMaterial.mainTextureOffset = new Vector2(0, m_timeIndex/(float) TextureSize);
            }
        }
    }
}