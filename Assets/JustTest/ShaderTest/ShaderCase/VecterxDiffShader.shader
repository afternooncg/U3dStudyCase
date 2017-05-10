// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Study/VecterxDiffShader"
{
	Properties
		{
			_Diffuse("Diffuse", Color) = (1,1,1,1)
		}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		Pass
		{
			Tags { "LightMode"="ForwardBase"}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			
			fixed4  _Diffuse;
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;			
				fixed3 color : color;
			};

			
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				//fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				fixed3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);

				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal , worldLight));

				o.color = ambient + diffuse;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(1.0,1.0,1.0,1.0);
				
				return fixed4 (i.color, 1.0);
			}
			ENDCG
		}
	}
}
