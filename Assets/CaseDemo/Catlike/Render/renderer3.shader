Shader "CaseDemo/Catlike/renderer3"
{
	Properties
	{
		_Tint("Tint",Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags 
			{
			//	"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			//#include "UnityCG.cginc"
			#include "UnityStandardBRDF.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:Normal;
				
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal:TEXCOORD1;
				float4 vertex : SV_POSITION;				
				float3 worldPos : TEXCOORD2;
			};

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.normal = v.normal;
				//o.normal =  mul(unity_ObjectToWorld, float4(v.normal,0));

				
				o.normal = mul(
					transpose((float3x3)unity_WorldToObject),
					v.normal
				);
				
				//o.normal = UnityObjectToWorldNormal(v.normal); //上行等效果    法线 通过逆装置矩阵才能得到正确的值
				//o.normal = mul(v.normal,(float3x3)unity_WorldToObject);  //也等效，原因在于mul对于点参数的顺序不同，采用的行列矩阵计算格式不同

				/*
				如果先传入矩阵,再传入矢量  按 matrix * 列矩阵
				如果先传入矢量 按 行矩阵*列矩阵

				mul(m,v)  => m * v => 列矩阵
				mul(v,transpose(m))  =  (v行*transpose(m))TT  =   (m*v列)T =   真正得到结果数值是相同，多了个 T 只是影响行和列的区别，untiy会处理
				

				*/

				o.normal = normalize(o.normal);

				o.worldPos = mul(unity_ObjectToWorld,v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);			
				i.normal = normalize(i.normal);
				//return fixed4(i.normal*.5+0.5,1);
				//return  dot(float3(0, 1, 0), i.normal);
				float3 lightDir = _WorldSpaceLightPos0.xyz;		//系统变量 第1个光源位置
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);			//视线方向

				float3 lightColor = _LightColor0.rgb;
				float3 reflectionDir = reflect(-lightDir, i.normal);
				return float4(reflectionDir * 0.5 + 0.5, 1);

				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;	
				return fixed4(albedo * lightColor*saturate(dot(lightDir, i.normal)),1);  //返回0-1

			}
			ENDCG
		}
	}
}
