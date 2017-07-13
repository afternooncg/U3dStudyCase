Shader "CatLike/rendererTestNormal"
{
	Properties
	{
		_Tint ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		_SpecularTint ("Specular", Color) = (0.5, 0.5, 0.5)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags {
				"LightMode" = "ForwardBase"     //有了这个标志，才能调用灯光颜色
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
			//#include "UnityCG.cginc"
			#include "UnityStandardBRDF.cginc"

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
				float3 normal:TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 color: TEXCOODR3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _Tint;
			float _Smoothness;
			float3 _SpecularTint;



			//============================================================================
			//shader概要提供的逐顶点光照
			v2f vert2 (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(UnityObjectToWorldNormal(v.normal));
				
				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex);

				float3 lightdir = _WorldSpaceLightPos0.xyz - o.worldPos;
				float3 viewdir = normalize( _WorldSpaceCameraPos.xyz - o.worldPos);

				//o.color = UNITY_LIGHTMODEL_AMBIENT.xyz + DotClamped(lightdir, o.normal);
				o.color = UNITY_LIGHTMODEL_AMBIENT.xyz + pow(DotClamped(viewdir, o.normal),2);

				return o;
			}
			
			fixed4 frag2(v2f i) : SV_Target
			{
				return fixed4(i.color,1);
			}


			//============================================================================
			//shader概要提供的逐像素点光照
			v2f vert3 (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(UnityObjectToWorldNormal(v.normal));				
				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex);
				

				return o;
			}
			
			fixed4 frag3(v2f i) : SV_Target
			{
				float3 lightdir = _WorldSpaceLightPos0.xyz - i.worldPos;
				float3 viewdir = normalize( _WorldSpaceCameraPos.xyz - i.worldPos);

				i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + DotClamped(lightdir, i.normal);
				//i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + pow(DotClamped(viewdir, i.normal),2);
				return fixed4(i.color,1);
			}


			//============================================================================
			//shader概要提供的halfLambert+高光逐像素点光照
			v2f vert4 (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(UnityObjectToWorldNormal(v.normal));				
				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex);
				

				return o;
			}


			
			fixed4 frag4(v2f i) : SV_Target
			{
				float3 lightdir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
				float3 viewdir = normalize( _WorldSpaceCameraPos.xyz - i.worldPos);
				float3 halfLambert = dot(i.normal, lightdir)*0.5+0.5;
				i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + pow(DotClamped(halfLambert, lightdir),5);


				float3 reflectdir = normalize(reflect(-lightdir,i.normal));
				//i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + DotClamped(lightdir, halfLambert);  
				float3 specular = pow(DotClamped(viewdir, reflectdir),10);
				i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + float3(0,1,0)*DotClamped(lightdir, halfLambert)  + float3(1,0,0)*specular; 
				return fixed4(i.color,1);
			}

			//============================================================================
			//shader概要提供的BlingPhone高光 逐像素点光照
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(UnityObjectToWorldNormal(v.normal));				
				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex);
				

				return o;
			}


			
			fixed4 frag(v2f i) : SV_Target
			{
				float3 lightdir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
				float3 viewdir = normalize( _WorldSpaceCameraPos.xyz - i.worldPos);
				float3 halfLambert = dot(viewdir, lightdir)*0.5+0.5;
				i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + pow(DotClamped(halfLambert, lightdir),5);


				float3 reflectdir = normalize(reflect(-lightdir,i.normal));
				//i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + DotClamped(i.normal, halfLambert);  
				float3 specular = pow(DotClamped(viewdir, reflectdir),10);
				i.color = UNITY_LIGHTMODEL_AMBIENT.xyz + float3(0,1,0)*DotClamped(lightdir, halfLambert)  + float3(1,0,0)*specular; 
				return fixed4(i.color,1);
			}

			
			v2f vert1 (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				//o.normal = v.normal; //两个一样
				o.normal = mul(unity_ObjectToWorld, v.normal);

				//o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
				o.normal = mul(
					transpose((float3x3)unity_WorldToObject), float4(v.normal, 0));
				 
				o.normal = normalize(o.normal);
				
				o.worldPos = mul((float3x3)unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			fixed4 frag1 (v2f i) : SV_Target
			{
				// sample the texture

				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = fixed4(normalize(i.normal),1);
				//return col;

				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;

				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

				float3 reflectionDir = reflect(-lightDir, i.normal);

				float3 halfVector = normalize(lightDir + viewDir);

				float3 specular = _SpecularTint*pow(DotClamped(halfVector, i.normal), _Smoothness*100);
				
				

				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
				float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);


				return fixed4(diffuse+specular,1);

				return pow(DotClamped(viewDir, reflectionDir), _Smoothness*10);
				return float4(reflectionDir * 0.5 + 0.5, 1);

				return float4(diffuse, 1);
				
			}
			ENDCG
		}
	}
}
