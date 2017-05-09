// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CC2/Unlit_SingleTexture_Character" 
{
	Properties 
	{
		_MainTex ("Main Texture", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "Queue" = "Geometry+200""RenderType"="Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma exclude_renderers ps3 xbox360 flash
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform fixed4 _MainTex_ST;
			
			struct vertexInput
			{
				fixed4 vertex : POSITION; 
				fixed4 texcoord : TEXCOORD0;
			};
			
			struct fragmentInput
			{
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};
			
			fragmentInput vert( vertexInput i )
			{
				fragmentInput o;
				o.pos = UnityObjectToClipPos( i.vertex );
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				return o;
			}
			
			half4 frag( fragmentInput i ):COLOR
			{
				return tex2D( _MainTex,i.uv)*1.15;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
