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
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID && !UNITY_EDITOR
using org.json;
using System.Threading;
using tv.ouya.console.api.content;
using tv.ouya.sdk;
#endif
using UnityEngine;

public class OuyaGameObject : MonoBehaviour
{
    #region Public Visible Variables

    [Serializable]
    public class KeyValuePair
    {
        public string Key = string.Empty;
        public string Value = string.Empty;
    }

    public List<KeyValuePair> OuyaPluginInitValues = new List<KeyValuePair>() { new KeyValuePair() { Key = "tv.ouya.developer_id", Value = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0" } };

    public bool m_useInputThreading = false;

    #endregion

    #region Private Variables
    private static OuyaGameObject m_instance = null;
    #endregion

    #region Singleton Accessor Class
    /// <summary>
    /// Singleton interface
    /// </summary>
    public static OuyaGameObject Singleton
    {
        get
        {
            if (null == m_instance)
            {
                GameObject ouyaGameObject = GameObject.Find("OuyaGameObject");
                if (ouyaGameObject)
                {
                    m_instance = ouyaGameObject.GetComponent<OuyaGameObject>();
                }
            }
            return m_instance;
        }
    }
    #endregion 
     
    #region Java To Unity Event Handlers

#if UNITY_ANDROID && !UNITY_EDITOR

    public void onMenuAppearing(string ignore)
    {
        //Debug.Log("onMenuAppearing");
        foreach (OuyaSDK.IMenuAppearingListener listener in OuyaSDK.getMenuAppearingListeners())
        {
            listener.OuyaMenuAppearing();
        }
    }

    public void onPause(string ignore)
    {
        //Debug.Log("onPause");
        foreach (OuyaSDK.IPauseListener listener in OuyaSDK.getPauseListeners())
        {
            listener.OuyaOnPause();
        }
    }

    public void onResume(string ignore)
    {
        Debug.Log("onResume");
        foreach (OuyaSDK.IResumeListener listener in OuyaSDK.getResumeListeners())
        {
            listener.OuyaOnResume();
        }
    }

#endif

    #endregion

    #region Initialization Listeners

    IEnumerator InvokeInitOuyaPlugin(bool wait)
    {
        if (wait)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return null;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (JSONArray jsonArray = new JSONArray())
            {
                int index = 0;
                foreach (OuyaGameObject.KeyValuePair kvp in OuyaPluginInitValues)
                {
                    try
                    {
                        using (JSONObject jsonObject = new JSONObject())
                        {
                            //Debug.Log(string.Format("key={0} value={1}", kvp.Key, kvp.Value));
                            jsonObject.put("key", kvp.Key);
                            jsonObject.put("value", kvp.Value);
                            jsonArray.put(index, jsonObject);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    ++index;
                }
                
                string jsonData = jsonArray.toString();
                //Debug.Log("InvokeInitOuyaPlugin jsonData" + jsonData);

                OuyaSDK.initOuyaPlugin(jsonData);
            }
        }
        catch (Exception)
        {
            OnFailureInitializePlugin("Failed to invoke initOuyaPlugin.");
        }
#endif
    }

    public void OnSuccessInitializePlugin(string ignore)
    {
        Debug.Log("OUYA Plugin Initialized.");
    }

    public void OnFailureInitializePlugin(string errorMessage)
    {
        Debug.Log(string.Format("initOuyaPlugin failed: {0}", errorMessage));
        //reattempt
        StartCoroutine("InvokeInitOuyaPlugin", true);
    }

    #endregion

    #region JSON Data Listeners

#if UNITY_ANDROID && !UNITY_EDITOR

    public void ContentDeleteListenerOnDeleted(string ignore)
    {
        foreach (OuyaSDK.IContentDeleteListener listener in OuyaSDK.getContentDeleteListeners())
        {
            if (null != listener)
            {
                listener.ContentDeleteOnDeleted(null);
            }
        }
    }
    public void ContentDeleteListenerOnDeleteFailed(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentDeleteListener listener in OuyaSDK.getContentDeleteListeners())
            {
                if (null != listener)
                {
                    listener.ContentDeleteOnDeleteFailed(null, code, reason);
                }
            }
        }
    }

    public void ContentDownloadListenerOnComplete(string ignore)
    {
        foreach (OuyaSDK.IContentDownloadListener listener in OuyaSDK.getContentDownloadListeners())
        {
            if (null != listener)
            {
                listener.ContentDownloadOnComplete(null);
            }
        }
    }
    public void ContentDownloadListenerOnProgress(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int progress = 0;
            if (jsonObject.has("progress"))
            {
                progress = jsonObject.getInt("progress");
            }
            foreach (OuyaSDK.IContentDownloadListener listener in OuyaSDK.getContentDownloadListeners())
            {
                if (null != listener)
                {
                    listener.ContentDownloadOnProgress(null, progress);
                }
            }
        }
    }
    public void ContentDownloadListenerOnFailed(string ignore)
    {
        foreach (OuyaSDK.IContentDownloadListener listener in OuyaSDK.getContentDownloadListeners())
        {
            if (null != listener)
            {
                listener.ContentDownloadOnFailed(null);
            }
        }
    }

