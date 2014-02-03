Shader "Custom/GA_HeatMapTransparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaxRadius ("MaxRadius", Range(0.0,2.0)) = 1.0
		_ColZero ("ColZero", Color) = (1, 1, 1, .0)
		_Cold ("Cold", Color) = (.0, .0, .9, .0)
		_Warm ("Warm", Color) = (.9, .0, .0, .0)
		_RangeMin ("Minimum", Range(0.0, 1.0)) = 0.0
		_RangeWidth ("Width", Range(0.0, 1.0)) = 1.0
		_RangeZero ("Zero", Range(0.0, 1.0)) = 0.0
	} 
	
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
			
		Pass
		{
			ZTest LEqual
			Cull Back
			ZWrite Off
			Lighting Off 
			Fog { Mode off }
			AlphaTest Greater 0
			ColorMask RGBA
			
			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
			
			CGPROGRAM 
			
			#include "UnityCG.cginc" 
		    #pragma vertex vert
		    #pragma fragment frag
			
			float4 _ColZero; 
			float4 _Cold;  
			float4 _Warm; 
			float _MaxRadius;
			float _RangeMin;
			float _RangeWidth;
			float _RangeZero;
			
			float4 _MainTex_ST;
		 
			struct v2f {
					float4 pos : SV_POSITION;
					float2 uv_MainTex : TEXCOORD0;
					float3 normal : TEXCOORD1;
				};
			 
			v2f vert(appdata_full v)
			{
				v.vertex = mul(_Object2World, v.vertex);
				v.normal = normalize(_WorldSpaceCameraPos - (float3)v.vertex);
				float3 up = float3(0,1,0);
				float3 side = normalize(cross(v.normal, up));
				float radius = v.color.x;///2;
				float RangeMax = _RangeMin + _RangeWidth;
				if(radius >= _RangeMin && radius <= RangeMax )
				{
					up = normalize(cross(side, v.normal));
					v.vertex.xyz = v.vertex.xyz  + (v.texcoord.x * side + v.texcoord.y * up) * _MaxRadius; 
					v.vertex =  mul(_World2Object, v.vertex); 
				}
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = (float3)v.color;
				return o;
			}
			
			sampler2D _MainTex;
			float4 frag(v2f IN) : COLOR 
		 	{
				float r = length(IN.uv_MainTex.xy);
				half4 col = half4(0,0,0,0);
				float radius = IN.normal.x;
				float RangeMax = _RangeMin + _RangeWidth;
				//if(radius >= _RangeMin && radius <= RangeMax )
				//{
					if (_RangeZero > 0)
					{
						if (radius <= _RangeZero)
						{
							radius -= _RangeMin;
							//radius /= (RangeMax - _RangeMin);
							float newNorm = radius / _RangeZero;
							col = newNorm * _ColZero + (1.0-newNorm)*_Cold;
							col.a *= cos(max(r*3.14, 0.0));
						}
						else
						{
							radius /= RangeMax;
							//radius /= (RangeMax - _RangeMin);
							float newNorm = _RangeZero / radius;
							newNorm = newNorm;
							col = newNorm * _ColZero + (1.0-newNorm)*_Warm;
							col.a *= cos(max(r*3.14, 0.0));
						}
					}
					else
					{
						radius -= _RangeMin;
						radius /= (RangeMax - _RangeMin);
						col = radius * _Warm + (1.0-radius)*_Cold;
						col.a *= cos(max(r*3.14, 0.0));
					}
				//}
				return col;
			}
			ENDCG 
		}
	} 
	Fallback "Diffuse"
}