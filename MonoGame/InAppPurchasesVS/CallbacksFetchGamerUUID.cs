using InAppPurchases;
using System.Reflection;

namespace Tv.Ouya.Sdk.Android
{
    public class CallbacksFetchGamerUUID
    {
        public void OnSuccess(string gamerUUID)
        {
            Game1.m_debugText = string.Format("Request Gamer UUID: {0}", gamerUUID);
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