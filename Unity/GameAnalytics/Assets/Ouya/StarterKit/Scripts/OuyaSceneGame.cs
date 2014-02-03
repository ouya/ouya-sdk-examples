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

public class OuyaSceneGame : MonoBehaviour
{
    public string NextScene = "SceneMain";
    public string SplashScene = "SceneSplash";

    public void OnGUI()
    {
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Back to Main Scene", GUILayout.MinHeight(60)))
        {
            Application.LoadLevel(NextScene);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label(string.Empty);
        GUILayout.Label(string.Empty);

        GUILayout.BeginHorizontal();
        GUILayout.Space(400);
        if (GUILayout.Button("Reset to Splash Screen", GUILayout.MinHeight(60)))
        {
            Application.LoadLevel(SplashScene);
        }
        GUILayout.EndHorizontal();
    }
}