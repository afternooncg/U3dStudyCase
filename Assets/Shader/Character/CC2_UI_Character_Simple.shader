Shader "CC2/CC2_UI_Character_Simple" {
	Properties
	{
		_MainTex("Base(RGB) Trans(A)", 2D) = "white" {}
		//_BlendMap("Emission", 2D) = "black" {}
		_SpecularMap("SpecularMap",2D) = "black"{}
		_Specular("Specular", Range(0, 1)) = 1
		_Albedo("Albedo", Range(0, 10)) = 1
		_RimColor("Rim Color", Color) = (0, 0, 0, 1)
		_RimPower("Rim Power", Range(0, 10)) = 1
	}

	SubShader
	{       Lighting Off
			Cull back
			Fog{ Mode Off }
		Tags
		{
			"Queue" = "Geometry+1"
			"RenderType" = "Opaque"
			"IgnoreProjector" = "True"
			}
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma surface surf CustomBlinnPhong nolightmap
        sampler2D _MainTex;
		//sampler2D _BlendMap;
		sampler2D _SpecularMap;
		fixed3 _RimColor;
		fixed _Specular;
		fixed _Albedo;
		fixed _RimPower;
		//float4 _LightDirection;
		struct Input
		{
			fixed2 uv_MainTex;
			fixed2 uv2_SpecularMap;
		};

		inline fixed4 LightingCustomBlinnPhong(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			fixed3 h = normalize(lightDir + viewDir);
			fixed diff = saturate(dot(s.Normal, lightDir));
			fixed nh = saturate(dot(s.Normal, h));
			fixed spec = pow(nh, s.Specular*512 ) * s.Gloss;
			fixed nv = pow(1 - saturate(dot(s.Normal, viewDir)), _RimPower);

			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten * 3.0);
			c.rgb += nv * _RimColor;
			c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten*2.6;

			return c;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{   
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			//fixed3 b = tex2D(_BlendMap, IN.uv_MainTex);
		    fixed3 s = tex2D(_SpecularMap, IN.uv2_SpecularMap);
		    s.rgb = lerp(c.rgb, s.rgb, 1);
			o.Albedo = c.rgb*_Specular*_Albedo;
			//o.Gloss = s.rgb * _Emission;
			//o.Specular = s.rgb*_Specular;
			o.Emission = s.rgb+c* _Specular;;//+ s.rgb * _Specular;
			}

		ENDCG
	}

	FallBack "Mobile/Diffuse"
}