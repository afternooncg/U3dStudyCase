Shader "Unlit/TestShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_centerX("CenterX",Range(0,1)) = 0
		_centerY("CenterY",Range(0,1)) = 0
		_time("time",float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		//Tags { "Queue"="Transparent" }  
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma alpha  
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _centerX;
			float _centerY;
			float _time;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv );

				//float d = length(i.uv - float2(_centerX,_centerY));
				float d = length(i.uv - float2(0.5,0.5));
				//缩小作用...
				//if(d<0.3)					
				//	col= tex2D(_MainTex, i.uv + (i.uv - float2(_centerX,_centerY)) );
				float t = _centerX/2;
				if((d<(_centerX+0.01) && d>_centerX) || (d<(t+0.01) && d>t))
					col= tex2D(_MainTex, i.uv + (0.01,0.01));
				else
				{
					float a = i.uv.x + sin(_time);
					float b = i.uv.y + cos(_time);
					if(a<0)
						a=1;
					else if(a>1)
						a=0;

					if(b<0)
						b=1;
					else if(b>1)
						b=0;
					col =  tex2D(_MainTex, float2(a, b ));
				}
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
