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

public class OuyaShowProducts : MonoBehaviour,
    OuyaSDK.IPauseListener, OuyaSDK.IResumeListener,
    OuyaSDK.IFetchGamerInfoListener,
    OuyaSDK.IGetProductsListener, OuyaSDK.IPurchaseListener, OuyaSDK.IGetReceiptsListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener
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

    void Awake()
    {
        OuyaSDK.registerMenuButtonUpListener(this);
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
        OuyaSDK.unregisterMenuButtonUpListener(this);
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
        OuyaSDK.unregisterFetchGamerInfoListener(this);
        OuyaSDK.unregisterGetProductsListener(this);
        OuyaSDK.unregisterPurchaseListener(this);
        OuyaSDK.unregisterGetReceiptsListener(this);
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
        foreach (OuyaSDK.Product product in products)
        {
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

    private void OnGUI()
    {
        try
        {
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
            if (GUILayout.Button("Get Gamer Info", GUILayout.Height(40)))
            {
                OuyaSDK.fetchGamerInfo();
            }
            GUILayout.EndHorizontal();

            GUILayout.Label(string.Empty);
            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (GUILayout.Button("Put Game Data", GUILayout.Height(40)))
            {
                OuyaSDK.putGameData(KEY_PUT_GAME_DATA, "This is a test!!!!");
            }
            if (GUILayout.Button("Get Game Data", GUILayout.Height(40)))
            {
                m_gameData = OuyaSDK.getGameData(KEY_PUT_GAME_DATA);
            }
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
            if (GUILayout.Button("Get Products", GUILayout.Height(40)))
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
            GUILayout.EndHorizontal();

            foreach (OuyaSDK.Product product in m_products)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(400);

                GUILayout.Label(string.Format("Name={0}", product.name));
                GUILayout.Label(string.Format("Price={0}", product.priceInCents));
                GUILayout.Label(string.Format("Identifier={0}", product.identifier));

                if (GUILayout.Button("Purchase"))
                {
                    Debug.Log(string.Format("Purchase Identifier: {0}", product.identifier));
                    OuyaSDK.Purchasable purchasable = new OuyaSDK.Purchasable();
                    purchasable.productId = product.identifier;
                    OuyaSDK.requestPurchase(purchasable);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Label(string.Empty);

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            GUILayout.Label("Receipts:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(400);
            if (GUILayout.Button("Get Receipts", GUILayout.Height(40)))
            {
                OuyaSDK.requestReceiptList();
            }
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

    #endregion
}