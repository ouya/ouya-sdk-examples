using InAppPurchases;
using System.Collections.Generic;
using System.Reflection;

namespace Tv.Ouya.Sdk.Android
{
    public class CallbacksRequestReceipts
    {
        public void OnSuccess(List<Receipt> receipts)
        {
            Game1.m_receipts = receipts;
            Game1.m_debugText = string.Format("Found {0} receipts", receipts.Count);
            Game1.m_focusManager.UpdateReceiptFocus(receipts.Count);
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