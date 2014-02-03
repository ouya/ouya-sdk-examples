//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Simple example script of how a button can be offset visibly when the mouse hovers over it or it gets pressed.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;
	public Vector3 hover = Vector3.zero;
	public Vector3 pressed = new Vector3(2f, -2f);
	public float duration = 0.2f;

	Vector3 mPos;
	bool mInitDone = false;
	bool mStarted = false;
	bool mHighlighted = false;

	void Start () { mStarted = true; }

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (tweenTarget != null)
		{
			TweenPosition tc = tweenTarget.GetComponent<TweenPosition>();

			if (tc != null)
			{
				tc.position = mPos;
				tc.enabled = false;
			}
		}
	}

	void Init ()
	{
		mInitDone = true;
		if (tweenTarget == null) tweenTarget = transform;
		mPos = tweenTarget.localPosition;
	}

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (!mInitDone) Init();
			TweenPosition.Begin(tweenTarget.gameObject, duration, isPressed ? mPos + pressed :
				(UICamera.IsHighlighted(gameObject) ? mPos + hover : mPos)).method = UITweener.Method.EaseInOut;
		}
	}

	void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (!mInitDone) Init();
			TweenPosition.Begin(tweenTarget.gameObject, duration, isOver ? mPos + hover : mPos).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}