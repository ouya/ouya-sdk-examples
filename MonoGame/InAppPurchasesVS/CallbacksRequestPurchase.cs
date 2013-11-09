using InAppPurchases;
using System.Collections.Generic;
using System.Reflection;

namespace Tv.Ouya.Sdk.Android
{
    public class CallbacksRequestPurchase
    {
        public void OnSuccess(Product product)
        {
            Game1.m_debugText = "Request Purchase has completed succesfully.";
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