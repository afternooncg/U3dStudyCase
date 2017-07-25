Shader "Custom/FogOfWarMask" 
{
	Properties {
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurPower("BlurPower", float) = 0.002
	}


	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "LightMode"="ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		LOD 200
		

		Pass
		{
			CGPROGRAM
				
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _Color;

				struct appdata
				{
						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
						 float3 normal : NORMAL;
				};

				struct v2f
				{
						float2 uv : TEXCOORD0;
						float4 vertex : SV_POSITION;
						float3 normal:TEXCOORD1;
				};


				v2f vert(appdata v)
				{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;
						o.normal = v.normal;
						return o;
				}
				//返回固定颜色
				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 baseColor = tex2D(_MainTex, i.uv);
					return fixed4(baseColor.xyz, _Color.a - baseColor.g);
						//return fixed4(1.0, 0.0, 0.0, 0);									
				}

			ENDCG
		}
	}


	Fallback "Diffuse"
}
