Shader "Study/ShaderScreenParam"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				float4 scr: TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);		
				o.scr = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				//fixed4 col = fixed4(normalize(fixed3(_ScreenParams.xy,0)),0);
				//fixed4 col = fixed4(_ScreenParams.xy,0,0); //屏幕长宽
				//fixed4 col = fixed4(scr.xy,0,0);	//屏幕坐标 0,0左上角
				//fixed4 col = fixed4(scr.y/_ScreenParams.y,0,0,0);      
				//fixed4 col = fixed4(scr.x/_ScreenParams.x,0,0,0);      
				fixed4 col = fixed4(i.scr.x/i.scr.w,0,0,0);      
				
				return col;
			}

			
			fixed4 frag (float4 scr:WPOS) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				//fixed4 col = fixed4(normalize(fixed3(_ScreenParams.xy,0)),0);
				//fixed4 col = fixed4(_ScreenParams.xy,0,0); //屏幕长宽
				//fixed4 col = fixed4(scr.xy,0,0);	//屏幕坐标 0,0左上角
				//fixed4 col = fixed4(scr.y/_ScreenParams.y,0,0,0);      
				fixed4 col = fixed4(scr.y/_ScreenParams.y,0,0,0);      			
				
				return col;
			}
			ENDCG
		}
	}
}
