using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//using LitJson;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

[RequireComponent(typeof(GA_HeatMapRenderer))]
[ExecuteInEditMode]
public class GA_HeatMapDataFilter : GA_HeatMapDataFilterBase
{
	public enum CombineHeatmapType { None, Add, Subtract, SubtractZero };
	
	[HideInInspector]
	public List<string> AvailableTypes;
	[HideInInspector]
	public int CurrentTypeIndex;
	
	[HideInInspector]
	public List<string> AvailableBuilds;
	[HideInInspector]
	public int CurrentBuildIndex;
	
	public bool RedownloadDataOnPlay;
	
	public bool IgnoreDates = true;
	public bool IgnoreBuilds = true;
	
	public bool DownloadingData = false;
	public bool BuildingHeatmap = false;
	public float BuildHeatmapPercentage = 0;
	
#if UNITY_EDITOR || !UNITY_FLASH
	//Pretty silly hack to force Unity to serialize DateTime. 
	//(It makes sense that it doesn't serialize structs, but there might be a better workaround.
	[SerializeField]
	private string _startDateTime;
	public DateTime? StartDateTime
	{
		get {
			DateTime res = DateTime.Now;
			DateTime.TryParse(_startDateTime,out res);
			return res;
		}
		set {_startDateTime = value.HasValue?value.Value.ToString():"";}
	}
	
	[SerializeField]
	private string _endDateTime;
	public DateTime? EndDateTime
	{
		get {
			DateTime res = DateTime.Now;
			DateTime.TryParse(_endDateTime,out res);
			return res;
		}
		set {_endDateTime = value.HasValue?value.Value.ToString():"";}
	}
#endif
	
	[HideInInspector]
	public List<string> AvailableAreas;
	[HideInInspector]
	public int CurrentAreaIndex;
	
	[HideInInspector]
	public List<string> AvailableEvents;
	[HideInInspector]
	public List<bool> CurrentEventFlag;
	
	[HideInInspector]
	public GA_HeatmapData DataContainer;
	[SerializeField]
	private bool didInit = false;

#if UNITY_EDITOR || !UNITY_FLASH
	private CombineHeatmapType _combineType = CombineHeatmapType.None;
#endif
	
	public void OnEnable ()
	{
#if UNITY_EDITOR
		EditorApplication.hierarchyWindowItemOnGUI += GA.HierarchyWindowCallback;
#endif
		
		if(didInit)
			return;
		
		Debug.Break();
		AvailableBuilds = new List<string>();
		AvailableAreas = new List<string>();
		AvailableTypes = new List<string>();
		AvailableEvents = new List<string>();
		 
		AvailableTypes.Add("Design");
		AvailableTypes.Add("Quality");
		AvailableTypes.Add("Business");
		
#if UNITY_EDITOR || !UNITY_FLASH
		StartDateTime = DateTime.Now;
		EndDateTime = DateTime.Now;
#endif
		
		didInit = true;
	}
	
#if UNITY_EDITOR
	void OnDisable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI -= GA.HierarchyWindowCallback;
	}
#endif
	
	void Awake()
	{
		if(Application.isPlaying && !Application.isEditor && RedownloadDataOnPlay)
		{
			DownloadData();
		}
	}
	
	public override List<GA_DataPoint> GetData()
	{
		return DataContainer!=null?DataContainer.Data:null;
	}
	
	public float LoadProgress;
	public bool Loading;
	
	private IEnumerator FollowProgress(WWW progress)
	{
		Loading = true;
		while(!progress.isDone)
		{
			LoadProgress = progress.progress;
			yield return null;
		}
		Loading = false;
		LoadProgress = 0;
		
	}
	
	void NormalizeDataPoints (List<GA_DataPoint> Data)
	{
		float dataMax = 0,dataMin = 0;
		foreach(GA_DataPoint p in Data)
		{
			if(dataMax < p.count)
			{
				dataMax = p.count;
			}
			if(dataMin > p.count)
			{
				dataMin = p.count;
			}			
		}
		
		foreach(GA_DataPoint p in Data)
		{
			p.density = (p.count - dataMin) / (dataMax - dataMin);
			
		}  
	}
	
