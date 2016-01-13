#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif

using UnityEngine;

public static class AdventureInput
{
#if UNITY_ANDROID && !UNITY_EDITOR
	static Vector3 _currentMousePosition = new Vector3(Screen.width * 0.5f,Screen.height * 0f,0f);
	static Vector3 _lastMousePosition = new Vector3(0f,0f,0f);

	const float xScale = 0.5f;
	const float yScale = 0.5f;
	const float moveSpeed = 1f;
#endif
    public static Vector3 mousePosition
    {
        get
        {

#if UNITY_ANDROID && !UNITY_EDITOR

			//Debug.Log("input = " + Input.mousePosition +  " last = " + _lastMousePosition);
			if (Input.mousePosition != _lastMousePosition) {
				// override with mouse
				_lastMousePosition = Input.mousePosition;
				_currentMousePosition = Input.mousePosition;
				return Input.mousePosition;
			}


			Vector3 newTargetPosition =  _currentMousePosition;
			float moveX = OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_X);			
			float moveY = OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_Y) * -1f;

			//Debug.Log("moveX = " + moveX + "moveY = " + moveY);
			//movement cutoff when remove is around 0.2,0.2 deadzone
			if (Mathf.Abs(moveX) < 0.2f) {
				moveX = 0f;
			}
			if (Mathf.Abs(moveY) < 0.2f) {
				moveY = 0f;
			}

			moveX *= Time.deltaTime;
			moveY *= Time.deltaTime;

			newTargetPosition.x = Mathf.Clamp(newTargetPosition.x + (Screen.width * xScale * moveX),0,Screen.width);
			newTargetPosition.y = Mathf.Clamp(newTargetPosition.y + (Screen.height * yScale * moveY),0f,Screen.height);
						
			_currentMousePosition = newTargetPosition;  // Vector3.sLerp(_currentMousePosition,newTargetPosition,Time.deltaTime * moveSpeed);
			return _currentMousePosition;


			/*
            float halfWidth = Screen.width * 0.5f;
            float halfHeight = Screen.height * 0.5f;
            float x = halfWidth + halfWidth * OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_X);
            float y = halfHeight - halfHeight * OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_Y);
            return new Vector3(x, y, 0);
			 */


#else

            return Input.mousePosition;
#endif
        }
    }

	// TODO: add things from MovablePickup (zoommovable, rotate movable, throwmovable)
    public static float GetAxis(string axisName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (axisName)
        {
            case "Horizontal":
                return OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_X);
            case "Vertical":
                return OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_Y);
			case "Mouse ScrollWheel":
				return OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_RS_Y);
            default:
                Debug.LogError(string.Format("Unknown Axis: {0}", axisName));
                break;
        }
        return 0f;
#else
        return Input.GetAxis(axisName);
#endif
    }

    public static float GetAxisRaw(string axisName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return GetAxis(axisName);
#else
        return Input.GetAxisRaw(axisName);
#endif
    }


	// TODO: add RUN
    public static bool GetButton(string buttonName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (buttonName)
        {
            case "ToggleCursor":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_L3);
            case "EndCutscene":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_Y);
            case "InteractionA":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_A);
            case "InteractionB":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_R1);
            case "Jump":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_R3);
            case "ToggleRun":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_L1);
            case "FlashHotspots":
                return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_U);
			case "Menu":
				return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_MENU);
            default:
                Debug.LogError(string.Format("Unknown Button: {0}", buttonName));
                return false;
        }
#else
        return Input.GetButton(buttonName);
#endif
    }

    public static bool GetButtonDown(string buttonName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (buttonName)
        {
            case "ToggleCursor":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_L3);
            case "EndCutscene":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_Y);
            case "InteractionA":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_A);
            case "InteractionB":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_R1);
            case "Jump":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_R3);
            case "ToggleRun":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_L1);
            case "FlashHotspots":
                return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_U);
			case "Menu":
				return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_MENU);
            default:
                Debug.LogError(string.Format("Unknown ButtonDown: {0}", buttonName));
                return false;
        }
#else
        return Input.GetButtonDown(buttonName);
#endif
    }

	public static bool GetButtonUp(string buttonName)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		switch (buttonName)
		{
		case "ToggleCursor":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_L3);
		case "EndCutscene":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_Y);
		case "InteractionA":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_A);
		case "InteractionB":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_R1);
		case "Jump":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_R3);
		case "ToggleRun":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_L1);
		case "FlashHotspots":
			return OuyaSDK.OuyaInput.GetButtonUp(0, OuyaController.BUTTON_U);
		default:
			Debug.LogError(string.Format("Unknown ButtonUp: {0}", buttonName));
			return false;
		}
		#else
		return Input.GetButtonUp(buttonName);
		#endif
	}

    public static bool GetMouseButton(int button)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (button)
        {
            case 0:
				return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_O) || Input.GetMouseButton(button);
			case 1:
				return OuyaSDK.OuyaInput.GetButton(0, OuyaController.BUTTON_A) || Input.GetMouseButton(button);;
            default:
                Debug.LogError(string.Format("Unknown MouseButton: {0}", button));
                return false;
        }
#else
        return Input.GetMouseButton(button);
#endif
    }

    public static bool GetMouseButtonDown(int button)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        switch (button)
        {
            case 0:
				return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_O) || Input.GetMouseButtonDown(0);
			case 1:
				return OuyaSDK.OuyaInput.GetButtonDown(0, OuyaController.BUTTON_A) || Input.GetMouseButtonDown(1);
            default:
			Debug.LogError(string.Format("Unknown MouseButtonDown: {0}", button));
                return false;
        }
#else
        return Input.GetMouseButtonDown(button);
#endif
    }

	// todo : add touchcount

	public static Touch GetTouch(int index)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		return Input.GetTouch(index);
		#else
		return Input.GetTouch(index);
		#endif
	}


}