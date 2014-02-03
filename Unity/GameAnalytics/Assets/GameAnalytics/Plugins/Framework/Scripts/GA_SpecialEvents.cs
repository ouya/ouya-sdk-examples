/// <summary>
/// This class handles special events unique to the Unity Wrapper, such as submitting level/scene changes, and delaying application quit
/// until data has been sent.
/// </summary>

using UnityEngine;
using System.Collections;

public class GA_SpecialEvents : MonoBehaviour
{
	/*[HideInInspector]
	public bool SubmitFpsAverage;
	[HideInInspector]
	public bool SubmitFpsCritical;
	[HideInInspector]
	public bool IncludeSceneChange;
	[HideInInspector]
	public int FpsCriticalThreshold;
	[HideInInspector]
	public int FpsSubmitInterval;*/
	#region private values
	
	private float _lastLevelStartTime = 0f;
	
	private static int _frameCountAvg = 0;
	private static float _lastUpdateAvg = 0f;
	private int _frameCountCrit = 0;
	private float _lastUpdateCrit = 0f;
	
	#endregion
	
	#region unity derived methods
	
	public void Start ()
	{
		SceneChange();
		
		StartCoroutine(SubmitFPSRoutine());
	}
	
	private IEnumerator SubmitFPSRoutine()
	{
		while(Application.isPlaying && GA_SystemTracker.GA_SYSTEMTRACKER != null && GA_SystemTracker.GA_SYSTEMTRACKER.SubmitFpsCritical)
		{
			SubmitCriticalFPS();
			yield return new WaitForSeconds(GA_SystemTracker.GA_SYSTEMTRACKER.FpsCirticalSubmitInterval);
		}
	}
	
	public void Update()
	{
		//average FPS
		if (GA_SystemTracker.GA_SYSTEMTRACKER.SubmitFpsAverage)
		{
			_frameCountAvg++;
		}
		
		//critical FPS
		if (GA_SystemTracker.GA_SYSTEMTRACKER.SubmitFpsCritical)
		{
			_frameCountCrit++;
		}
	}
	
	public void OnLevelWasLoaded ()
	{
		SceneChange();
	}
	
	public void OnApplicationQuit ()
	{
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
		if (!GA_Queue.QUITONSUBMIT)
		{
			SubmitAverageFPS();
		}
#endif
	}
	
	public static void SubmitAverageFPS()
	{
		//average FPS
		if (GA_SystemTracker.GA_SYSTEMTRACKER != null && GA_SystemTracker.GA_SYSTEMTRACKER.SubmitFpsAverage)
		{
			float timeSinceUpdate = Time.time - _lastUpdateAvg;
			
			if (timeSinceUpdate > 1.0f)
			{
				float fpsSinceUpdate = _frameCountAvg / timeSinceUpdate;
				_lastUpdateAvg = Time.time;
				_frameCountAvg = 0;
				
				if (fpsSinceUpdate > 0)
				{
					if (GA.SettingsGA.TrackTarget != null)
						GA.API.Design.NewEvent("GA:AverageFPS", ((int)fpsSinceUpdate), GA.SettingsGA.TrackTarget.position);
					else
						GA.API.Design.NewEvent("GA:AverageFPS", ((int)fpsSinceUpdate));
				}
			}
		}
	}
	
	public void SubmitCriticalFPS()
	{
		//critical FPS
		if (GA_SystemTracker.GA_SYSTEMTRACKER != null && GA_SystemTracker.GA_SYSTEMTRACKER.SubmitFpsCritical)
		{
			float timeSinceUpdate = Time.time - _lastUpdateCrit;
			
			if (timeSinceUpdate > 1.0f)
			{
				float fpsSinceUpdate = _frameCountCrit / timeSinceUpdate;
				_lastUpdateCrit = Time.time;
				_frameCountCrit = 0;
				
				if (fpsSinceUpdate <= GA_SystemTracker.GA_SYSTEMTRACKER.FpsCriticalThreshold)
				{
					if (GA.SettingsGA.TrackTarget != null)
						GA.API.Design.NewEvent("GA:CriticalFPS", _frameCountCrit, GA.SettingsGA.TrackTarget.position);
					else
						GA.API.Design.NewEvent("GA:CriticalFPS", _frameCountCrit);
					
				}
			}
		}
	}
	
	#endregion
	
	#region private methods
	
	private void SceneChange()
	{
		if (GA_SystemTracker.GA_SYSTEMTRACKER.IncludeSceneChange)
		{
			if (GA.SettingsGA.TrackTarget != null)
				GA.API.Design.NewEvent("GA:LevelStarted", Time.time - _lastLevelStartTime, GA.SettingsGA.TrackTarget.position);
			else
				GA.API.Design.NewEvent("GA:LevelStarted", Time.time - _lastLevelStartTime);
		}
		_lastLevelStartTime = Time.time;
	}
	
	#endregion
	
}