#if UNITY_EDITOR || !UNITY_FLASH
	public void OnSuccessDownload(GA_Request.RequestType requestType, Hashtable jsonList, GA_Request.SubmitErrorHandler errorEvent)
	{
		DownloadingData = false;
		BuildingHeatmap = true;
		
		if(DataContainer == null)
		{
			var dataContainerObject = new GameObject("GA_Data");
			dataContainerObject.transform.parent = transform;
			DataContainer = dataContainerObject.AddComponent<GA_HeatmapData>();

			DataContainer.Data = new List<GA_DataPoint>();
			GA.Log(DataContainer);
		}
		else if (_combineType == CombineHeatmapType.None)
			DataContainer.Data.Clear();
		
		List<GA_DataPoint> DPsToDelete = new List<GA_DataPoint>();
		
		ArrayList jsonArrayX = (ArrayList)jsonList["x"];
		ArrayList jsonArrayY = (ArrayList)jsonList["y"];
		ArrayList jsonArrayZ = (ArrayList)jsonList["z"];
		ArrayList jsonArrayValue = (ArrayList)jsonList["value"];
		for (int i = 0; i < jsonArrayX.Count; i++)
		{
			try
			{
				bool done = false;
				if (_combineType == CombineHeatmapType.Add)
				{
					Vector3 position = new Vector3(float.Parse(jsonArrayX[i].ToString()) / GA.SettingsGA.HeatmapGridSize.x, float.Parse(jsonArrayY[i].ToString()) / GA.SettingsGA.HeatmapGridSize.y, float.Parse(jsonArrayZ[i].ToString()) / GA.SettingsGA.HeatmapGridSize.z);
					int count = int.Parse(jsonArrayValue[i].ToString());
					
					for (int u = 0; u < DataContainer.Data.Count; u++)
					{
						if (DataContainer.Data[u].position == position)
						{
							DataContainer.Data[u].count += count;
							done = true;
							u = DataContainer.Data.Count;
						}
					}
				}
				else if (_combineType == CombineHeatmapType.Subtract)
				{
					Vector3 position = new Vector3(float.Parse(jsonArrayX[i].ToString()) / GA.SettingsGA.HeatmapGridSize.x, float.Parse(jsonArrayY[i].ToString()) / GA.SettingsGA.HeatmapGridSize.y, float.Parse(jsonArrayZ[i].ToString()) / GA.SettingsGA.HeatmapGridSize.z);
					int count = int.Parse(jsonArrayValue[i].ToString());
					
					for (int u = 0; u < DataContainer.Data.Count; u++)
					{
						if (DataContainer.Data[u].position == position)
						{
							DataContainer.Data[u].count = DataContainer.Data[u].count - count;
							
							u = DataContainer.Data.Count;
							done = true;
						}
					}
				}
				else if (_combineType == CombineHeatmapType.SubtractZero)
				{
					done = true;
					
					Vector3 position = new Vector3(float.Parse(jsonArrayX[i].ToString()) / GA.SettingsGA.HeatmapGridSize.x, float.Parse(jsonArrayY[i].ToString()) / GA.SettingsGA.HeatmapGridSize.y, float.Parse(jsonArrayZ[i].ToString()) / GA.SettingsGA.HeatmapGridSize.z);
					int count = int.Parse(jsonArrayValue[i].ToString());
					
					for (int u = 0; u < DataContainer.Data.Count; u++)
					{
						if (DataContainer.Data[u].position == position)
						{
							DataContainer.Data[u].count = Mathf.Max(DataContainer.Data[u].count - count, 0);
							
							if (DataContainer.Data[u].count == 0)
								DPsToDelete.Add(DataContainer.Data[u]);
							
							u = DataContainer.Data.Count;
						}
					}
				}
				
				if (_combineType == CombineHeatmapType.Subtract && !done)
				{
					GA_DataPoint p = new GA_DataPoint();
					p.position = new Vector3(float.Parse(jsonArrayX[i].ToString()) / GA.SettingsGA.HeatmapGridSize.x, float.Parse(jsonArrayY[i].ToString()) / GA.SettingsGA.HeatmapGridSize.y, float.Parse(jsonArrayZ[i].ToString()) / GA.SettingsGA.HeatmapGridSize.z);
					p.count = -(int.Parse(jsonArrayValue[i].ToString()));
					DataContainer.Data.Add(p);
				}
				else if (_combineType != CombineHeatmapType.Subtract && (_combineType == CombineHeatmapType.None || !done))
				{
					GA_DataPoint p = new GA_DataPoint();
					p.position = new Vector3(float.Parse(jsonArrayX[i].ToString()) / GA.SettingsGA.HeatmapGridSize.x, float.Parse(jsonArrayY[i].ToString()) / GA.SettingsGA.HeatmapGridSize.y, float.Parse(jsonArrayZ[i].ToString()) / GA.SettingsGA.HeatmapGridSize.z);
					p.count = int.Parse(jsonArrayValue[i].ToString());
					DataContainer.Data.Add(p);
				}
			} 
			catch (Exception e)
			{
				// JSON format error
				GA.LogError("GameAnalytics: Error in parsing JSON data from server - " + e.Message);
			}
			BuildHeatmapPercentage = (i * 100) / jsonArrayX.Count;
		}
		foreach (GA_DataPoint dp in DPsToDelete)
		{
			DataContainer.Data.Remove(dp);
		}
		
		BuildingHeatmap = false;
		BuildHeatmapPercentage = 0;
		
		NormalizeDataPoints (DataContainer.Data);
		
		GetComponent<GA_HeatMapRenderer>().OnDataUpdate();
		Loading = false;
	}
