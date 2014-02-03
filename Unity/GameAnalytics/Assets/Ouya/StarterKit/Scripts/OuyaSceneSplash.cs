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

public class OuyaSceneSplash : MonoBehaviour
{
    public string NextScene = "SceneMain";

    public Material SplashMaterial = null;

    public int TransitionIn = 1500;
    public int TransitionStay = 2000;
    public int TransitionOut = 1500;
    public float Alpha = 0f;

    private DateTime m_timerIn = DateTime.MinValue;
    private DateTime m_timerStay = DateTime.MinValue;
    private DateTime m_timerOut = DateTime.MinValue;

    public void Start()
    {
        m_timerIn = DateTime.Now + TimeSpan.FromMilliseconds(TransitionIn);
        m_timerStay = m_timerIn + TimeSpan.FromMilliseconds(TransitionStay);
        m_timerOut = m_timerStay + TimeSpan.FromMilliseconds(TransitionOut);
    }

    public void OnEnable()
    {
        SplashMaterial.SetColor("_Color", new Color(1, 1, 1, 0));
    }

    public void OnDestroy()
    {
        SplashMaterial.SetColor("_Color", new Color(1, 1, 1, 0));
    }

    public void Update()
    {
        if (m_timerIn == DateTime.MinValue ||
            m_timerStay == DateTime.MinValue ||
            m_timerOut == DateTime.MinValue ||
            null == SplashMaterial)
        {
            return;
        }

        if (m_timerOut < DateTime.Now)
        {
            Application.LoadLevel(NextScene);
            return;
        }

        if (m_timerStay < DateTime.Now)
        {
            int elapsed = (int)(m_timerOut - DateTime.Now).TotalMilliseconds;
            Alpha = Mathf.InverseLerp(0, TransitionOut, elapsed);
        }
        else if (m_timerIn < DateTime.Now)
        {
            Alpha = 1f;
        }
        else
        {
            int elapsed = (int)(m_timerIn - DateTime.Now).TotalMilliseconds;
            Alpha = Mathf.InverseLerp(TransitionIn, 0, elapsed);
        }

        SplashMaterial.SetColor("_Color", new Color(1, 1, 1, Alpha));
        //Debug.Log(Alpha.ToString());
    }
}