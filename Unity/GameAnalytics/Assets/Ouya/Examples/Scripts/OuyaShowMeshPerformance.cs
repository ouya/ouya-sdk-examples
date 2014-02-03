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
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class OuyaShowMeshPerformance : MonoBehaviour,
                                       OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
                                       OuyaSDK.IMenuAppearingListener
{
    private DateTime m_timerChange = DateTime.MinValue;

    public UILabel RendererLabel = null;
    public UILabel PolysLabel = null;

    private List<MeshFilter> m_items = new List<MeshFilter>();

    public Material SharedMaterial = null;

    public MeshFilter PrefabCube = null;

    public List<Action> m_deferList = new List<Action>();

    private void Awake()
    {
        OuyaSDK.registerMenuButtonUpListener(this);
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
    }

    private void OnDestroy()
    {
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    private void Start()
    {
        Input.ResetInputAxes();
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

    #region Presentation

    private void OnGUI()
    {
#if UNITY_EDITOR
        if (GUILayout.Button("DPAD LEFT", GUILayout.MinHeight(40)))
        {
            DecreaseGeometry(1f);
        }

        if (GUILayout.Button("DPAD RIGHT", GUILayout.MinHeight(40)))
        {
            IncreaseGeometry(1f);
        }

        if (GUILayout.Button("DPAD UP", GUILayout.MinHeight(40)))
        {
            CombineGeometry();
        }

        if (GUILayout.Button("O BUTTON", GUILayout.MinHeight(40)))
        {
            IncreaseGeometry(0.1f);
        }

        if (GUILayout.Button("A BUTTON", GUILayout.MinHeight(40)))
        {
            DecreaseGeometry(0.1f);
        }
#endif
    }

    void Update()
    {
        if (m_deferList.Count > 0)
        {
            m_deferList[0].Invoke();
            m_deferList.RemoveAt(0);
        }
    }

    private void FixedUpdate()
    {
        if (RendererLabel)
        {
            RendererLabel.text = m_items.Count.ToString();
        }

        if (PolysLabel)
        {
            int count = 0;
            foreach (MeshFilter mf in m_items)
            {
                if (mf.mesh)
                {
                    count += mf.mesh.vertexCount;
                }
            }
            PolysLabel.text = count.ToString();
        }

        if (m_timerChange < DateTime.Now)
        {
            m_timerChange = DateTime.Now + TimeSpan.FromMilliseconds(200);

            if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, OuyaSDK.OuyaPlayer.player1))
            {
                DecreaseGeometry(1f);
            }

            if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, OuyaSDK.OuyaPlayer.player1))
            {
                IncreaseGeometry(1f);
            }

            if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_DPAD_UP, OuyaSDK.OuyaPlayer.player1))
            {
                CombineGeometry();
            }

            if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player1))
            {
                IncreaseGeometry(0.1f);
            }

            if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_A, OuyaSDK.OuyaPlayer.player1))
            {
                DecreaseGeometry(0.1f);
            }
        }

        float x = OuyaExampleCommon.GetAxis("LY", OuyaSDK.OuyaPlayer.player1);
        float y = OuyaExampleCommon.GetAxis("LX", OuyaSDK.OuyaPlayer.player1);
        Camera.main.transform.RotateAround(new Vector3(0, 0, 3), Camera.main.transform.rotation * Vector3.right, x * 45 * Time.fixedDeltaTime);
        Camera.main.transform.RotateAround(new Vector3(0, 0, 3), Camera.main.transform.rotation * Vector3.up, y * 45 * Time.fixedDeltaTime);
    }

    private void IncreaseGeometry(float size)
    {
        m_deferList.Clear();
        if (m_items.Count > 0)
        {
            int count = m_items.Count;
            for (int index = 0; index < count; ++index)
            {
                Action action = () =>
                                    {
                                        SpawnPolys(size);
                                    };
                m_deferList.Add(action);
            }
        }
        else
        {
            SpawnPolys(size);
        }
    }

    private void DecreaseGeometry(float size)
    {
        m_deferList.Clear();
        int count = m_items.Count/2;
        while (m_items.Count != 0 &&
            count < m_items.Count)
        {
            MeshFilter mf = m_items[m_items.Count - 1];
            m_items.Remove(mf);
            mf.mesh.Clear();
            Object.DestroyImmediate(mf.gameObject, true);
        }
    }

    private void CombineGeometry(int index)
    {
        if (index >= m_items.Count)
        {
            return;
        }
        int combineIndex = index + 1;
        if (combineIndex >= m_items.Count)
        {
            return;
        }
        
        MeshFilter mf = m_items[index];
        MeshFilter mf2 = m_items[combineIndex];
        if (mf.mesh &&
            mf2.mesh)
        {
            int trianglesCount = mf.mesh.triangles.Length + mf2.mesh.triangles.Length;
            int vertCount = mf.mesh.vertices.Length + mf2.mesh.vertices.Length;

            if (65000 < trianglesCount &&
                65000 < vertCount)
            {
                return;
            }

            CombineInstance[] combine =
            {
                new CombineInstance() { mesh = mf.mesh, transform = Matrix4x4.identity, subMeshIndex = 0},
                new CombineInstance() { mesh = mf2.mesh, transform = Matrix4x4.identity, subMeshIndex = 0},
            };

            Mesh newMesh = new Mesh();
            newMesh.CombineMeshes(combine);

            mf.mesh = newMesh;

            m_items.Remove(mf2);
            Object.DestroyImmediate(mf2.gameObject, true);
        }
    }

    private void CombineGeometry()
    {
        m_deferList.Clear();
        for (int index = 0; index < (m_items.Count / 2); ++index)
        {
            int indexCopy = index;
            Action action = () =>
                                {
                                    CombineGeometry(indexCopy);
                                };
            m_deferList.Add(action);
        }
    }

    private void SpawnPolys(float size)
    {
        Vector3 position = new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(-3f, 3f),
            Random.Range(5f, 1f));

        GameObject go = (GameObject)Object.Instantiate(PrefabCube.gameObject, Vector3.zero, Quaternion.identity);
        go.transform.localScale = Vector3.one;
        go.name = "polys";
        go.transform.parent = transform;
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        MeshFilter mf = go.GetComponent<MeshFilter>();
        m_items.Add(mf);

        //scale the verts
        Vector3[] verts = mf.mesh.vertices;
        for (int v = 0; v < mf.mesh.vertexCount; ++v)
        {
            verts[v] = position + verts[v] * size;
        }
        mf.mesh.vertices = verts;

        mr.sharedMaterial = SharedMaterial;
    }

    #endregion
}