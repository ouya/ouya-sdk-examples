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
using tv.ouya.console.api;
using UnityEngine;
using Object=UnityEngine.Object;

public class OuyaPlotMeshThumbstick : MonoBehaviour
{
    public int PlayerNum;
    public int AxisX = 0;
    public int AxisY = 0;
    public MeshRenderer PlotMesh = null;
    private Material PlotMaterial = null;

    private int TextureSize = 128;

    private Texture2D m_texture = null;
    private Color32[] m_pixels = null;
    private float[] m_pixelVs = null;

    public Color32 BackgroundColor = Color.clear;

    private static Color m_plotBackground = new Color(0f, 0f, 0f, 0.5f);

    private static List<OuyaPlotMeshThumbstick> m_plots = new List<OuyaPlotMeshThumbstick>();

    private int m_lastX = -2;
    private int m_lastY = -2;

    public string m_label = null;

    private int m_pixelCount = 0;
    private int m_updatePixelCount = 0;
    private float m_pixelRatio = 0;

    private DateTime m_timerText = DateTime.MinValue;
    private DateTime m_timerTexture = DateTime.MinValue;

    public Vector2 m_labelPosition = Vector2.zero;

    public void OnEnable()
    {
        m_texture = new Texture2D(TextureSize, TextureSize, TextureFormat.ARGB32, false);
        m_texture.filterMode = FilterMode.Point;
        m_pixels = m_texture.GetPixels32();
        m_pixelVs = new float[m_pixels.Length];
        if (PlotMesh &&
            PlotMesh.material)
        {
            PlotMaterial = (Material)Object.Instantiate(PlotMesh.material);
            PlotMesh.material = PlotMaterial;
        }
        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = m_texture;
        }

        ClearTexture();

        m_plots.Add(this);
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

        m_plots.Remove(this);
    }

    void ClearTexture()
    {
        if (null != m_pixels)
        {
            for (int index = 0; index < m_pixels.Length; ++index)
            {
                m_pixels[index] = m_plotBackground;
                m_pixelVs[index] = -1;
            }
            m_texture.SetPixels32(m_pixels);
            m_texture.Apply();

            m_pixelCount = 0;
            m_updatePixelCount = 0;
            m_pixelRatio = 0;
        }
    }

    private void UpdateCounts()
    {
        m_pixelCount = 0;
        m_updatePixelCount = 0;

        for (int index = 0; index < m_pixelVs.Length; ++index)
        {
            if (m_pixelVs[index] == 0f)
            {
                ++m_pixelCount;
            }
            if (m_pixelVs[index] > 0f)
            {
                ++m_updatePixelCount;
            }
        }

        if (m_pixelCount == 0)
        {
            m_pixelRatio = 0f;
        }
        else
        {
            m_pixelRatio = m_updatePixelCount/(float) m_pixelCount;
        }
    }

    private float m_increment = 1/32f;

    void UpdateTexture()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // range -1 to 1
        float axisX = OuyaSDK.OuyaInput.GetAxis(PlayerNum, AxisX);
        float axisY = OuyaSDK.OuyaInput.GetAxis(PlayerNum, AxisY);

        // put in 0 to TextureSize range
        int x = (int)((axisX + 1) * 0.5f * (TextureSize - 1));
        int y = (int)((-axisY + 1) * 0.5f * (TextureSize - 1));

        int index = x + y * TextureSize;

        if (x != m_lastX ||
            y != m_lastY)
        {
            m_lastX = x;
            m_lastY = y;
            if (index >= 0 &&
                index < m_pixels.Length)
            {
                if (m_pixelVs[index] < 0f)
                {
                    m_pixelVs[index] = 0f;
                }
                else
                {
                    m_pixelVs[index] = m_pixelVs[index] + m_increment;
                }
                Vector3 c = Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(1, 1, 1), m_pixelVs[index]);
                m_pixels[index].r = (byte) (int) (c.x*255);
                m_pixels[index].g = (byte) (int) (c.y*255);
                m_pixels[index].b = (byte) (int) (c.z*255);
                m_pixels[index].a = 255;
            }
        }
#endif
    }

    public void Update()
    {
        if (null == m_texture)
        {
            return;
        }

        UpdateTexture();

        if (m_timerTexture < DateTime.Now)
        {
            m_timerTexture = DateTime.Now + TimeSpan.FromMilliseconds(100);
            m_texture.SetPixels32(m_pixels);
            m_texture.Apply();
        }

        if (m_timerText < DateTime.Now)
        {
            m_timerText = DateTime.Now + TimeSpan.FromMilliseconds(1000);
            
            UpdateCounts();
            m_label = string.Format("c={0} | u={1} | {2:F2}%", m_pixelCount, m_updatePixelCount, m_pixelRatio * 100);
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (OuyaSDK.OuyaInput.GetButtonDown(PlayerNum, OuyaController.BUTTON_A))
        {
            foreach (OuyaPlotMeshThumbstick plot in m_plots)
            {
                if (plot)
                {
                    plot.ClearTexture();
                }
            }
        }
#endif
    }

    void OnGUI()
    {
        if (string.IsNullOrEmpty(m_label) ||
            m_labelPosition == Vector2.zero)
        {
            return;
        }

        GUI.Label(new Rect(m_labelPosition.x, m_labelPosition.y, 200, 200), m_label);
    }
}
