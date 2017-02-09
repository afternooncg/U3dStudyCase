// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Custom/GroundPlane_LightmapAsTexture" 
{
	Properties 
	{
		//_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Shadow Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Threshold("Threshold", Range(0,1)) = 0.3
	}
	SubShader 
	{
	  
	   
	   		// Strategy here is render after the other Geo (e.g. the ground plane) but don't write to the Z-Buffer
		// This prevents flickering
		
		Tags
		{
			"IgnoreProjector" = "True"
			"Queue" = "Transparent-500"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		//ZTest LEqual
		//ZWrite Off
		
		//LOD 200
		//Offset -1, -1
	   
	   
		Pass
		{
			CGPROGRAM
			#pragma exclude_renderers ps3 xbox360 flash
			#pragma fragmentoption ARB_precision_hit_fastest
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

			//uniforms
			//uniform sampler2D _MainTex;
			//uniform float4 _MainTex_ST;
			
			uniform float4 _Color;
			half _Threshold;
			#ifdef LIGHTMAP_ON
				
               //uniform sampler2D unity_Lightmap;
               //uniform fixed4 unity_LightmapST;

            #endif
			
			//appdata_base = position,          normal, one texture coordinate.
			//appdata_tan  = position, tangent, normal, one texture coordinate.
			//appdata_full = position, tangent, normal, two texture coordinates, color.

			struct vertexInput//这里是所有untiy 提供的顶点输入选项
			{
				float4 vertex : POSITION; //position(in object coordinates)//从物体坐标或unity提供的本地坐标中提取的位置.
				//float4 tangent : TANGENT; //vector orthogonal to the suface normal//切线
				//float3 normal : NORMAL; //surface normal vector//法线
				//float4 texcoord : TEXCOORD0; //UV通道1，颜色通道
				float4 texcoord1 : TEXCOORD1; //UV通道2，lightmap通道
				fixed4 color : COLOR; //vertex color 顶点颜色
				
			};
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR0;
				half2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(8)
			};
			
			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				UNITY_INITIALIZE_OUTPUT(fragmentInput, o);

				o.pos = mul( UNITY_MATRIX_MVP, i.vertex );//获取顶点位置
				//o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);//获取uv坐标
				 UNITY_TRANSFER_FOG(o,o.pos);
				#ifdef LIGHTMAP_ON

                    o.uv2 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

                #endif
				
				o.color = i.color;
								
				return o;
			}
			
			half4 frag( fragmentInput i ):COLOR
			{
				fixed4 c;// = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
				//c.rgb = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
			   // #ifdef LIGHTMAP_ON

                //    c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));

               // #endif
				//fixed4 c = (0,0,0,0);
					
				#ifdef LIGHTMAP_ON
				    c.rgb = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
				#else
					c.rgb = fixed3(1,1,1);	
				#endif
				//c.rgb = abs(c.rgb);
				
				//float4 outp;
				//float4 newCol = (1-c.r,1-c.g,1-c.b,1);
				//newCol = abs(c);						
				
				
				//c.r = pow((c.r + 0.15f),2) - 0.15f; 
                //c.r = max(0,c.r);
				//c.a = 1 - c.r;
				
				
				//c.a = (1 - c.r)  * i.color.a *_Color.a;

					if (c.r > _Threshold)
						c.a = 0;
					else
						c.a = (1 - c.r)*i.color.a*_Color.a;

				//c.a = (1 - c.r)* i.color.a*_Color.a;
				//c.a = 1;
						//c.rgb =	(8.0 * c.a) * c.rgb * _Color.a * i.color.a;
							
							
				c.rgb *= _Color.rgb;
				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
				
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
