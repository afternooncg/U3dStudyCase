// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CaseDemo/Catlike/renderer1"
{	
	Properties
	{
		_Tint("Tint",Color) = (1,1,1,1)
		_MainTexture("Textrue", 2D) = "white" {}
	}


	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTexture;
			float4 _MainTexture_ST;

/*
			float4 MyVertexProgram (float4 position : POSITION,	 out float3 localPosition:TEXCOORD0) : SV_POSITION {
				localPosition = position.xyz;
				return UnityObjectToClipPos(position);
			}

			float4 MyFragmentProgram (float4 position:SV_POSITION,
			float3 localPosition : TEXCOORD0
			): SV_TARGET {
				
				return float4(localPosition,1);
				//return _Tint;				
			}
*/

		    struct vi
			{
				float4 position: POSITION;
				float2 uv:TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram (vi input) {
				Interpolators i;
				//i.uv = TRANSFORM_TEX(input.uv, _MainTexture);   //包括 scale和offset
				i.uv = input.uv * _MainTexture_ST.xy + _MainTexture_ST.zw;  //等同于上1行
				i.position = UnityObjectToClipPos(input.position);
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				return tex2D(_MainTexture, i.uv)*_Tint;
				
			}

			ENDCG
		}		
	}
}
