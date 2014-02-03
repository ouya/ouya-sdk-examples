using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GA_Histogram 
{
	public enum HistogramScale {Linear,Logarithmic};
	public float min = Mathf.Infinity;
	public float max = 0;
	public float RealDataMin;
	public float RealDataMax;
	
	public float[] Data;
	
	public void Recalculate(List<GA_DataPoint> data,int numChunks,HistogramScale scale)
	{
		GA.Log("GameAnalytics: Recalculating Histogram");
		
		min = Mathf.Infinity;
		max = 0;
		//if we have less data samples than numChunks, we need a smaller array
		List<int> sampleCount = new List<int>();
		//Find range
		foreach(GA_DataPoint c in data)
		{
			if(c.count<min)
				min = c.count;		
			if(c.count>max)
				max = c.count;	
			
			if(!sampleCount.Contains((int)c.count))
				sampleCount.Add((int)c.count);
		}
		RealDataMin = min;
		RealDataMax = max;
		
		//Calculate bucket sizes
		float interval = max-min;

		numChunks = Mathf.Min(sampleCount.Count,numChunks);
		Data = new float[numChunks];
		
		for(int i=0; i < numChunks; i++)
			Data[i]=0;
		
		//Put data into buckets
		foreach(GA_DataPoint p in data)
		{
			int c = (int)Mathf.FloorToInt(((p.count-min)/(interval))*numChunks);
			c = Mathf.Clamp(c,0,numChunks-1);
			
			Data[c]+=1f;

		}
		
		//Normalize data
		float countMax=-Mathf.Infinity,countMin=Mathf.Infinity,scaleValue=0;
		foreach(float d in Data)
		{
			
			//if(scale == HistogramScale.Linear)
				scaleValue = d;
			//GA_TODO: Enable log scale.
			//else 
			//	scaleValue = Mathf.Log(d);
			
			if(scaleValue<countMin)
				countMin = scaleValue;
			if(scaleValue>countMax)
				countMax = scaleValue;
			
		}
		float normValue = 0;
		for(int i=0; i < Data.Length; i++)
		{
			if(scale == HistogramScale.Linear)
				normValue = Data[i];
			else
				normValue = Mathf.Log(Data[i]);
			
			Data[i] = (normValue-countMin)/(countMax-countMin);
		}
	}
};
