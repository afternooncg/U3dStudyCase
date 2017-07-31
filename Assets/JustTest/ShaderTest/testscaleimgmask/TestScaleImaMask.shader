Shader "Study/TestScaleImaMask"
{
	Properties
	{
		_MaskTex ("Mask", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
		_alphavar("alpha", Range (0,1.0)) = 0.5
		_openBit("openBit", int) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;				
				float4 vertex : SV_POSITION;
			};

			sampler2D _MaskTex;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _alphavar;
			int _openBit;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float _BlurPower=0.02;
				fixed4 col = tex2D(_MainTex, i.uv);
				
				


				fixed4 mask = tex2D(_MaskTex,i.uv);
				/*
				if(mask.r < _alphavar)
					col = fixed4(0,0,0,0);
				*/
				//if(abs((mask.r - (_openBit/256))) <= 0.001)
				//	col = fixed4(0,0,0,0);
			

				int xx = (int)(mask.r*100000*256);
				int ssss = xx%100000;
				if( ((ssss)&_openBit)==0)
					col = fixed4(0,0,0,0);
				return col;
				
					
			}
			ENDCG
		}
	}
}
