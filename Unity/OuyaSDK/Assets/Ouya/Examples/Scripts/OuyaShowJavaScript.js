#pragma strict

import System.Collections.Generic;

public class OuyaShowJavaScript extends MonoBehaviour
#if UNITY_ANDROID && !UNITY_EDITOR
    implements
    OuyaSDK.IPauseListener,
    OuyaSDK.IResumeListener,
    OuyaSDK.IMenuAppearingListener,
	OuyaSDK.IRequestGamerInfoListener,
	OuyaSDK.IRequestProductsListener,
	OuyaSDK.IRequestPurchaseListener,
	OuyaSDK.IRequestReceiptsListener
#endif
{

#if UNITY_ANDROID && !UNITY_EDITOR
    function Awake()
    {
        OuyaSDK.registerMenuAppearingListener(this);
        OuyaSDK.registerPauseListener(this);
        OuyaSDK.registerResumeListener(this);
        OuyaSDK.registerRequestGamerInfoListener(this);
        OuyaSDK.registerRequestProductsListener(this);
        OuyaSDK.registerRequestPurchaseListener(this);
        OuyaSDK.registerRequestReceiptsListener(this);
    }

    function OnDestroy()
    {
        OuyaSDK.unregisterMenuAppearingListener(this);
        OuyaSDK.unregisterPauseListener(this);
        OuyaSDK.unregisterResumeListener(this);
        OuyaSDK.unregisterRequestGamerInfoListener(this);
        OuyaSDK.unregisterRequestProductsListener(this);
        OuyaSDK.unregisterRequestPurchaseListener(this);
        OuyaSDK.unregisterRequestReceiptsListener(this);
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

    public function RequestGamerInfoOnSuccess(uuid : String, username : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestGamerInfoOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestGamerInfoOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function RequestProductsOnSuccess(products : List.<OuyaSDK.Product>)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
		for (var index : int = 0; index < products.Count; ++index)
        {
			//product : OuyaSDK.Product in products
        }
    }
    public function RequestProductsOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestProductsOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function RequestPurchaseOnSuccess(product : OuyaSDK.Product)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestPurchaseOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestPurchaseOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function RequestReceiptsOnSuccess(receipts : List.<OuyaSDK.Receipt>)
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
    public function RequestReceiptsOnFailure(errorCode : int, errorMessage : String)
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }
    public function RequestReceiptsOnCancel()
    {
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    public function OnGUI() {
        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label("This is JavaScript! Check out the code for syntax examples.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
#endif
}