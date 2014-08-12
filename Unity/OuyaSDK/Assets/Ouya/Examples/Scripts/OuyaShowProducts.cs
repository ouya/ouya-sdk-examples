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
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;

public class OuyaShowProducts : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IFetchGamerInfoListener,
    OuyaSDK.IGetProductsListener, OuyaSDK.IPurchaseListener, OuyaSDK.IGetReceiptsListener,
    OuyaSDK.IMenuAppearingListener
{
    /// <summary>
    /// Handle focusing items
    /// </summary>
    private FocusManager m_focusManager = new FocusManager();

    /// <summary>
    /// The products to display for purchase
    /// </summary>
    public string[] Purchasables =
    {
        "long_sword",
        "sharp_axe",
        "__DECLINED__THIS_PURCHASE",
    };

    /// <summary>
    /// The gamer UUID
    /// </summary>
    private string m_gamerUUID = string.Empty;

    /// <summary>
    /// The gamer User Name
    /// </summary>
    private string m_gamerUserName = string.Empty;

    /// <summary>
    /// A key to store game data
    /// </summary>
    private const string KEY_PUT_GAME_DATA = "ShowProductsExample";

    /// <summary>
    /// The game data to display what was stored
    /// </summary>
    private string m_gameData = string.Empty;

    /// <summary>
    /// Buttons
    /// </summary>
    private object m_btnGetGamerInfo = new object();
    private object m_btnPutGameData = new object();
    private object m_btnGetGameData = new object();
    private object m_btnGetProducts = new object();
    private object m_btnGetReceipts = new object();

    void Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
        OuyaSDK.registerFetchGamerInfoListener(this);
        OuyaSDK.registerGetProductsListener(this);
        OuyaSDK.registerPurchaseListener(this);
        OuyaSDK.registerGetReceiptsListener(this);
    }
    void OnDestroy()
    {
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
        OuyaSDK.unregisterFetchGamerInfoListener(this);
        OuyaSDK.unregisterGetProductsListener(this);
        OuyaSDK.unregisterPurchaseListener(this);
        OuyaSDK.unregisterGetReceiptsListener(this);
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

    public void OuyaFetchGamerInfoOnSuccess(string gamerUUID, string gamerUserName)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        m_gamerUUID = gamerUUID;
        m_gamerUserName = gamerUserName;
    }

    public void OuyaFetchGamerInfoOnFailure(int errorCode, string errorMessage)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaFetchGamerInfoOnCancel()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public void OuyaGetProductsOnSuccess(List<OuyaSDK.Product> products)
    {
        m_products.Clear();
        for (int index = 0; index < products.Count; ++index)
        {
            OuyaSDK.Product product = products[index];
            // Get Products Right goes to the first element
            if (index == 0)
            {
                m_focusManager.Mappings[m_btnGetProducts].Right = product;
            }
            // Products left goes back to the GetProducts button
            m_focusManager.Mappings[product] = new FocusManager.ButtonMapping()
            {
                Left = m_btnGetProducts
            };
            // Product down goes to the next element
            if ((index + 1) < products.Count)
            {
                m_focusManager.Mappings[product].Down = products[index+1];
            }
            // Product up goes to the previous element
            if (index > 0)
            {
                m_focusManager.Mappings[product].Up = products[index - 1];
            }
            m_products.Add(product);
        }
    }

    public void OuyaGetProductsOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("OuyaGetProductsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
    }

    public void OuyaGetProductsOnCancel()
    {
        Debug.LogError("OuyaGetProductsOnCancel:");
    }

    public void OuyaPurchaseOnSuccess(OuyaSDK.Product product)
    {
        
    }

    public void OuyaPurchaseOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("OuyaPurchaseOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
    }

    public void OuyaPurchaseOnCancel()
    {
        Debug.LogError("OuyaPurchaseOnCancel:");
    }

    public void OuyaGetReceiptsOnSuccess(List<OuyaSDK.Receipt> receipts)
    {
        m_receipts.Clear();
        foreach (OuyaSDK.Receipt receipt in receipts)
        {
            m_receipts.Add(receipt);
        }
    }

    public void OuyaGetReceiptsOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("OuyaGetReceiptsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
    }

    public void OuyaGetReceiptsOnCancel()
    {
        Debug.LogError("OuyaGetReceiptsOnCancel:");
    }

    #region Data containers

    private List<OuyaSDK.Product> m_products = new List<OuyaSDK.Product>();

    private List<OuyaSDK.Receipt> m_receipts = new List<OuyaSDK.Receipt>();

    #endregion

    #region Presentation

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnGUI()
    {
        try
        {
            Color oldColor = GUI.backgroundColor;

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(OuyaSDK.isIAPInitComplete() ? "IAP is initialized" : "IAP initializing...");
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Gamer UUID: {0}", m_gamerUUID));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Gamer User Name: {0}", m_gamerUserName));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnGetGamerInfo)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Get Gamer Info", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnGetGamerInfo &&
                OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
            {
                OuyaSDK.fetchGamerInfo();
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnPutGameData)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Put Game Data", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnPutGameData &&
                OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
            {
                OuyaSDK.putGameData(KEY_PUT_GAME_DATA, "This is a test!!!!");
            }
            GUI.backgroundColor = oldColor;

            if (m_focusManager.SelectedButton == m_btnGetGameData)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Get Game Data", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnGetGameData &&
                OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
            {
                m_gameData = OuyaSDK.getGameData(KEY_PUT_GAME_DATA);
            }
            GUI.backgroundColor = oldColor;
            GUILayout.Label(string.Format("GameData: {0}", m_gameData));
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label("Products:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnGetProducts)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Get Products", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnGetProducts &&
                OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
            {
                List<OuyaSDK.Purchasable> productIdentifierList =
                    new List<OuyaSDK.Purchasable>();

                foreach (string productId in Purchasables)
                {
                    OuyaSDK.Purchasable purchasable = new OuyaSDK.Purchasable();
                    purchasable.productId = productId;
                    productIdentifierList.Add(purchasable);
                }

                OuyaSDK.requestProductList(productIdentifierList);
            }
            GUI.backgroundColor = oldColor;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            foreach (OuyaSDK.Product product in m_products)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);

                GUILayout.Label(string.Format("Name={0}", product.name));
                GUILayout.Label(string.Format("Price={0}", product.localPrice));
                GUILayout.Label(string.Format("Identifier={0}", product.identifier));

                if (m_focusManager.SelectedButton == product)
                {
                    GUI.backgroundColor = Color.red;
                }
                if (GUILayout.Button("Purchase") ||
                    (m_focusManager.SelectedButton == product &&
                    OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
                {
                    Debug.Log(string.Format("Purchase Identifier: {0}", product.identifier));
                    OuyaSDK.Purchasable purchasable = new OuyaSDK.Purchasable();
                    purchasable.productId = product.identifier;
                    OuyaSDK.requestPurchase(purchasable);
                }
                GUI.backgroundColor = oldColor;

                GUILayout.EndHorizontal();
            }

            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label("Receipts:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnGetReceipts)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Get Receipts", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnGetReceipts &&
                OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O)))
            {
                OuyaSDK.requestReceiptList();
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();

            foreach (OuyaSDK.Receipt receipt in m_receipts)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);

                GUILayout.Label(string.Format("Price={0}", receipt.priceInCents));
                GUILayout.Label(string.Format("Identifier={0}", receipt.identifier));

                GUILayout.EndHorizontal();
            }
        }
        catch (System.Exception)
        {
        }
    }
