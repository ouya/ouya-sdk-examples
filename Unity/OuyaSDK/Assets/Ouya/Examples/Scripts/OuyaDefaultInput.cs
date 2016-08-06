using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections;
using tv.ouya.console.api;
#endif

class OuyaDefaultInput : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public IEnumerator Start()
    {
        // wait for IAP to initialize
        while (!OuyaSDK.isIAPInitComplete())
        {
            yield return null;
        }

        OuyaSDK.useDefaultInput();
    }
#endif

    void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Unity Default Input");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
}
