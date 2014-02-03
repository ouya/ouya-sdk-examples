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
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OuyaNGUILabelRenderFPS : MonoBehaviour
{
    public UILabel Label = null;
    public bool ExecuteInEditMode = false;

    private DateTime m_timer = DateTime.MinValue;

    private Stopwatch m_stopWatch = new Stopwatch();

    private float m_renderLatency = 0f;

    public void OnPreRender()
    {
        m_stopWatch.Reset();
        m_stopWatch.Start();
    }

    public void OnPostRender()
    {
        if (!ExecuteInEditMode)
        {
            if (!Application.isPlaying)
            {
                return;
            }
        }

        m_stopWatch.Stop();
        Double elapsed = m_stopWatch.Elapsed.TotalMilliseconds;
        if (elapsed != 0f)
        {
            if (m_timer < DateTime.Now)
            {
                m_timer = DateTime.Now + TimeSpan.FromMilliseconds(500);

                m_renderLatency = (float) elapsed;

                if (Label)
                {
                    Label.text = string.Format("{0:F2}",
                        m_renderLatency);
                }
            }
        }
    }
}