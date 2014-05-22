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
using System.Threading;
using UnityEngine;

public class OuyaThreadExample : MonoBehaviour
{
    /// <summary>
    /// All threads exit when false
    /// </summary>
    private bool m_keepWorking = true;

    /// <summary>
    /// Controller manipulates the thread count
    /// </summary>
    private int m_threadCount = 1;

    private List<Thread> m_threads = new List<Thread>();

    void OnEnable()
    {
        m_keepWorking = true;
    }

    void OnDestroy()
    {
        m_keepWorking = false;
    }

    void KillAllThreads()
    {
        
    }

    public class ThreadWrapper
    {
        public Thread m_thread = null;
    }

    void SpawnThreads()
    {
        ThreadWrapper tw = new ThreadWrapper();
        ParameterizedThreadStart ps = new ParameterizedThreadStart(ThreadWorker);
        tw.m_thread = new Thread(ps);
        tw.m_thread.Start(tw);
    }

    void RespawnThreads()
    {
        KillAllThreads();
        SpawnThreads();
    }

    public void ThreadWorker(object obj)
    {
        Thread self = (Thread) obj;

        m_threads.Add(self);

        int a = 0;
        int b = 1;
        int c = 2;

        while (m_keepWorking)
        {
            // super good performance here,
            // certified by lead game dev
            c = a + b;
            if (c == a)
            {
                c = b + a;
                if (c == 3)
                {
                    Thread.Sleep(0);
                }
            }
            else if (c == b)
            {
                c = a + b;
                if (c == 0)
                {
                    Thread.Sleep(0);
                }
            }
            Thread.Sleep(0);
        }

        m_threads.Remove(self);
    }

    void OnGUI()
    {
        if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player1))
        {
            ++m_threadCount;
            RespawnThreads();
        }

        if (OuyaExampleCommon.GetButton(OuyaSDK.KeyEnum.BUTTON_A, OuyaSDK.OuyaPlayer.player1))
        {
            m_threadCount = Mathf.Max(0, m_threadCount - 1);
            RespawnThreads();
        }
    }
}