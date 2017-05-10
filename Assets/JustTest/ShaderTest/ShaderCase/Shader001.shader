// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/Shader001"
{
	SubShader        
	{	
			// No culling or depth
			// Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
				#pragma vertex vert1
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
				};


				v2f vert1(appdata v)
				{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;
						return o;
				}
				//返回固定颜色
				fixed4 frag1(v2f i) : SV_Target
				{
						return fixed4(1.0, 1.0, 0.5, 1.0);									
				}
					
				fixed4 frag2(v2f i) : SV_Target
				{  
					fixed3 color1 = fixed3(0.886, 0.576, 0.898);
					fixed3 color2 = fixed3(0.537, 0.741, 0.408);
					fixed3 pixel;
						//屏幕的坐标范围为0~1
							if(i.uv.x > 0.5)
							{
								if(i.uv.y > 0.5)
									pixel = color2;
								else
									pixel = fixed3(1.0, 1.0, 0.5);

							}
							 else 
							{
								pixel = color1;
							}
							return fixed4(pixel, 0.5f);
				}
								
				//用_ScreenParam访问屏幕
				fixed4 frag3(v2f i) : SV_Target							
				{		
				 fixed3 color1 = fixed3(0.886, 0.576, 0.898);
				fixed3 color2 = fixed3(0.537, 0.741, 0.408);
				fixed3 pixel;
				float dist = 200;
					//屏幕的坐标范围为0~1
						if(i.uv.x * _ScreenParams.x >  dist)
						{
							if(i.uv.y > 0.5)
								pixel = color2;
							else
								pixel = fixed3(1.0, 1.0, 0.5);

						}
						 else 
						{
							pixel = color1;
						}
						return fixed4(pixel, 0.5f);
						}	
						
											
				//画线	
				fixed4 frag4(v2f i) : SV_Target
				{	
				   fixed3 backgroundColor = fixed3(1.0, 1.0, 1.0);
					fixed3 color1 = fixed3(0.216, 0.471, 0.698); // blue
					fixed3 color2 = fixed3(1.00, 0.329, 0.298); // red
					fixed3 color3 = fixed3(0.867, 0.910, 0.247); // yellow
					 
					fixed3 pixel = backgroundColor;
					 
					// line1
					float leftCoord = 0.54;
					float rightCoord = 0.55;
					if (i.uv.x < rightCoord && i.uv.x > leftCoord) pixel = color1;
					 
					// line2
					float lineCoordinate = 0.4;
					float lineThickness = 0.003;
					if (abs(i.uv.x - lineCoordinate) < lineThickness) pixel = color2;
					 
					// line3
					if (abs(i.uv.y - 0.6) < 0.01) pixel = color3;
					 
					return fixed4(pixel, 1.0);
				}			
				
				
				 float mod(float a,float b)
                {//求余数  可用系统函数fmod代替
                        //floor(x)方法是Cg语言内置的方法，返回小于x的最大的整数
                        return a - b*floor(a / b);
                }
				
				//画刻度线
				fixed4 frag5(v2f i) : SV_Target
				{
					fixed3 color = fixed3(1.0, 1.0, 1.0);
					//if(mod(i.uv.x, 0.2) < 0.02)
					//if( floor(i.uv.x/0.1) == 1)
					if(fmod(i.uv.x,0.1) <= 0.05)
						color.x = 0.5f;
					return fixed4(color,1.0f);
				}			
				
				
				//根据半径，原点、原点的偏移量r和颜色来绘制圆盘        

                fixed3 disk(fixed2 r,fixed2 center,fixed radius,fixed3 cirlcecolor,fixed3 bgcolor)
                {
                        fixed3 col = bgcolor;
                        if (length(r - center) < radius)
                        {
                                col = cirlcecolor;
                        }
                        return col;
                }



				//画圆盘
				fixed4 frag6(v2f i) : SV_Target
				{
					fixed3 color = fixed3(1.0, 1.0, 1.0);
					
					color = disk(i.uv, fixed2(0.5,0.5), 0.2,  fixed3(1.0,0,0), color);

					return fixed4(color,1.0f);
				}
				
				//画旋转圆盘										
				fixed4 frag7(v2f i) : SV_Target
				{
					fixed3 color = fixed3(1.0, 1.0, 1.0);
					fixed2 center = fixed2(0.5,0.5);
					float _RotateSpeed = 1.0;
					
					 center = float2(    center.x*cos(_RotateSpeed * _Time.y) - center.y*sin(_RotateSpeed*_Time.y),
                                      center.x*sin(_RotateSpeed * _Time.y) + center.y*cos(_RotateSpeed*_Time.y) );
					
					center *=0.5;
					center += 0.5;

					color = disk(i.uv, center, 0.1,  fixed3(1.0,0,0), color);
					
					return fixed4(color,1.0f);
				}

				//旋转线条 思路 y值相等的情况 x的范围
				fixed4 frag8(v2f i) : SV_Target
				{
					fixed3 color = fixed3(1.0, 1.0, 1.0);
					fixed2 center = fixed2(0.5,i.uv.y);
					float _RotateSpeed = 1.0;
					center = (center-0.5)*2.0;
					center = float2(  center.x*cos(_RotateSpeed * _Time.y) - center.y*sin(_RotateSpeed*_Time.y),
                                      center.x*sin(_RotateSpeed * _Time.y) + center.y*cos(_RotateSpeed*_Time.y) ) -0.5;	
					
					i.uv = (i.uv-0.5)*2.0;
										
					i.uv = float2(    i.uv.x*cos(_RotateSpeed * _Time.y) -  i.uv.y*sin(_RotateSpeed*_Time.y),
                                      i.uv.x*sin(_RotateSpeed * _Time.y) +  i.uv.y*cos(_RotateSpeed*_Time.y) );					
					
					if(abs(i.uv.x -0) <= 0.05 )
						color.x = 0.5f;					
					
					
					return fixed4(color,1.0f);
				}

				//sin函数
				fixed frag(v2f i) : SV_Target
				{
					fixed3 color = fixed3(1.0, 1.0, 1.0);
					
					float _RotateSpeed = 1.0;
					i.uv = (i.uv-0.5)*2.0;
				
										
				color.x= lerp(i.uv.y-sin(2.0*(i.uv.x+ (_RotateSpeed* _Time.y))),0.01,0.02);
						
					
					
					
					return fixed4(color,1.0f);
				}




				
			ENDCG
		}
	}
}