#endif
	
	public void OnErrorDownload(string msg)
	{
		GA.Log("GameAnalytics: Download failed: "+msg);
		DownloadingData = false;
		Loading = false;
	}

#if UNITY_EDITOR || !UNITY_FLASH
	public void SetCombineHeatmapType(CombineHeatmapType combineType)
	{
		_combineType = combineType;
	}
#endif
	
	public void DownloadData()
	{
		DownloadingData = true;
		
		List<string> events = new List<string>();
		
		for(int i = 0; i<AvailableEvents.Count;i++)
		{
			if(CurrentEventFlag[i])
				events.Add(AvailableEvents[i]);
		}
		
		bool cancel = false;
		
		if(CurrentAreaIndex>=AvailableAreas.Count)
		{
			GA.LogWarning("GameAnalytics: Warning: Area selected is out of bounds. Not downloading data.");
			cancel = true;
		}
		
		string build = "";
		if (!IgnoreBuilds)
		{
			if(CurrentBuildIndex>=AvailableBuilds.Count)
			{
				GA.LogWarning("GameAnalytics: Warning: Build selected is out of bounds. Not downloading data.");
				cancel = true;
			}
			else
			{
				build = AvailableBuilds[CurrentBuildIndex];
			}
		}
		
		if (cancel)
		{
			DownloadingData = false;
			return;
		}
		
		
#if UNITY_EDITOR || !UNITY_FLASH
		GA.API.Request.RequestHeatmapData(events, AvailableAreas[CurrentAreaIndex], build,IgnoreDates?null:StartDateTime,IgnoreDates?null:EndDateTime, OnSuccessDownload, OnErrorDownload);
#endif
	}
	
	// Update is called once per frame
	public void DownloadUpdate ()
	{
		DownloadData();
	}
	
#if UNITY_EDITOR || !UNITY_FLASH
	public void OnSuccessDownloadInfo(GA_Request.RequestType requestType, Hashtable jsonList, GA_Request.SubmitErrorHandler errorEvent)
	{
		GA.Log("Succesful index downloaded");
		CurrentAreaIndex = 0;
		CurrentTypeIndex = 0;
		CurrentBuildIndex = 0;
		StartDateTime = DateTime.Now.AddDays(-14);
		EndDateTime = DateTime.Now;
		AvailableEvents = new List<string>();
		CurrentEventFlag = new List<bool>();
		AvailableAreas = new List<string>();
		AvailableBuilds = new List<string>();
		IgnoreDates = true;
	
		ArrayList jsonArrayEventID = (ArrayList)jsonList["event_id"];
		for (int i = 0; i < jsonArrayEventID.Count; i++)
		{
			try
			{
				string name;
				name = jsonArrayEventID[i].ToString();
				AvailableEvents.Add(name);	
				CurrentEventFlag.Add(false);
				
			}
			catch
			{
				// JSON format error
				GA.LogError("GameAnalytics: Error in parsing JSON data from server");
			}
		}
		
		ArrayList jsonArrayArea = (ArrayList)jsonList["area"];
		for (int j = 0; j < jsonArrayArea.Count; j++)
		{
			try
			{
				string name;
				name = jsonArrayArea[j].ToString();
				AvailableAreas.Add(name);		
				
			}
			catch
			{
				// JSON format error
				GA.LogError("GameAnalytics: Error in parsing JSON data from server");
			}
		}
		
		ArrayList jsonArrayBuild = (ArrayList)jsonList["build"];
		for (int k = 0; k < jsonArrayBuild.Count; k++)
		{
			try
			{
				string name;
				name = jsonArrayBuild[k].ToString();
				AvailableBuilds.Add(name);		
				
			}
			catch
			{
				// JSON format error
				GA.LogError("GameAnalytics: Error in parsing JSON data from server");
			}
		}
	}

	public void OnErrorDownloadInfo(string msg)
	{
		GA.Log("GameAnalytics: Download failed: "+msg);
	}
	
	public void UpdateIndexData ()
	{
		GA.Log("Downloading index from server");
		GA.API.Request.RequestGameInfo(OnSuccessDownloadInfo, OnErrorDownloadInfo);
	}
#endif
}