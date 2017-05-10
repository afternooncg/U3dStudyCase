// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Flag" {
Properties {
    _MainTex ("Texture", 2D) = "white" { }
}
SubShader 
{
    Pass 
    {
CGINCLUDE
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#pragma debug
	struct v2f {
	   fixed4  pos : SV_POSITION;
	   fixed2  uv : TEXCOORD;
	};
		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _MainTex_ST;
		v2f vert (appdata_base v)
		{
			fixed4 v2 = v.vertex;
			//v2.x += _SinTime[2] * (v2.y)*.5f;
			//v2.x += sin(_Time[2] + v2.y*2)*0.3f;
			v2.y += _SinTime[2] * (v2.y)*1.3f;
			//v2 += float4(v.normal *_SinTime[3] * .2 ,0);
			
			
		   v2f o;
		   o.pos = UnityObjectToClipPos (v2);
			
		   o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
		   return o;
		}
		ENDCG



		CGPROGRAM

		half4 frag (v2f i) : COLOR
		{
		   return tex2D (_MainTex, i.uv);
		}
		ENDCG

    }
}

Fallback "VertexLit"
} 
