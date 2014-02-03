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
using UnityEngine;

public class OuyaShowSounds : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{
    public AudioClip m_soundMP3 = null;
    public AudioClip m_soundOGG = null;
    public AudioClip m_soundWAV = null;

    void Awake()
    {
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
    }
    void OnDestroy()
    {
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    public void OuyaMenuButtonUp()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
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

    private void OnGUI()
    {
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Play MP3", GUILayout.MinHeight(40)))
        {
            audio.PlayOneShot(m_soundMP3, 100);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Play OGG", GUILayout.MinHeight(40)))
        {
            audio.PlayOneShot(m_soundOGG, 100);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Play Wave", GUILayout.MinHeight(40)))
        {
            audio.PlayOneShot(m_soundWAV, 100);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(string.Empty);
    }
}