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
using UnityEngine;
using Object=UnityEngine.Object;

[RequireComponent(typeof(Camera))]
public class OuyaPlotFPS : MonoBehaviour
{
    public Material PlotMaterial = null;

    private int m_updateLastCount = 0;
    private int m_updateCount = 0;
    private DateTime m_updateTimer = DateTime.MinValue;

    private int TextureSize = 128;

    private Texture2D m_texture = null;
    private Color32[] m_pixels = null;

    private int m_timeIndex = 0;

    public Color32 BackgroundColor = Color.clear;

    public void OnEnable()
    {
        m_texture = new Texture2D(TextureSize, TextureSize, TextureFormat.ARGB32, false);
        m_texture.filterMode = FilterMode.Point;
        m_pixels = m_texture.GetPixels32();
        if (PlotMaterial)
        {
            PlotMaterial.mainTexture = m_texture;
        }
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
        int fps30Index = TextureSize - (int)(30 * TextureSize / 100f);
        int fps60Index = TextureSize - (int)(60 * TextureSize / 100f);
        int fpsIndex = (int)(m_updateLastCount*TextureSize/100f);
        int offset = m_timeIndex*TextureSize;
        for (int i = 0; i < TextureSize; ++i)
        {
            float ratio = i/(float) TextureSize;
            int index = i + offset;

            if (i == fps30Index)
            {
                m_pixels[index] = new Color32(255, 0, 0, 255);
            }
            else if (i == fps60Index)
            {
                m_pixels[index] = new Color32(0, 255, 0, 255);
            }
            else if (i < (TextureSize - fpsIndex))
            {
                m_pixels[index] = BackgroundColor;
            }
            else
            {
                m_pixels[index] = new Color32(255, (byte)(ratio * 255), 0, 255);
            }
        }
    }

    public void OnPostRender()
    {
        if (null == m_texture)
        {
            return;
        }
        
        UpdateTexture();
        m_timeIndex = (m_timeIndex + 1)%TextureSize;
    }

    void FixedUpdate()
    {
        m_texture.SetPixels32(m_pixels);
        m_texture.Apply();

        if (PlotMaterial)
        {
            PlotMaterial.mainTextureOffset = new Vector2(0, m_timeIndex / (float)TextureSize);
        }
    }

    private void LateUpdate()
    {
        ++m_updateCount;
        if (m_updateTimer < DateTime.Now)
        {
            m_updateLastCount = m_updateCount;
            m_updateCount = 0;
            m_updateTimer = DateTime.Now + TimeSpan.FromSeconds(1);
        }
    }
}