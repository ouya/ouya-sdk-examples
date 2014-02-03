using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class GA_HeatMapDataFilterBase : MonoBehaviour
{
	public delegate void DataDownloadDelegate(GA_HeatMapDataFilterBase sender);
	public event DataDownloadDelegate OnDataUpdate;
	
	public abstract List<GA_DataPoint> GetData();
	
	public void OnDataUpdated()
	{
		if(OnDataUpdate != null)
			OnDataUpdate(this);
	}
}
