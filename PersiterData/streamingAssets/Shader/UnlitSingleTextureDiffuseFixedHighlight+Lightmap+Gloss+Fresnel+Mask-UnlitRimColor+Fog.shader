// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable

// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable

//Copyright (c) 2014 Kyle Halladay
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.


//for the secondary texture, R = AO, G = specular, B = Emission;


Shader "Custom/Unlit Single Texture Diffuse Fixed Highlight + Lightmap + Gloss + Fresnel + Mask - Unlit Rim Color + Fog" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SecondaryTex ("Secondary Texture", 2D) = "white" {}
		_EmissionTex ("Emission Texture", 2D) = "white" {}
		_SpecOffset ("Specular Offset from Camera", Vector) = (1, 2, 1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)		
		_SpecularPower("Specular Power", Float) = 1
		_Shininess ("Shininess", Float) = 10
		
		_SecondaryLightOffset ("Secondary Light", Vector) = (-1, -2, -1,1)
		_SpecColor2 ("Secondary Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecularPower2("Secondary Specular Power", Float) = 1
		_Shininess2 ("Secondary Specular Shininess", Float) = 10
		
		_Scale ("Frensel Scale", Range(0.0, 1.0)) = 0.5
		_RimColor ("Rim Color", Color) = (0.5, 0.5, 0.5, 1)	
		_EmissionIntensity ("Damage Emission Intensity", Range(0.0, 10.0)) = 0.0
		//_EmissionColor ("Emission Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
	        #pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			
			uniform sampler2D _MainTex;
			uniform sampler2D _SecondaryTex;
			uniform sampler2D _EmissionTex;
			uniform float4 _MainTex_ST;
			uniform float4 _RimColor;
			uniform float _Bias; //Marcus: You could pack all of these float1s into float4s. That should be faster to load on PS4. Maybe for Phones as well?
			uniform float _Power;
			uniform float _Scale;
			//for specular
			uniform float3 _SpecOffset;
			uniform float4 _SpecColor;
			uniform float _SpecularPower;
			uniform float _Shininess;
			
			uniform float3 _SecondaryLightOffset;
			uniform float4 _SpecColor2;
			uniform float _SpecularPower2;
			uniform float _Shininess2;
			
			uniform float _EmissionIntensity;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};

			struct fragmentInput
			{
				//float3 normal : NORMAL;
				float3 i : TEXCOORD4; //Marcus: Need better name than i
				float3 normalD : TEXCOORD5;
				float3 lightDirection : TEXCOORD6;
				//float3 test : TEXCOORD7;
				
				
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD1;
				float reflectionFactor : TEXCOORD3;
				//for specular
				fixed3 spec : TEXCOORD2;
				UNITY_FOG_COORDS(8)
			};
			
			
			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				UNITY_INITIALIZE_OUTPUT(fragmentInput, o);
				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv = i.texcoord;
                UNITY_TRANSFER_FOG(o,o.pos);
				float3 posWorld = mul(unity_ObjectToWorld, i.vertex).xyz;
				
				float3 normalDirection = normalize( mul ( float4( i.normal, 1.0 ), unity_WorldToObject ).xyz);
				float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
				
				o.normalD = normalDirection;
				o.i = I;
				
				
				o.reflectionFactor = _Scale * pow(1.0 + dot(I, normalDirection), 1.4);
				
				////for specular==============================================================================================================
				
				float3 lightDirection = normalize(_SpecOffset);
				float3 lightDirection2 = normalize(_SecondaryLightOffset);
			
				o.lightDirection = lightDirection;
				
				fixed3 specularReflection = _SpecColor.rgb * pow( max(0.0,dot( reflect(-lightDirection, normalDirection), -I)),_Shininess);
				fixed3 specularReflection2 = _SpecColor2.rgb * pow( max(0.0,dot( reflect(-lightDirection2, normalDirection), -I)),_Shininess2);
		
               
				o.spec = (specularReflection * _SpecularPower) + (specularReflection2 * _SpecularPower2);

				
				#ifdef LIGHTMAP_ON

                    o.uv2 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

                #endif

				return o;
			}

			float4 frag(fragmentInput i) :  COLOR
			{  

				fixed4 c = tex2D (_MainTex, i.uv);
				fixed4 c2 = tex2D (_SecondaryTex, i.uv);
				fixed4 c3 = tex2D (_EmissionTex, i.uv);
			
				fixed3 specularReflection =  max(0.0,_SpecColor.rgb * pow( max(0.0,dot( reflect(-i.lightDirection, i.normalD), -i.i)), c2.g * _Shininess ));


				c.rgb = c.rgb + ((specularReflection + i.spec.rgb) * c2.g); 
				
				
				 
				
				
				// AO
				fixed4 finelTex = lerp(c,_RimColor, i.reflectionFactor);
				finelTex = finelTex * c2.r;
				
				
				
				
				
				
				#ifdef LIGHTMAP_ON

                   finelTex.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));

                #endif
				
					 
				 // Emission
				finelTex.rgb = (1 - c2.b + (_EmissionIntensity * c2.b)) * c3.rgb + finelTex.rgb;
				 UNITY_APPLY_FOG(i.fogCoord, finelTex);
			
				
				
				return finelTex;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
