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
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;
using Object=UnityEngine.Object;

[RequireComponent(typeof(Camera))]
public class OuyaPlotAxisButton : MonoBehaviour
{
    public int PlayerNum = 0;

    public Material PlotMaterial = null;

    private int TextureSize = 128;

    private Texture2D m_texture = null;
    private Color32[] m_pixels = null;

    private int m_timeIndex = 0;

    private List<int> m_plotAxises = new List<int>();
    private List<int> m_plotButtons = new List<int>();

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

#if UNITY_ANDROID && !UNITY_EDITOR
        m_plotAxises.Add(OuyaController.AXIS_LS_X);
        m_plotAxises.Add(OuyaController.AXIS_LS_Y);
        m_plotAxises.Add(OuyaController.AXIS_RS_X);
        m_plotAxises.Add(OuyaController.AXIS_RS_Y);
        m_plotAxises.Add(OuyaController.AXIS_L2);
        m_plotAxises.Add(OuyaController.AXIS_R2);

        m_plotButtons.Add(OuyaController.BUTTON_O);
        m_plotButtons.Add(OuyaController.BUTTON_U);
        m_plotButtons.Add(OuyaController.BUTTON_Y);
        m_plotButtons.Add(OuyaController.BUTTON_A);
        m_plotButtons.Add(OuyaController.BUTTON_L1);
        m_plotButtons.Add(OuyaController.BUTTON_R1);
        m_plotButtons.Add(OuyaController.BUTTON_L3);
        m_plotButtons.Add(OuyaController.BUTTON_R3);
        m_plotButtons.Add(OuyaController.BUTTON_DPAD_UP);
        m_plotButtons.Add(OuyaController.BUTTON_DPAD_DOWN);
        m_plotButtons.Add(OuyaController.BUTTON_DPAD_RIGHT);
        m_plotButtons.Add(OuyaController.BUTTON_DPAD_LEFT);
        m_plotButtons.Add(OuyaController.BUTTON_MENU);
#endif
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

    void UpdateTexture()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        int offset = m_timeIndex*TextureSize;
        int i = 0;
        foreach (int axis in m_plotAxises)
        {
            int index = i + offset;
            float ratio = i / (float)TextureSize;
            if (Mathf.Abs(OuyaSDK.OuyaInput.GetAxis(PlayerNum, axis)) > 0.25f)
            {
                PrintPixel(index, new Color32(255, (byte)(ratio * 255), 0, 255));
            }
            else
            {
                PrintPixel(index, new Color32(0, 0, 0, 0));
            }
            i += 4;
        }
        foreach (int button in m_plotButtons)
        {
            int index = i + offset;
            float ratio = i / (float)TextureSize;
            if (OuyaSDK.OuyaInput.GetButtonDown(PlayerNum, button) ||
                OuyaSDK.OuyaInput.GetButtonUp(PlayerNum, button))
            {
                PrintPixel(index, new Color32(0, (byte)(ratio * 255), 255, 255));
            }
            else
            {
                PrintPixel(index, new Color32(0, 0, 0, 0));
            }
            i += 4;
        }
#endif
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
            UpdateTexture();
            m_timeIndex = (m_timeIndex + 1)%TextureSize;
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