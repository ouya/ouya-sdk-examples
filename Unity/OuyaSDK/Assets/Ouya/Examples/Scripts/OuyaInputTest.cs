using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif

class OuyaInputTest : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    Dictionary<int, Dictionary<int, float>> _axis = new Dictionary<int, Dictionary<int, float>>();
    Dictionary<int, Dictionary<int, DateTime>> _buttonDown = new Dictionary<int, Dictionary<int, DateTime>>();
    Dictionary<int, Dictionary<int, DateTime>> _buttonUp = new Dictionary<int, Dictionary<int, DateTime>>();

    private int[] _axises =
    {
        OuyaController.AXIS_LS_X,
        OuyaController.AXIS_LS_Y,
        OuyaController.AXIS_RS_X,
        OuyaController.AXIS_RS_Y,
        OuyaController.AXIS_L2,
        OuyaController.AXIS_R2,
    };

    private int[] _buttons =
    {
        OuyaController.BUTTON_O,
        OuyaController.BUTTON_U,
        OuyaController.BUTTON_Y,
        OuyaController.BUTTON_A,
        OuyaController.BUTTON_L1,
        OuyaController.BUTTON_L3,
        OuyaController.BUTTON_R1,
        OuyaController.BUTTON_R3,
        OuyaController.BUTTON_DPAD_DOWN,
        OuyaController.BUTTON_DPAD_LEFT,
        OuyaController.BUTTON_DPAD_RIGHT,
        OuyaController.BUTTON_DPAD_UP,
        OuyaController.BUTTON_MENU,
    };

    float GetAxis(int controller, int axis, Dictionary<int, Dictionary<int, float>> dict)
    {
        if (null == dict)
        {
            return 0f;
        }
        if (dict.ContainsKey(controller))
        {
            if (dict[controller].ContainsKey(axis))
            {
                return dict[controller][axis];
            }
        }
        return 0f;
    }

    bool CheckButton(int controller, int button, Dictionary<int, Dictionary<int, DateTime>> dict)
    {
        if (null == dict)
        {
            return false;
        }
        if (dict.ContainsKey(controller))
        {
            if (dict[controller].ContainsKey(button))
            {
                return dict[controller][button] > DateTime.Now;
            }
        }
        return false;
    }

    void Awake()
    {
        for (int controller = 0; controller < OuyaController.MAX_CONTROLLERS; ++controller)
        {
            _axis[controller] = new Dictionary<int, float>();
            _buttonDown[controller] = new Dictionary<int, DateTime>();
            _buttonUp[controller] = new Dictionary<int, DateTime>();
        }
    }

    void Update()
    {
        if (null == _axis ||
            null == _buttonDown ||
            null == _buttonUp)
        {
            return;
        }
        for (int controller = 0; controller < OuyaController.MAX_CONTROLLERS; ++controller)
        {
            if (_axis.ContainsKey(controller) &&
                _axis[controller] != null)
            {
                foreach (int axis in _axises)
                {
                    _axis[controller][axis] = OuyaSDK.OuyaInput.GetAxis(controller, axis);
                }
            }

            if (_buttonDown.ContainsKey(controller) &&
                _buttonDown[controller] != null)
            {
                foreach (int button in _buttons)
                {
                    if (OuyaSDK.OuyaInput.GetButtonDown(controller, button))
                    {
                        _buttonDown[controller][button] = DateTime.Now + TimeSpan.FromSeconds(0.5f);
                    }
                    if (OuyaSDK.OuyaInput.GetButtonUp(controller, button))
                    {
                        _buttonUp[controller][button] = DateTime.Now + TimeSpan.FromSeconds(0.5f);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        {
            GUILayout.FlexibleSpace();
            for (int controller = 0; controller < OuyaController.MAX_CONTROLLERS; ++controller)
            {
                GUILayout.BeginVertical(GUILayout.Height(Screen.height));
                {
                    foreach (int axis in _axises)
                    {
                        GUILayout.Label(string.Format("({0}) {1} val={2}",
                            axis,
                            DebugInput.DebugGetAxisName(axis),
                            GetAxis(controller, axis, _axis)));
                    }
                    GUILayout.FlexibleSpace();
                    foreach (int button in _buttons)
                    {
                        GUILayout.Label(string.Format("({0}) {1} up={2} down={3}",
                            button,
                            DebugInput.DebugGetButtonName(button),
                            CheckButton(controller, button, _buttonUp),
                            CheckButton(controller, button, _buttonDown)));
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
#endif
}