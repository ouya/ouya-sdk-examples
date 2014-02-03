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

public class OuyaShowCamera : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
{
    private WebCamTexture m_webTexture = null;
    private string m_webDevice = string.Empty;

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

        DestroyWebCam();
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

    private void UpdateWebCam()
    {
        DestroyWebCam();
        if (!string.IsNullOrEmpty(m_webDevice))
        {
            Debug.Log(string.Format("Creating Web Camera for: {0}", m_webDevice));
            m_webTexture = new WebCamTexture(m_webDevice, 640, 480, 30);
            m_webTexture.Play();
        }
    }

    private void DestroyWebCam()
    {
        if (null != m_webTexture)
        {
            m_webTexture.Stop();
            Object.DestroyImmediate(m_webTexture, true);
            m_webTexture = null;
        }
    }

    #region Presentation

    private void OnGUI()
    {
        try
        {
            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            WebCamDevice[] devices = WebCamTexture.devices;

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Cameras: {0}", devices.Length));
            GUILayout.EndHorizontal();

            foreach (WebCamDevice device in devices)
            {
                GUILayout.Label(string.Empty, GUILayout.MinHeight(10));
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);
                if (GUILayout.Button(device.name, GUILayout.MinHeight(40)))
                {
                    m_webDevice = device.name;
                    UpdateWebCam();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Label(string.Empty, GUILayout.MinHeight(20));

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(m_webTexture);
            GUILayout.EndHorizontal();
        }
        catch (System.Exception)
        {
        }
    }

    #endregion
}