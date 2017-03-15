// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Study/ShaderViewNoraml"
{
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;				
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;			
				float4 vertex : SV_POSITION;
				float4 vertexLocal : TEXCOORD1;
				float3 normal:NORMAL;
				float4 spec:TEXCOORD2;
			};

		
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				
				o.uv = v.uv;
				//o.vertexLocal = mul(unity_ObjectToWorld, v.vertex); //v.vertex;
				o.vertexLocal = mul(UNITY_MATRIX_MV, v.vertex); 
				
				o.normal = o.normal;


				float3 viewNormal   =   mul((float3x3)UNITY_MATRIX_MV, v.normal); //mul(unity_ObjectToWorld, v.normal);//
				float4 viewPos      = mul(UNITY_MATRIX_MV, v.vertex);
				float3 viewDir      = float3(0.0,1);
				float3 viewLightPos = float3(0,0 ,0);
              
				float3 dirToLight = viewPos.xyz - viewLightPos;
              
				float3 h = viewDir + normalize(-dirToLight);
				o.spec   = float4(1.0,0,0,1.0) * pow(saturate(dot(viewNormal, normalize(h))), 89);
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				
				fixed4 col = fixed4(1.0,1.0,1.0,1.0);
				//col.x = fixed4(i.vertex.x);
				//col.x = i.vertexLocal.x;
				//col.x = i.uv.x;
				col = fixed4(i.uv.x,i.uv.x,i.uv.x,1.0);
				col = fixed4(i.normal.x,i.normal.y,i.normal.z,1.0);
				//col = fixed4(i.vertex.z,i.vertex.z,i.vertex.z,1.0);
				
				
				
				//return float4(i.normal*0.5+0.5, 1.0); //全是灰色
				return i.spec;
			}
			ENDCG
		}
	}
}
