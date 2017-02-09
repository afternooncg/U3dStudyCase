Shader "Custom/Water Flow" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Diffuse", Color) = (1,1,1,0.75)
		_MoveSpeedU ("U Move Speed", Range(-6,6)) = 0.5
		_MoveSpeedV ("V Move Speed", Range(-6,6)) = 0.5
	}
	SubShader {
		Tags {
				"IgnoreProjector"="True" 
				"Queue"="Transparent" 
				"RenderType"="Transparent"
		} 
		ZWrite Off
		LOD 200
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

		CGPROGRAM
		//#pragma surface surf Lambert
		#pragma vertex vert
		#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed4 _Color;
		fixed _MoveSpeedU;
		fixed _MoveSpeedV;

		//struct Input {
		//	float2 uv_MainTex;
		//};

		struct appdata
		{
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
			half4 vertcolor : COLOR0;
		};

		//void surf (Input IN, inout SurfaceOutput o) {
		//
		//	fixed2 MoveScrolledUV = IN.uv_MainTex;
		//	
		//	fixed MoveU = -_MoveSpeedU * _Time;
		//	fixed MoveV = -_MoveSpeedV * _Time;
		//	
		//	MoveScrolledUV += fixed2(MoveU, MoveV);
		//
		//	half4 c = tex2D (_MainTex, MoveScrolledUV);
		//	o.Albedo = c.rgb*_Color;
		//	o.Alpha = _Color.a;
		//}

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.vertcolor = v.color;

			UNITY_TRANSFER_FOG(o, o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			//fixed4 col = tex2D(_MainTex, i.uv);

			fixed MoveU = -_MoveSpeedU * _Time;
			fixed MoveV = -_MoveSpeedV * _Time;

			i.uv += fixed2(MoveU, MoveV);

			fixed4 c = tex2D(_MainTex, i.uv);
			c.rgb = c.rgb*_Color;
			c.a = c.a*i.vertcolor.a;

			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);

			return c;
		}

		ENDCG
		}
	}
	FallBack "Diffuse" 
}