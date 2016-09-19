using System;
using UnityEngine;

public class DetectBack : MonoBehaviour
{
    private DateTime _mBackDetected = DateTime.MinValue;

    void OnGUI()
    {
        if (_mBackDetected != DateTime.MinValue)
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Back detected");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mBackDetected = DateTime.Now + TimeSpan.FromSeconds(1);
        }
        else if (_mBackDetected != DateTime.Now &&
            _mBackDetected < DateTime.Now)
        {
            _mBackDetected = DateTime.MinValue;
        }
    }
}
