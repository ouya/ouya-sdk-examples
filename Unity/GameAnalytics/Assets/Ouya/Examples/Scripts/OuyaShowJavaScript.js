#pragma strict

import System.Collections.Generic;

public class OuyaShowJavaScript extends MonoBehaviour implements
    OuyaSDK.IPauseListener,
    OuyaSDK.IResumeListener,
    OuyaSDK.IMenuButtonUpListener,
    OuyaSDK.IMenuAppearingListener,
	OuyaSDK.IFetchGamerInfoListener,
	OuyaSDK.IGetProductsListener,
	OuyaSDK.IPurchaseListener,
	OuyaSDK.IGetReceiptsListener
{
    function Awake()
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

    function OnDestroy()
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

    public function OuyaMenuButtonUp()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaMenuAppearing()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaOnPause()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaOnResume()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaFetchGamerInfoOnSuccess(uuid : String, username : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaFetchGamerInfoOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaFetchGamerInfoOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaGetProductsOnSuccess(products : List.<OuyaSDK.Product>)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
		for (var index : int = 0; index < products.Count; ++index)
        {
			//product : OuyaSDK.Product in products
        }
    }
    public function OuyaGetProductsOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaGetProductsOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaPurchaseOnSuccess(product : OuyaSDK.Product)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaPurchaseOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaPurchaseOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OuyaGetReceiptsOnSuccess(receipts : List.<OuyaSDK.Receipt>)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        for (var receipt : OuyaSDK.Receipt in receipts)
        {
                if (receipt.identifier == "__MY_ID__")
                {
                    //detected purchase
                }
        }
    }
    public function OuyaGetReceiptsOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function OuyaGetReceiptsOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
}