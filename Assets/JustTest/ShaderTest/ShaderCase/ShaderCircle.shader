Shader "Study/ShaderCircle"
{
	
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


		
			fixed3 disk(fixed2 r,fixed2 center,fixed radius,fixed littleRadius,fixed3 cirlcecolor,fixed3 bgcolor)
            {
                    fixed3 col = bgcolor;
					float len = length(r - center);
					float w = (radius - littleRadius)*0.5;
					float countr = radius - w;
					
					//简单清晰的算法，有锯齿
					
                    if (abs(len - countr) < w)
                    {
                       col = cirlcecolor;
					   //col.x =lerp(0.5,1.0,abs(len - countr)*5);
					  // col.x =smoothstep(0.1,0.5,abs(len - countr)*15);

					 // col.x =lerp(abs(len - countr)*5,1.0,0.5);
                    }

                    return col;
            }


			fixed3 circle(fixed2 r,fixed2 center,fixed radius,fixed3 cirlcecolor,fixed3 bgcolor)
			{
				 fixed3 col = bgcolor;
				 float len = length(r - center);
				 if(len<(radius+0.02) && len>(radius-0.02))
					col =cirlcecolor;

				return col;
			}

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);						   
				o.uv = v.uv;
				return o;
			}
			

			//画单个圈
			fixed4 frag1 (v2f i) : SV_Target
			{				
				float tmp = sin(fmod(0.1*_Time.y, 3.14/5.0));
				fixed3 col = disk(i.uv,fixed2(0.5,0.5), tmp,0.8*tmp, fixed3(1.0,0,0), fixed3(1.0,1.0,1.0));
				//fixed3 col = fixed3(1.0,1.0,1.0);
				//col.x = lerp(0.0,10.0,i.uv.x*10)/10;

				return fixed4(col,1.0);
			}

			//Step画线
			fixed4 frag2 (v2f i) : SV_Target
			{				
				//float tmp = sin(fmod(0.1*_Time.y, 3.14/5.0));
				//fixed3 col = disk(i.uv,fixed2(0.5,0.5), tmp,0.8*tmp, fixed3(1.0,0,0), fixed3(1.0,1.0,1.0));
				//fixed3 col = fixed3(1.0,1.0,1.0);
				//col.x = lerp(0.0,10.0,i.uv.x*10)/10;
				fixed3 col = fixed3(1.0,1.0,1.0);
							
				//col = circle(i.uv, fixed2(0.5,0.5),0.5,fixed3(1.0,0,0), fixed3(1.0,1.0,1.0));

				
				//col.x = step(0.5,i.uv.x);   //屏幕对半

				//另一种画线方式 
				//float c1 = fmod(i.uv.x,0.1);
				//c1 = step(0.05,c1);
				//col.x = c1;


				
				

				return fixed4(col,1.0);
			}


			//smoothstep和lerp对比
			fixed4 frag3 (v2f i) : SV_Target
			{				
				
				fixed3 col = fixed3(1.0,1.0,1.0);
				
				if(i.uv.y<0.5)
					col.x = smoothstep(0, 1,i.uv.x);
				else
					col.x = lerp(0, 1,i.uv.x);
					
				return fixed4(col,1.0);
			}


			//Step画线加模糊边缘
			fixed4 frag4 (v2f i) : SV_Target
			{				
				
				fixed3 col = fixed3(1.0,1.0,1.0);
							
				
				//另一种画线方式 
				float c1 = fmod(i.uv.x,0.1);


				if(i.uv.y>0.5)
				{
					if(c1<0.05)
				{
					c1 = smoothstep(0.03,0.1, abs(c1-0.05*.5)*4);					
					col.x = c1;
				}
				//else
				//	col.y = 0.1;
				}
				else
				{
					if(c1<0.05)
				{
					//c1 = smoothstep(0.03,0.1, abs(c1-0.05*.5)*4);
					c1 = lerp(0.03,0.1, abs(c1-0.05*.5)*400);
					col.x = c1;
				}
				//else
				//	col.y = 0.1;
				}
				
					


				
				

				return fixed4(col,1.0);
			}


			  // 添加画圆盘的方法
                float disk(float2 r, float2 center, float radius) {
                float distanceFromCenter = length(r - center);
                float outsideOfDisk = smoothstep(radius - 0.005, radius + 0.005, distanceFromCenter);
                float insideOfDisk = 1.0 - outsideOfDisk;
                return insideOfDisk;
 
                }
			
			//lerp用于颜色	
			fixed4 frag (v2f i) : SV_Target
			{				
				
				fixed3 col = fixed3(1.0,1.0,1.0);
				
				fixed3 col1 = fixed3(0,1.0,1.0);

				fixed3 col2 = fixed3(1.0,0,1.0);


				if(i.uv.x < 0.4)
				{
					
					float d =  disk(i.uv, float2(0.5,0.5), 0.2);
					col = lerp(col, col1, d);
				
					d =  disk(i.uv, float2(0.3,0.3), 0.3);

					col = lerp(col, col2, d);  //覆盖

					col = lerp(col, col2, d) * col;  //混合

					


				}
				else if(i.uv.x < 0.6)
				{
					//白底只能用-
					col -=  disk(i.uv, float2(0.5,0.5), 0.2) * col1;

					col -=  disk(i.uv, float2(0.6,0.6), 0.3) * col2;
				
				}
				else				
				{
					col = fixed3(0,0,0);

					col +=  disk(i.uv, float2(0.5,0.5), 0.2) * col1;

					col +=  disk(i.uv, float2(0.6,0.6), 0.3) * col2;
				
				}


				

				return fixed4(col,1.0);
			}
			ENDCG
		}
	}
}


