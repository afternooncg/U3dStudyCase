Shader "MyTest/TestQueueSphere"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		//Tags { "RenderType"="Opaque" }
		Tags { "RenderType"="Opaque" "Queue" = "Geometry+100"  "IgnoreProjector" = "True" }
		
		LOD 200
		 
		// Pass {
		//Tags{ "Queue" = "Geometry+100" }
	    ZWrite on	 
		ZTest on
		//}


		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
