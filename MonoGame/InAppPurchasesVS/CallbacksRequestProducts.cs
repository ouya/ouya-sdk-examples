using InAppPurchases;
using System.Collections.Generic;
using System.Reflection;

namespace Tv.Ouya.Sdk.Android
{
    public class CallbacksRequestProducts
    {
        public void OnSuccess(List<Product> products)
        {
            Game1.m_products = products;
            Game1.m_debugText = string.Format("Found {0} products", products.Count);
            Game1.m_focusManager.UpdateProductFocus(products.Count);
        }

        public void OnFailure(int errorCode, string errorMessage)
        {
            Game1.m_debugText = string.Format("{0} errorCode={1} errorMessage={2}", MethodBase.GetCurrentMethod(), errorCode, errorMessage);
        }

        public void OnCancel()
        {
            Game1.m_debugText = MethodBase.GetCurrentMethod().Name;
        }
    }
}