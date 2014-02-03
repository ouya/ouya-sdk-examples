using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class GA_HeatMapRenderer : MonoBehaviour
{
	//Points are just vertice points colored from blue to red based on density
	//Billboards are triangles that are expanded from shader and colored based on density 
	
	public enum RenderModel { Transparent, TransparentOverlay } //, Solid, SolidOverlay }
	public RenderModel CurrentRenderModel = RenderModel.TransparentOverlay;
	
	public Color MinColor = Color.yellow;
	public Color MaxColor = Color.red;
	public float RangeMin = 0.0f;
	public float RangeMax = 1.0f;
	public float MaxRadius = 1.0f;
	
	public bool ShowValueLabels;
	
	public GA_Histogram Histogram;
	public GA_Histogram.HistogramScale HistogramScale;
	
	public GA_HeatMapDataFilterBase datafilter;
	
	public GameObject BillBoard;
	
	List<GameObject> DestroyList;
	
	void OnEnable()
	{
		datafilter = GetComponent<GA_HeatMapDataFilterBase>();
	}
	//Called from datafilter
	public void OnDataUpdate()
	{
		//Put old meshes on destroy list
		if(DestroyList == null)
		{
			DestroyList = new List<GameObject>();
		}
		for(int i = 0; i < transform.childCount; i++)
		{
			GameObject go = transform.GetChild(i).gameObject;
			if(!DestroyList.Contains(go) && go.GetComponent<GA_HeatmapData>() == null && go.name.Contains("GA_"))
			{
				DestroyList.Add(go);
			}
		}
		
		RenderModelChanged ();
		 
		//Create meshes
		
		if(datafilter == null)
		{
			datafilter = GetComponent<GA_HeatMapDataFilterBase>();
		}
		
		if(datafilter != null)
		{
			var data = datafilter.GetData();
			RecalculateHistogram();
			
			if(data == null || data.Count==0)
			{
				GA.Log ("GameAnalytics: No data to create heatmap. Returning.");
				return;
			}
		   	createBillboards(data);
		}
		else
		{
			GA.Log ("GameAnalytics: GA_HeatMapDataFilterBase component missing.");
		}
		
	}
	
	void createBillboards(List<GA_DataPoint> data)
	{
		GA.Log("GameAnalytics: Creating mesh billboards for heapmap");
		
		if(data != null)
		{
			int datapoints = data.Count;
			
			int index = 0;		
			
			while(datapoints > 0)
			{
				int chunk = datapoints;
				int limit = (int)Mathf.Floor(65000.0f / 3);
				if (chunk > limit)
				{
					chunk = limit;
				}
				BillBoard = new GameObject("GA_Billboards-" + index.ToString() + "-" + (index+chunk).ToString(), typeof(MeshFilter), typeof(MeshRenderer));
				
				
				BillBoard.transform.parent = transform;
				BillBoard.transform.localPosition = Vector3.zero;
				
				Vector3[] vertices = new Vector3[chunk*3];
				Vector3[] normals = new Vector3[chunk*3];
				Vector2[] uvs = new Vector2[chunk*3];
				Color[] colors = new Color[chunk*3];
				int[] triangles = new int[chunk*3];
				
				for(int i=0;i<chunk; i++)
				{
					for(int j = 0; j < 3; j++)
					{
						vertices[3*i +  j] = data[index].position;
						colors[3*i + j] = new Color(data[index].density, 0.0f, 0.0f,1.0f);
						float deg =j*Mathf.PI*2.0f/3.0f; 
						
						//First component points up
						uvs[3*i +  j] = new Vector2(Mathf.Cos(deg),Mathf.Sin(deg));
						
						normals[3*i +  j] = Vector3.zero;
					}
					triangles[i*3 +  0] = i*3 + 0; 
					triangles[i*3 +  1] = i*3 + 2;
					triangles[i*3 +  2] = i*3 + 1;
					
					//Just a random variable used to blend rotation of billboards 
					index++;
				}
				
				MeshFilter mf = BillBoard.GetComponent<MeshFilter>();
				Mesh mesh = new Mesh();
				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.uv = uvs;
				mesh.colors = colors;
				mesh.triangles = triangles;
				mf.sharedMesh = mesh;
				mf.sharedMesh.RecalculateBounds();
				
				//MeshRenderer mr = go.GetComponent<MeshRenderer>();
				//mr.sharedMaterial = material;
				
				datapoints -= chunk;
				SetMaterialVariables();
			}
		}
	}
	
	public void SetMaterialVariables()
	{
		foreach(MeshRenderer m in gameObject.GetComponentsInChildren<MeshRenderer>())
		{
			if(m.gameObject.name.Contains("GA_Billboards"))
			{
				if(m.sharedMaterial == null)
				{
					m.sharedMaterial = new Material(Shader.Find("Custom/GA_HeatMapSolid"));
				}
				switch(CurrentRenderModel)
				{
					/*case RenderModel.Solid:
						m.sharedMaterial.shader = Shader.Find("Custom/GA_HeatMapSolid");
						break;
					case RenderModel.SolidOverlay:
						m.sharedMaterial.shader = Shader.Find("Custom/GA_HeatMapSolidOverlay");
						break;*/
					case RenderModel.TransparentOverlay:
						m.sharedMaterial.shader = Shader.Find("Custom/GA_HeatMapTransparentOverlay");
						break;
					default:
						m.sharedMaterial.shader = Shader.Find("Custom/GA_HeatMapTransparent");
						break;
				}
				
				m.sharedMaterial.SetColor("_Cold", MinColor);
				m.sharedMaterial.SetColor("_Warm", MaxColor);
				m.sharedMaterial.SetFloat("_RangeMin", RangeMin);
				m.sharedMaterial.SetFloat("_RangeWidth", RangeMax-RangeMin);
				m.sharedMaterial.SetFloat("_MaxRadius", MaxRadius);
				
				float zeroNorm = (0 - Histogram.RealDataMin) / (Histogram.RealDataMax - Histogram.RealDataMin);
				m.sharedMaterial.SetFloat("_RangeZero", zeroNorm);
				m.sharedMaterial.SetColor("_ColZero", Color.white);
			}
		}
	}
	
	public void RecalculateHistogram ()
	{
		if(datafilter != null)
		{
			var data = datafilter.GetData();
			if(data != null)
			Histogram.Recalculate(data,40,HistogramScale);
		}
	}

	public void RenderModelChanged ()
	{
		SetMaterialVariables();
	}

	public void OnScaleChanged ()
	{
		//GA_TODO: change scale on particles
		RecalculateHistogram();
	}
	
	void Update()
	{
		//Destroy gameobjects that need destroying
		if(DestroyList != null)
		{
			foreach(GameObject go in DestroyList)
			{
				if(Application.isPlaying)
				{
					Destroy(go);
				}
				else
				{
#if UNITY_EDITOR
					Editor.DestroyImmediate(go);
#endif
				}
			}
		}
	}
	
}
