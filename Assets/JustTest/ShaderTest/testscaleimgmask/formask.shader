Shader "study/formask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("噪音", 2D) = "white" {}
 	    _Radiu("半径", Float) = 3.0
		_RadiuBlur("模糊半径", Float) = 0.5
		_Target("目标点", Vector) = (0,0,0,0)
	}
	SubShader
	{
      Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		
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
				float4 position_in_world_space : TEXCOORD1;
			};

			
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float _Radiu;
			float _RadiuBlur;
			float3 _Target;
			//uniform float4 _Points[100];  // 数组变量
			uniform float3 _Points[50];  // 数组变量
			uniform float _Points_Num;  // 数组长度变量
			
			float countalpha1(float2 pos, float2 target)
			{
				float attn = clamp(_Radiu - distance(pos,  target), 0.0, _Radiu);
				return 1.0/_RadiuBlur * attn /_Radiu;
				
			}

			float countalpha(float2 pos, float2 target, float3 nosie)
			{
				//float attn = clamp(_Radiu - distance(pos+nosie*5,  target), 0.0, _Radiu);
				float attn = clamp(_Radiu - distance(pos,  target), 0.0, _Radiu);
				return 1.0/_RadiuBlur * attn /_Radiu;
				
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.position_in_world_space = mul(unity_ObjectToWorld,v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				/*
				for (int j=0; j<_Points_Num; j++)
				{
				   float3 p4 = _Points[j]; // 索引取值
				   // 自定义处理
				     float dist = distance(i.position_in_world_space.xz,  p4.xz);
					if (dist < _Radiu)					
						col.a = col.a - 1;					
					else
						col.a = col.a+1;
				 
				}*/
				
				float _BlurPower=0.02;

				
				// 模糊算法
				half4 baseColor1 = tex2D (_MainTex, i.uv + float2(-_BlurPower, 0));
			half4 baseColor2 = tex2D (_MainTex, i.uv + float2(0, -_BlurPower));
			half4 baseColor3 = tex2D (_MainTex, i.uv + float2(_BlurPower, 0));
			half4 baseColor4 = tex2D (_MainTex, i.uv + float2(0, _BlurPower));
			half4 baseColor = 0.25 * (baseColor1 + baseColor2 + baseColor3 + baseColor4);
			
				/*	
				fixed noise = tex2D(_NoiseTex, i.uv).r;
				float4 viewCenter = float4(0,0,0,2);
				half2 AB = half2(viewCenter.z , viewCenter.z*0.02)*noise;
				half M = (i.uv.x - viewCenter.x)*(i.uv.x - viewCenter.x)/(AB.x*AB.x);
				half S = (i.uv.y - viewCenter.y)*(i.uv.y - viewCenter.y)/(AB.y*AB.y);
				half dist = M+S;
				half alpha = smoothstep(AB.x-viewCenter.w, AB.x + viewCenter.w, dist) * 0.5;
				*/

				fixed4 col1 = tex2D(_NoiseTex, i.uv);
				
				float alpha = 0;
				for (int j=0; j<_Points_Num; j++)
				{
				   float3 p4 = _Points[j]; // 索引取值
				   // 自定义处理
				   
					//alpha += countalpha1(i.position_in_world_space.xz,  _Points[j].xz); 
					alpha += countalpha(i.position_in_world_space.xz,  _Points[j].xz, col1.rgb); 
				}

				
				//col.rgba = float4(0,0,0,0.8);
				col.a = (1.0-alpha);
				/*
				if(col.a<1)	
				{	
					col.rgb = baseColor.rgb;						
					col.a = col.a - col1.r;
					}
					*/
				


				

    			
				
				return col;
			}
			ENDCG
		}
	}
}
