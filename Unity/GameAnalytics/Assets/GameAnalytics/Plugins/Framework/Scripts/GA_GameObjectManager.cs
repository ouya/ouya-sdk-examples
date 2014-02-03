using UnityEngine;
using System.Collections;

public class GA_GameObjectManager : MonoBehaviour {
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	public void RunCoroutine(IEnumerator routine)
	{
		StartCoroutine(routine);
	}
	
	public void OnApplicationQuit ()
	{
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		if (!GA_Queue.QUITONSUBMIT)
		{
			GA_Queue.QUITONSUBMIT = true;
			GA.API.Design.NewEvent("GA:ExitGame");
			GA_Queue.SubmitQueue();
			Application.CancelQuit();
		}
#endif
	}
	
#if UNITY_IPHONE || UNITY_ANDROID
	void OnApplicationPause(bool paused)
	{
		if (!paused && GA.SettingsGA.NewSessionOnResume)
		{
			Debug.Log("new session id");
			GA.API.GenericInfo.SetSessionUUID();
		}
	}
#endif
}