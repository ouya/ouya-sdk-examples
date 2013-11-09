using Android.App;
using Android.Content;
using Android.Runtime;
using System;
using System.Collections.Generic;
using tv.ouya.sdk.monogame;

namespace Tv.Ouya.Sdk.Android
{
    public class PluginOuya
    {

        public static void CacheClasses()
        {
        }

        public static void OuyaControllerInit(Context context)
        {
            OuyaController.init(context);
        }

        public static void AsyncSetActivity(Activity activity)
        {
            AsyncCppOuyaSetActivity.invoke(activity);
        }

        public static void AsyncSetApplicationKey(Context context)
        {
            AsyncCppOuyaSetApplicationKey.invoke(context);
        }

        public static void AsyncSetDeveloperId(string developerId)
        {
            AsyncCppOuyaSetDeveloperId.invoke(developerId);
        }

        public static void AsyncOuyaFetchGamerUUID(CallbacksFetchGamerUUID callbacksFetchGamerUUID)
        {
            AsyncCppOuyaFetchGamerUUID.invoke();
        }

        public static void AsyncOuyaRequestProducts(CallbacksRequestProducts callbacksRequestProducts, List<string> productIds)
        {
        }

        public static void AsyncOuyaRequestPurchase(CallbacksRequestPurchase callbacksRequestPurchase, string purchasable)
        {
        }

        public static void AsyncOuyaRequestReceipts(CallbacksRequestReceipts callbacksRequestReceipts)
        {
        }
    }
}