#endif

    #endregion

    #region Focus Handling

    void Start()
    {
        m_focusManager.Mappings[m_btnGetGamerInfo] = new FocusManager.ButtonMapping()
        {
            Down = m_btnPutGameData
        };
        m_focusManager.Mappings[m_btnPutGameData] = new FocusManager.ButtonMapping()
        {
            Up = m_btnGetGamerInfo,
            Right = m_btnGetGameData,
            Down = m_btnGetProducts
        };
        m_focusManager.Mappings[m_btnGetGameData] = new FocusManager.ButtonMapping()
        {
            Up = m_btnGetGamerInfo,
            Left = m_btnPutGameData,
            Down = m_btnGetProducts
        };
        m_focusManager.Mappings[m_btnGetProducts] = new FocusManager.ButtonMapping()
        {
            Up = m_btnPutGameData,
            Down = m_btnGetReceipts
        };
        m_focusManager.Mappings[m_btnGetReceipts] = new FocusManager.ButtonMapping()
        {
            Up = m_btnGetProducts
        };

        // set default selection
        m_focusManager.SelectedButton = m_btnGetGamerInfo;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void Update()
    {
        if (OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_DPAD_DOWN))
        {
            m_focusManager.FocusDown();
        }
        if (OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_DPAD_LEFT))
        {
            m_focusManager.FocusLeft();
        }
        if (OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_DPAD_RIGHT))
        {
            m_focusManager.FocusRight();
        }
        if (OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_DPAD_UP))
        {
            m_focusManager.FocusUp();
        }
    }
#endif

    public class FocusManager
    {
        private const int DELAY_MS = 150;

        public object SelectedButton = null;

        private void SetSelection(object selection)
        {
            if (null != selection)
            {
                SelectedButton = selection;
            }
        }

        public class ButtonMapping
        {
            public object Up = null;
            public object Left = null;
            public object Right = null;
            public object Down = null;
        }

        public Dictionary<object, ButtonMapping> Mappings = new Dictionary<object, ButtonMapping>();

        public void FocusDown()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Down);
            }
        }
        public void FocusLeft()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Left);
            }
        }
        public void FocusRight()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Right);
            }
        }
        public void FocusUp()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Up);
            }
        }
    }

    #endregion
}