    public void ContentInitListenerOnInitialized(string ignore)
    {
        foreach (OuyaSDK.IContentInitializedListener listener in OuyaSDK.getContentInitializedListeners())
        {
            if (null != listener)
            {
                listener.ContentInitializedOnInitialized();
            }
        }
    }
    public void ContentInitListenerOnDestroyed(string ignore)
    {
        foreach (OuyaSDK.IContentInitializedListener listener in OuyaSDK.getContentInitializedListeners())
        {
            if (null != listener)
            {
                listener.ContentInitializedOnDestroyed();
            }
        }
    }

    public void ContentInstalledSearchListenerOnResults(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int count = 0;
            if (jsonObject.has("count"))
            {
                count = jsonObject.getInt("count");
            }
            List<OuyaMod> ouyaMods = OuyaUnityPlugin.getOuyaContentInstalledResults();
            foreach (OuyaSDK.IContentInstalledSearchListener listener in OuyaSDK.getContentInstalledSearchListeners())
            {
                if (null != listener)
                {
                    listener.ContentInstalledSearchOnResults(ouyaMods, count);
                }
            }
        }
    }
    public void ContentInstalledSearchListenerOnError(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentInstalledSearchListener listener in OuyaSDK.getContentInstalledSearchListeners())
            {
                if (null != listener)
                {
                    listener.ContentInstalledSearchOnError(code, reason);
                }
            }
        }
    }

    public void ContentPublishedSearchListenerOnResults(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int count = 0;
            if (jsonObject.has("count"))
            {
                count = jsonObject.getInt("count");
            }
            List<OuyaMod> ouyaMods = OuyaUnityPlugin.getOuyaContentPublishedResults();
            foreach (OuyaSDK.IContentPublishedSearchListener listener in OuyaSDK.getContentPublishedSearchListeners())
            {
                if (null != listener)
                {
                    listener.ContentPublishedSearchOnResults(ouyaMods, count);
                }
            }
        }
    }
    public void ContentPublishedSearchListenerOnError(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentPublishedSearchListener listener in OuyaSDK.getContentPublishedSearchListeners())
            {
                if (null != listener)
                {
                    listener.ContentPublishedSearchOnError(code, reason);
                }
            }
        }
    }

    public void ContentSaveListenerOnSuccess(string ignore)
    {
        foreach (OuyaSDK.IContentSaveListener listener in OuyaSDK.getContentSaveListeners())
        {
            if (null != listener)
            {
                listener.ContentSaveOnSuccess(null);
            }
        }
    }
    public void ContentSaveListenerOnError(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentSaveListener listener in OuyaSDK.getContentSaveListeners())
            {
                if (null != listener)
                {
                    listener.ContentSaveOnError(null, code, reason);
                }
            }
        }
    }

    public void ContentPublishListenerOnSuccess(string ignore)
    {
        foreach (OuyaSDK.IContentPublishListener listener in OuyaSDK.getContentPublishListeners())
        {
            if (null != listener)
            {
                listener.ContentPublishOnSuccess(null);
            }
        }
    }
    public void ContentPublishListenerOnError(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentPublishListener listener in OuyaSDK.getContentPublishListeners())
            {
                if (null != listener)
                {
                    listener.ContentPublishOnError(null, code, reason);
                }
            }
        }
    }

    public void ContentUnpublishListenerOnSuccess(string ignore)
    {
        foreach (OuyaSDK.IContentUnpublishListener listener in OuyaSDK.getContentUnpublishListeners())
        {
            if (null != listener)
            {
                listener.ContentUnpublishOnSuccess(null);
            }
        }
    }
    public void ContentUnpublishListenerOnError(string jsonData)
    {
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            int code = 0;
            string reason = string.Empty;
            if (jsonObject.has("code"))
            {
                code = jsonObject.getInt("code");
            }
            if (jsonObject.has("reason"))
            {
                reason = jsonObject.getString("reason");
            }
            foreach (OuyaSDK.IContentUnpublishListener listener in OuyaSDK.getContentUnpublishListeners())
            {
                if (null != listener)
                {
                    listener.ContentUnpublishOnError(null, code, reason);
                }
            }
        }
    }

    public void RequestGamerInfoSuccessListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestGamerInfo = false;
        //Debug.Log(string.Format("RequestGamerInfoSuccessListener: jsonData={0}", jsonData));
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            OuyaSDK.GamerInfo gamerInfo = OuyaSDK.GamerInfo.Parse(jsonObject);
            foreach (OuyaSDK.IRequestGamerInfoListener listener in OuyaSDK.getRequestGamerInfoListeners())
            {
                if (null != listener)
                {
                    listener.RequestGamerInfoOnSuccess(gamerInfo);
                }
            }
        }
    }
    public void RequestGamerInfoFailureListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestGamerInfo = false;
        //Debug.LogError(string.Format("RequestGamerInfoFailureListener: jsonData={0}", jsonData));
        foreach (OuyaSDK.IRequestGamerInfoListener listener in OuyaSDK.getRequestGamerInfoListeners())
        {
            if (null != listener)
            {
                listener.RequestGamerInfoOnFailure(0, jsonData);
            }
        }
    }
    public void RequestGamerInfoCancelListener(string ignore)
    {
        OuyaUnityPlugin.m_pendingRequestGamerInfo = false;
        foreach (OuyaSDK.IRequestGamerInfoListener listener in OuyaSDK.getRequestGamerInfoListeners())
        {
            if (null != listener)
            {
                listener.RequestGamerInfoOnCancel();
            }
        }
    }

    public void RequestProductsSuccessListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestProducts = false;

        //Debug.Log(string.Format("OuyaSDK.RequestProductsSuccessListener: jsonData={0}", jsonData));

        using (JSONArray jsonArray = new JSONArray(jsonData))
        {
            List<OuyaSDK.Product> products = new List<OuyaSDK.Product>();
            for (int index = 0; index < jsonArray.length(); ++index)
            {
                using (JSONObject jsonObject = jsonArray.getJSONObject(index))
                {
                    //Debug.Log(string.Format("Found Product: {0}", jsonObject.toString()));
                    OuyaSDK.Product product = OuyaSDK.Product.Parse(jsonObject);
                    products.Add(product);
                }
            }
            foreach (OuyaSDK.IRequestProductsListener listener in OuyaSDK.getRequestProductsListeners())
            {
                if (null != listener)
                {
                    listener.RequestProductsOnSuccess(products);
                }
            }
        }
    }
    public void RequestProductsFailureListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestProducts = false;
        //Debug.LogError(string.Format("RequestProductsFailureListener: jsonData={0}", jsonData));
        foreach (OuyaSDK.IRequestProductsListener listener in OuyaSDK.getRequestProductsListeners())
        {
            if (null != listener)
            {
                listener.RequestProductsOnFailure(0, jsonData);
            }
        }
    }

    public void RequestPurchaseSuccessListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestPurchase = false;
        //Debug.Log(string.Format("RequestPurchaseSuccessListener: jsonData={0}", jsonData));
        using (JSONObject jsonObject = new JSONObject(jsonData))
        {
            OuyaSDK.Product product = OuyaSDK.Product.Parse(jsonObject);
            foreach (OuyaSDK.IRequestPurchaseListener listener in OuyaSDK.getRequestPurchaseListeners())
            {
                if (null != listener)
                {
                    listener.RequestPurchaseOnSuccess(product);
                }
            }
        }
    }
    public void RequestPurchaseFailureListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestPurchase = false;
        //Debug.LogError(string.Format("RequestPurchaseFailureListener: jsonData={0}", jsonData));
        foreach (OuyaSDK.IRequestPurchaseListener listener in OuyaSDK.getRequestPurchaseListeners())
        {
            if (null != listener)
            {
                listener.RequestPurchaseOnFailure(0, jsonData);
            }
        }
    }
    public void RequestPurchaseCancelListener(string ignore)
    {
        OuyaUnityPlugin.m_pendingRequestPurchase = false;
        foreach (OuyaSDK.IRequestPurchaseListener listener in OuyaSDK.getRequestPurchaseListeners())
        {
            if (null != listener)
            {
                listener.RequestPurchaseOnCancel();
            }
        }
    }

    public void RequestReceiptsSuccessListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestReceipts = false;

        //Debug.Log(string.Format("OuyaSDK.RequestReceiptsSuccessListener: jsonData={0}", jsonData));

        using (JSONArray jsonArray = new JSONArray(jsonData))
        {
            List<OuyaSDK.Receipt> receipts = new List<OuyaSDK.Receipt>();
            for (int index = 0; index < jsonArray.length(); ++index)
            {
                using (JSONObject jsonObject = jsonArray.getJSONObject(index))
                {
                    //Debug.Log(string.Format("Found Receipt: {0}", jsonObject.toString()));
                    OuyaSDK.Receipt receipt = OuyaSDK.Receipt.Parse(jsonObject);
                    receipts.Add(receipt);
                }
            }
            foreach (OuyaSDK.IRequestReceiptsListener listener in OuyaSDK.getRequestReceiptsListeners())
            {
                if (null != listener)
                {
                    listener.RequestReceiptsOnSuccess(receipts);
                }
            }
        }
    }
    public void RequestReceiptsFailureListener(string jsonData)
    {
        OuyaUnityPlugin.m_pendingRequestReceipts = false;
        Debug.LogError(string.Format("RequestReceiptsFailureListener: jsonData={0}", jsonData));
        foreach (OuyaSDK.IRequestReceiptsListener listener in OuyaSDK.getRequestReceiptsListeners())
        {
            if (null != listener)
            {
                listener.RequestReceiptsOnFailure(0, jsonData);
            }
        }
    }
    public void RequestReceiptsCancelListener(string ignore)
    {
        OuyaUnityPlugin.m_pendingRequestReceipts = false;
        foreach (OuyaSDK.IRequestReceiptsListener listener in OuyaSDK.getRequestReceiptsListeners())
        {
            if (null != listener)
            {
                listener.RequestReceiptsOnCancel();
            }
        }
    }

