Shader "CC2/Dissolve_Simple" {
	Properties{
		_BurnAmount("Burn Amount", Range(0.0, 1.0)) = 0.0
		_EdgeWidth("Burn Edge Width", Range(0.0, 0.5)) = 0.1
		_MainTex("Base (RGB)", 2D) = "white" {}
	    _BurnFirstColor("Burn First Color", Color) = (1, 0, 0, 1)
		_BurnSecondColor("Burn Second Color", Color) = (1, 0, 0, 1)
		_ColorAmount("Color Amount", Range(0.1, 8.0)) = 1.0
		_BurnMap("Burn Map", 2D) = "white"{}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

		Pass{
		Tags{ "LightMode" = "ForwardBase" }

		Cull Off

		CGPROGRAM

#include "Lighting.cginc"
#include "AutoLight.cginc"

#pragma multi_compile_fwdbase

#pragma vertex vert
#pragma fragment frag

	fixed _BurnAmount;
	fixed _EdgeWidth;
	sampler2D _MainTex;
	fixed4 _BurnFirstColor;
	fixed4 _BurnSecondColor;
	fixed _ColorAmount;
	sampler2D _BurnMap;

	float4 _MainTex_ST;
	float4 _BurnMap_ST;

	struct a2v {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uvMainTex : TEXCOORD0;
		float2 uvBurnMap : TEXCOORD1;
		};

	v2f vert(a2v v) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uvMainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.uvBurnMap = TRANSFORM_TEX(v.texcoord, _BurnMap);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
	fixed3 burn = tex2D(_BurnMap, i.uvBurnMap).rgb;
	clip(burn.r - _BurnAmount);
	fixed3 albedo = tex2D(_MainTex, i.uvMainTex).rgb;
	fixed3 diffuse = albedo*1.2;

	fixed t = 1 - smoothstep(0.0, _EdgeWidth, burn.r - _BurnAmount);
	fixed3 burnColor = lerp(_BurnSecondColor,_BurnFirstColor, t);
	burnColor = pow(burnColor, _ColorAmount);

	//fixed3 finalColor = lerp(diffuse, burnColor, t * step(0.0001, _BurnAmount));
	fixed3 finalColor = diffuse+burnColor*0.8;
	return fixed4(finalColor, 1);
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}