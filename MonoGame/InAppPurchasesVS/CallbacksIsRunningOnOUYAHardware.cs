using InAppPurchases;
using System.Reflection;

namespace Tv.Ouya.Sdk.Android
{
    public class CallbacksIsRunningOnOUYAHardware
    {
        public void OnSuccess(bool isRunningOnOUYAHardware)
        {
            Game1.m_debugText = string.Format("Running on OUYA Hardware: {0}", isRunningOnOUYAHardware);
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