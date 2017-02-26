Shader "CC2/Unlit_SingleTexture_Alpha" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput  o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			o.Emission = c.rgb*1.15;
			//o.Alpha = c.g;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
