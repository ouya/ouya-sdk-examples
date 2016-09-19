using System;
using UnityEngine;

public class DetectPause : MonoBehaviour
{
    private DateTime _mPauseDetected = DateTime.MinValue;

    void OnGUI()
    {
        if (_mPauseDetected != DateTime.MinValue)
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height)); 
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            GUILayout.FlexibleSpace();
            GUILayout.Label("");
            GUILayout.Label("PAUSE DETECTED");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            _mPauseDetected = DateTime.Now + TimeSpan.FromSeconds(1);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        _mPauseDetected = DateTime.Now + TimeSpan.FromSeconds(1);
    }

    void Update()
    {
        if (_mPauseDetected != DateTime.Now &&
            _mPauseDetected < DateTime.Now)
        {
            _mPauseDetected = DateTime.MinValue;
        }
    }
}
