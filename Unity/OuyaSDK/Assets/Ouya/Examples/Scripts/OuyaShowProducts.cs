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

#define XIAOMI

using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif
using UnityEngine;

public class OuyaShowProducts : MonoBehaviour
#if UNITY_ANDROID && !UNITY_EDITOR
    ,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IRequestGamerInfoListener,
    OuyaSDK.IRequestProductsListener, OuyaSDK.IRequestPurchaseListener, OuyaSDK.IRequestReceiptsListener,
    OuyaSDK.IMenuAppearingListener
#endif
{
    /// <summary>
    /// The products to display for purchase
    /// </summary>
    public string[] Purchasables =
    {
        "long_sword",
        "sharp_axe",
        "__DECLINED__THIS_PURCHASE",
    };

#if UNITY_ANDROID && !UNITY_EDITOR

    /// <summary>
    /// Handle focusing items
    /// </summary>
    private FocusManager m_focusManager = new FocusManager();

    /// <summary>
    /// Show the current state
    /// </summary>
    private string m_state = string.Empty;

    /// <summary>
    /// Show the current status
    /// </summary>
    private string m_status = string.Empty;

    /// <summary>
    /// The gamer UUID
    /// </summary>
    private string m_gamerUUID = string.Empty;

    /// <summary>
    /// The gamer User Name
    /// </summary>
    private string m_gamerUsername = string.Empty;

    /// <summary>
    /// A key to store game data
    /// </summary>
    private const string KEY_PUT_GAME_DATA = "ShowProductsExample";

    /// <summary>
    /// The game data to display what was stored
    /// </summary>
    private string m_gameData = string.Empty;

    /// <summary>
    /// Check for is running on OUYA Hardware
    /// </summary>
    private bool m_isRunningOnOUYAHardware = false;

    /// <summary>
    /// Buttons
    /// </summary>
    private object m_btnPutGameData = new object();
    private object m_btnGetGameData = new object();
    private object m_btnRequestGamerInfo = new object();
    private object m_btnRequestProducts = new object();
    private object m_btnRequestReceipts = new object();
    private object m_btnExit = new object();

    void Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
        OuyaSDK.registerRequestGamerInfoListener(this);
        OuyaSDK.registerRequestProductsListener(this);
        OuyaSDK.registerRequestPurchaseListener(this);
        OuyaSDK.registerRequestReceiptsListener(this);
    }
    void OnDestroy()
    {
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
        OuyaSDK.unregisterRequestGamerInfoListener(this);
        OuyaSDK.unregisterRequestProductsListener(this);
        OuyaSDK.unregisterRequestPurchaseListener(this);
        OuyaSDK.unregisterRequestReceiptsListener(this);
    }

    public void OuyaMenuAppearing()
    {
        m_status = "OUYA Menu is appearing";
    }

    public void OuyaOnPause()
    {
        m_state = "Detected Pause";
    }

    public void OuyaOnResume()
    {
        m_state = "Detected Resume";
    }

    public void RequestGamerInfoOnSuccess(OuyaSDK.GamerInfo gamerInfo)
    {
        m_status = "RequestGamerInfoOnSuccess";
        m_gamerUsername = gamerInfo.username;
        m_gamerUUID = gamerInfo.uuid;
    }

    public void RequestGamerInfoOnFailure(int errorCode, string errorMessage)
    {
        m_status = string.Format("RequestGamerInfoOnFailure: error={0} errorMessage={1}", errorCode, errorMessage);
    }

    public void RequestGamerInfoOnCancel()
    {
        m_status = "RequestGamerInfoOnCancel";
    }

    public void RequestProductsOnSuccess(List<OuyaSDK.Product> products)
    {
        m_status = "RequestProductsOnSuccess";
        m_products.Clear();
        for (int index = 0; index < products.Count; ++index)
        {
            OuyaSDK.Product product = products[index];
            // Get Products Right goes to the first element
            if (index == 0)
            {
                m_focusManager.Mappings[m_btnRequestProducts].Right = product;
            }
            // Products left goes back to the GetProducts button
            m_focusManager.Mappings[product] = new FocusManager.ButtonMapping()
            {
                Left = m_btnRequestProducts
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

    public void RequestProductsOnFailure(int errorCode, string errorMessage)
    {
        m_status = string.Format("RequestProductsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage);
    }

    public void RequestProductsOnCancel()
    {
        m_status = "RequestProductsOnCancel";
    }

    public void RequestPurchaseOnSuccess(OuyaSDK.Product product)
    {
        m_status = string.Format("RequestPurchaseOnSuccess: {0}", product.identifier);
    }

    public void RequestPurchaseOnFailure(int errorCode, string errorMessage)
    {
        m_status = string.Format("RequestPurchaseOnFailure: error={0} errorMessage={1}", errorCode, errorMessage);
    }

    public void RequestPurchaseOnCancel()
    {
        m_status = "RequestPurchaseOnCancel";
    }

    public void RequestReceiptsOnSuccess(List<OuyaSDK.Receipt> receipts)
    {
        m_status = "RequestReceiptsOnSuccess";
        m_receipts.Clear();
        foreach (OuyaSDK.Receipt receipt in receipts)
        {
            m_receipts.Add(receipt);
        }
    }

    public void RequestReceiptsOnFailure(int errorCode, string errorMessage)
    {
        m_status = string.Format("RequestReceiptsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage);
    }

    public void RequestReceiptsOnCancel()
    {
        m_status = "RequestReceiptsOnCancel";
    }

    public bool GetButtonUp(int button)
    {
        for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index)
        {
            if (OuyaSDK.OuyaInput.GetButtonUp(index, button))
            {
                return true;
            }
        }
        return false;
    }

    #region Data containers

    private List<OuyaSDK.Product> m_products = new List<OuyaSDK.Product>();

    private List<OuyaSDK.Receipt> m_receipts = new List<OuyaSDK.Receipt>();

    #endregion

    #region Presentation

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
            GUILayout.Label(string.Format("IsRunningOnOUYAHardware: {0}", m_isRunningOnOUYAHardware));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("App Name: {0}", OuyaSDK.getStringResource("app_name")));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(OuyaSDK.isIAPInitComplete() ? "IAP is initialized" : "IAP initializing...");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("State: {0}", m_state));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Status: {0}", m_status));
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Gamer UUID: {0}", m_gamerUUID));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label(string.Format("Gamer User Name: {0}", m_gamerUsername));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnRequestGamerInfo)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Request Gamer Info", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnRequestGamerInfo &&
                GetButtonUp(OuyaController.BUTTON_O)))
            {
                m_status = "Requesting gamer info...";
                OuyaSDK.requestGamerInfo();
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
                GetButtonUp(OuyaController.BUTTON_O)))
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
                GetButtonUp(OuyaController.BUTTON_O)))
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
            if (m_focusManager.SelectedButton == m_btnRequestProducts)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Request Products", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnRequestProducts &&
                GetButtonUp(OuyaController.BUTTON_O)))
            {
                List<OuyaSDK.Purchasable> productIdentifierList =
                    new List<OuyaSDK.Purchasable>();

                foreach (string productId in Purchasables)
                {
                    OuyaSDK.Purchasable purchasable = new OuyaSDK.Purchasable();
                    purchasable.productId = productId;
                    productIdentifierList.Add(purchasable);
                }

                m_status = "Requesting products...";
                OuyaSDK.requestProducts(productIdentifierList);
            }
            GUI.backgroundColor = oldColor;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            foreach (OuyaSDK.Product product in m_products)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(500);

                GUILayout.Label(string.Format("Name={0}", product.name));
                GUILayout.Label(string.Format("Price={0}", product.localPrice));
                GUILayout.Label(string.Format("Identifier={0}", product.identifier));

                GUILayout.FlexibleSpace();

                if (m_focusManager.SelectedButton == product)
                {
                    GUI.backgroundColor = Color.red;
                }
                if (GUILayout.Button("Request Purchase") ||
                    (m_focusManager.SelectedButton == product &&
                    GetButtonUp(OuyaController.BUTTON_O)))
                {
                    m_status = "Requesting purchase...";
                    //Debug.Log(string.Format("Purchase Identifier: {0}", product.identifier));
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
            if (m_focusManager.SelectedButton == m_btnRequestReceipts)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Request Receipts", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnRequestReceipts &&
                GetButtonUp(OuyaController.BUTTON_O)))
            {
                m_status = "Requesting receipts...";
                OuyaSDK.requestReceipts();
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();

            foreach (OuyaSDK.Receipt receipt in m_receipts)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);

                GUILayout.Label(string.Format("LocalPrice={0}", receipt.localPrice));
                GUILayout.Label(string.Format("Identifier={0}", receipt.identifier));

                GUILayout.EndHorizontal();
            }

            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (m_focusManager.SelectedButton == m_btnExit)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button("Exit", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == m_btnExit &&
                GetButtonUp(OuyaController.BUTTON_O)))
            {
                m_status = "Exiting...";
                Application.Quit();
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
        }
        catch (System.Exception)
        {
        }
    }

    #endregion

    #region Focus Handling

    public IEnumerator Start()
    {
        m_focusManager.Mappings[m_btnRequestGamerInfo] = new FocusManager.ButtonMapping()
        {
            Down = m_btnPutGameData
        };
        m_focusManager.Mappings[m_btnPutGameData] = new FocusManager.ButtonMapping()
        {
            Up = m_btnRequestGamerInfo,
            Right = m_btnGetGameData,
            Down = m_btnRequestProducts
        };
        m_focusManager.Mappings[m_btnGetGameData] = new FocusManager.ButtonMapping()
        {
            Up = m_btnRequestGamerInfo,
            Left = m_btnPutGameData,
            Down = m_btnRequestProducts
        };
        m_focusManager.Mappings[m_btnRequestProducts] = new FocusManager.ButtonMapping()
        {
            Up = m_btnPutGameData,
            Down = m_btnRequestReceipts
        };
        m_focusManager.Mappings[m_btnRequestReceipts] = new FocusManager.ButtonMapping()
        {
            Up = m_btnRequestProducts,
            Down = m_btnExit
        };
        m_focusManager.Mappings[m_btnExit] = new FocusManager.ButtonMapping()
        {
            Up = m_btnRequestReceipts
        };

        // set default selection
        m_focusManager.SelectedButton = m_btnRequestGamerInfo;

        while (!OuyaSDK.isIAPInitComplete())
        {
            yield return null;
        }

        m_isRunningOnOUYAHardware = OuyaSDK.isRunningOnOUYASupportedHardware();
    }

    private void Update()
    {
        if (GetButtonUp(OuyaController.BUTTON_DPAD_DOWN))
        {
            m_focusManager.FocusDown();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_LEFT))
        {
            m_focusManager.FocusLeft();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_RIGHT))
        {
            m_focusManager.FocusRight();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_UP))
        {
            m_focusManager.FocusUp();
        }
    }

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
#endif
}