Shader "Custom/SunRay" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_BaseTex ("Base (RGBA)", 2D) = "black"
		_EmissionTex ("Emission Map (RGBA)", 2D) = "black"
	}
	SubShader {

		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 300
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert decal:blend

		sampler2D _BaseTex;
		sampler2D _EmissionTex;
		float4 _Color;
		float4 _EmissionColor;

		struct Input {
			float2 uv_BaseTex;
			float2 uv_EmissionTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			half4 baseTex = tex2D(_BaseTex, IN.uv_BaseTex) * _Color;
			o.Albedo = baseTex.rgb;
			
			half4 emissionTex = tex2D(_EmissionTex, IN.uv_EmissionTex) * _EmissionColor * _EmissionColor.a;
			o.Emission = emissionTex;
			
			o.Alpha = baseTex.a * emissionTex.a;
		}
		
		ENDCG 
	}
	FallBack "Diffuse"
}
