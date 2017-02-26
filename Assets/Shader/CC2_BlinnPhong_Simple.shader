Shader "CC2/CC2_BlinnPhong_Simple" {
	Properties
	{
		_MainTex("Base(RGB) Gloss(A)", 2D) = "white" {}
	    //_SpecularTex("Base(RGB) Gloss(A)", 2D) = "white" {}
	    _Specular("Specular", Range(0, 10)) = 1
		_Shininess("Shininess", Range(0.01, 1)) = 0.5
	}

		SubShader
	{
		Pass
	{
		CGPROGRAM
#include "UnityCG.cginc"
#include "Lighting.cginc"
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_fwdbase

		sampler2D _MainTex;
	fixed4 _MainTex_ST;
	fixed _Specular;
	fixed _Shininess;

	struct a2v
	{
		fixed4 vertex : POSITION;
		fixed3 normal : NORMAL;
		fixed4 tangent : TANGENT;
		fixed4 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed3 normal : TEXCOORD1;
		fixed3 viewDir : TEXCOORD2;
		fixed3 lightDir : TEXCOORD3;
	};

	v2f vert(a2v v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

		TANGENT_SPACE_ROTATION;
		o.normal = mul(rotation, v.normal);
		o.viewDir = normalize(mul(rotation, ObjSpaceViewDir(v.vertex)));
		o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 c = tex2D(_MainTex, i.uv.xy);
	fixed4 o = c;

	fixed nh = saturate(dot(i.normal, normalize(i.viewDir + i.lightDir)));
	fixed3 spec = _LightColor0.rgb * pow(nh, _Shininess * 128) * _Specular;
	o.rgb += spec * c.a;

	return o;
	}

		ENDCG
	}
	}

		FallBack "Mobile/Diffuse"
}