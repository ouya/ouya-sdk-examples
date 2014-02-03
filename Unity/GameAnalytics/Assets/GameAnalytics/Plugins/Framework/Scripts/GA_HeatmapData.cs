using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// G a_ heatmap data. Due to Unity watching serializeable properties we need to save the data on a component that isn't active. If we have a lot of data points Unity's watch will totally crash the game.
/// </summary>
public class GA_HeatmapData : MonoBehaviour
{
	public List<GA_DataPoint> Data;
}
