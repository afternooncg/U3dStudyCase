// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MyTest/ImgEffectWorldPosi"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TargetPosi("Target", Vector) = (0,0,0,1)

	}
	SubShader
	{
	  Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		// No culling or depth
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off 
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;				
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosi : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPosi = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _TargetPosi;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col = 1 - col;

				float FogRadius = 1;
				float alpha = clamp( length(i.worldPosi.xy - _TargetPosi.xy) -FogRadius, 0.0, FogRadius);
				col.a = alpha ;

				return col;
			}
			ENDCG
		}
	}
}
