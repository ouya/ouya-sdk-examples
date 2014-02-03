Shader "Custom/GA_HeatMapSolidOverlay"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaxRadius ("MaxRadius", Range(0.0,2.0)) = 1.0 
		_Cold ("Cold", Color) = (.0, .0, .9, .0)
		_Warm ("Warm", Color) = (.9, .0, .0, .0)
		_RangeMin ("Minimum", Range(0.0, 1.0)) = 0.0
		_RangeWidth ("Width", Range(0.0, 1.0)) = 1.0
	} 
	
	SubShader
	{
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		Pass
		{
	        ZTest Off
			Cull Back
			ZWrite On
			Lighting Off 
			Fog { Mode off }
			AlphaTest GEqual 0.005
			ColorMask RGBA
			
			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
			
			CGPROGRAM 
			
			#include "UnityCG.cginc" 
		    #pragma vertex vert
		    #pragma fragment frag
		    
			float4 _Cold;  
			float4 _Warm; 
			float _MaxRadius;
			float _RangeMin;
			float _RangeWidth;
			float4 _MainTex_ST;
		   
			struct v2f {
					float4 pos : SV_POSITION;
					float2 uv_MainTex : TEXCOORD0;
					float3 normal : TEXCOORD1;
				};
			 
			v2f vert(appdata_full v)
			{
				v.vertex = mul(_Object2World, v.vertex);
				v.normal = normalize(_WorldSpaceCameraPos - v.vertex);
				float3 up = float3(0,1,0);
				float3 side = normalize(cross(v.normal, up));
				
				//Density - [0-1]
				float size = v.color.x; 
				
				//Scale to range
				size -= _RangeMin;
				size /= _RangeWidth;
				
				//Clamp to range
				size = min(size, 1.0);
				size = max(size, 0.0);
				
				up = normalize(cross(side, v.normal));
				
				if(size > 0.0 && size < 1.0)
				{
					v.vertex.xyz = v.vertex.xyz  + (v.texcoord.x * side + v.texcoord.y * up) * max(size * _MaxRadius, 0.1) * 3; 
					v.vertex =  mul(_World2Object, v.vertex); 
				}
				
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = v.texcoord;
				//Pass radius to frag shader through normal X component [0-1]
				o.normal.x = size;
				return o;
			}
			
			sampler2D _MainTex;
			float4 frag(v2f IN) : COLOR 
		 	{
				half4 col = half4(0,0,0,0);
				
				//Radius [0-1]
				half radius = length(IN.uv_MainTex.xy) * 2.0;
				//Only draw in circle
				if(radius <= 1.0 )
				{
					//Find base color (Opaque)
					_Warm.a = _Cold.a = 1.0;
					col = IN.normal.x * _Warm + (1.0-IN.normal.x) * _Cold;
					
					//Find normal of point which happens to be point
					half3 n;
					n.xy = IN.uv_MainTex.xy;
					n.z = sqrt(n.x*n.x + n.y*n.y - 0.5);
					n = normalize(n);
					
					//Shade according to dotproduct with ambient contribution
					half a = 0.3;
					col *= (1.0-a)*dot(normalize(n), normalize(half3(1.5,1.5,0.5))) + a;
					col.a = 1.0;
				}
				return col;
			}
			ENDCG
		
		}
	
		
		Pass
		{
			ZTest LEqual
			Cull Front
			ZWrite On
			Lighting Off 
			Fog { Mode off }
			AlphaTest GEqual 0.005
			ColorMask RGBA
			
			
			CGPROGRAM 
			
			#include "UnityCG.cginc" 
		    #pragma vertex vert
		    #pragma fragment frag
		    
			
			float4 _Cold;  
			float4 _Warm; 
			float _MaxRadius;
			float _RangeMin;
			float _RangeWidth;
			float4 _MainTex_ST;
		   
			struct v2f {
					float4 pos : SV_POSITION;
					float2 uv_MainTex : TEXCOORD0;
					float3 normal : TEXCOORD1;
				};
			 
			v2f vert(appdata_full v)
			{
				v.vertex = mul(_Object2World, v.vertex);
				v.normal = normalize(_WorldSpaceCameraPos - v.vertex);
				float3 up = float3(0,1,0);
				float3 side = normalize(cross(v.normal, up));
				
				//Density - [0-1]
				float size = v.color.x; 
				
				//Scale to range
				size -= _RangeMin;
				size /= _RangeWidth;
				
				//Clamp to range
				size = min(size, 1.0);
				size = max(size, 0.0);
				
				up = normalize(cross(side, v.normal));
				
				if(size > 0.0 && size < 1.0)
				{
					v.vertex.xyz = v.vertex.xyz  + (v.texcoord.x * side + v.texcoord.y * up) * max(size * _MaxRadius, 0.1); 
					v.vertex =  mul(_World2Object, v.vertex); 
				}
				
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = v.texcoord;
				//Pass radius to frag shader through normal X component [0-1]
				o.normal.x = size;
				return o;
			}
			
			sampler2D _MainTex;
			float4 frag(v2f IN) : COLOR 
		 	{
				half4 col = half4(0,0,0,0);
				
				//Radius [0-1]
				half radius = length(IN.uv_MainTex.xy) * 2.0;
				//Only draw in circle
				if(radius <= 1.0 )
				{
					//Find base color (Opaque)
					_Warm.a = _Cold.a = 1.0;
					col = IN.normal.x * _Warm + (1.0-IN.normal.x) * _Cold;
					
					//Find normal of point which happens to be point
					half3 n;
					n.xy = IN.uv_MainTex.xy;
					n.z = sqrt(n.x*n.x + n.y*n.y - 0.5);
					n = normalize(n);
					
					//Shade according to dotproduct with ambient contribution
					half a = 0.3;
					col *= (1.0-a)*dot(normalize(n), normalize(half3(1.5,1.5,0.5))) + a;
					col.a = 1.0;
				}
				return col;
			}
			ENDCG  
		}
	} 
	Fallback "Diffuse"
}