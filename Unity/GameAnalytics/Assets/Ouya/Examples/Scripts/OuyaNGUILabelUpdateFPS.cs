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

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OuyaNGUILabelUpdateFPS : MonoBehaviour
{
    public UILabel Label = null;
    public bool ExecuteInEditMode = false;

    private int m_lastCount = 0;
    private int m_count = 0;
    private DateTime m_timer = DateTime.MinValue;

    public void OnPostRender()
    {
        if (!ExecuteInEditMode)
        {
            if (!Application.isPlaying)
            {
                return;
            }
        }

        ++m_count;
        if (m_timer < DateTime.Now)
        {
            m_timer = DateTime.Now + TimeSpan.FromSeconds(1);
            m_lastCount = m_count;
            m_count = 0;

            if (Label)
            {
                Label.text = m_lastCount.ToString();
            }
        }
    }
}