#endif
    #endregion

    #region UNITY Awake, Start & Update
    void Awake()
    {
        m_instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log(string.Format("OuyaPluginVersion: VERSION={0}", OuyaSDK.PLUGIN_VERSION));
#endif
    }
    void Start()
    {
        Input.ResetInputAxes();
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(transform.gameObject);

        StartCoroutine("InvokeInitOuyaPlugin", false);

        #region Init Input

#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_useInputThreading)
        {
            ThreadStart ts = new ThreadStart(InputWorker);
            Thread thread = new Thread(ts);
            thread.Start();
        }
#endif

        #endregion
    }
    #endregion

    #region Controllers

#if UNITY_ANDROID && !UNITY_EDITOR
    private bool m_waitForExit = true;
    void OnDestroy()
    {
        m_waitForExit = false;
    }
    void OnApplicationQuit()
    {
        m_waitForExit = false;
    }
    private bool m_clearFrame = true;
    public void Update()
    {
        if (m_useInputThreading)
        {
            m_clearFrame = true;   
        }
        else
        {
            OuyaSDK.OuyaInput.UpdateInputFrame();
            OuyaSDK.OuyaInput.ClearButtonStates();
        }
    }
    private void InputWorker()
    {
        while (m_waitForExit)
        {
            OuyaSDK.OuyaInput.UpdateInputFrame();
            if (m_clearFrame)
            {
                m_clearFrame = false;
                OuyaSDK.OuyaInput.ClearButtonStates();
            }
            Thread.Sleep(1);
        }
    }
#endif

    private void FixedUpdate()
    {
        OuyaSDK.UpdateJoysticks();
    }

    #endregion

    #region Debug Logs from Java
    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public void DebugLogError(string message)
    {
        Debug.LogError(message);
    }
    #endregion

}