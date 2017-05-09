// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CC2/CC2_Unlit Single Texture Diffuse Fixed Highlight + Lightmap + Gloss + Fresnel + Mask - Unlit Rim Color" 
{
	Properties 
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_SecondaryTex ("MainTex_R", 2D) = "white" {}
		_SpecularTex ("Specular Texture", 2D) = "white" {}
		_SpecOffset ("Specular Offset from Camera", Vector) = (1, 10, 2)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)		
		_SpecularPower("Specular Power", Float) = 10
		_Shininess ("Shininess", Float) = 10
		
		_SecondaryLightOffset ("Secondary Light", Vector) = (1, 10, 2)
		_SpecColor2 ("Secondary Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_SpecularPower2("Secondary Specular Power", Float) = 10
		_Shininess2 ("Secondary Specular Shininess", Float) = 10
		
		_Scale ("Frensel Scale", Range(0.0, 1.0)) = 1.0
		_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_EmissionIntensity ("Damage Emission Intensity", Range(0.0, 10.0)) = 1.0
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
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			
			uniform sampler2D _MainTex;
			uniform sampler2D _SecondaryTex;
			uniform sampler2D _SpecularTex;
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
			
			
			//for Emission Color
			//uniform float4 _EmissionColor;
			
			#ifdef LIGHTMAP_ON
				
                // uniform sampler2D unity_Lightmap;
                // uniform float4 unity_LightmapST;

            #endif
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
				
			};
			
			
			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				UNITY_INITIALIZE_OUTPUT(fragmentInput, o);
			
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv = i.texcoord;

				float3 posWorld = mul(unity_ObjectToWorld, i.vertex).xyz;
				
				float3 normalDirection = normalize( mul ( float4( i.normal, 1.0 ), unity_WorldToObject ).xyz);
				float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
				
				o.normalD = normalDirection;
				o.i = I;
				
				//o.normal = i.normal;
				
				o.reflectionFactor = _Scale * pow(1.0 + dot(I, normalDirection), 1.4);
				
				////for specular==============================================================================================================
				
				float3 lightDirection = normalize(_SpecOffset);
				float3 lightDirection2 = normalize(_SecondaryLightOffset);
			
				o.lightDirection = lightDirection;
				
				fixed3 specularReflection = _SpecColor.rgb * pow( max(0.0,dot( reflect(-lightDirection, normalDirection), -I)),_Shininess);
				fixed3 specularReflection2 = _SpecColor2.rgb * pow( max(0.0,dot( reflect(-lightDirection2, normalDirection), -I)),_Shininess2);
		
               
				o.spec = (specularReflection * _SpecularPower) + (specularReflection2 * _SpecularPower2);
				//o.spec = specularReflection2 * _SpecularPower2;
				
				
				#ifdef LIGHTMAP_ON

                    o.uv2 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

                #endif
              
				//o.test = specularReflection;
				return o;
			}

			float4 frag(fragmentInput i) :  COLOR
			{  
			
				//float3 normalDirection = normalize( mul ( float4( i.normal, 1.0 ), _World2Object ).xyz);
				fixed4 c = tex2D (_MainTex, i.uv);
				fixed4 c2 = tex2D (_SecondaryTex, i.uv);
				fixed4 c3 = tex2D (_SpecularTex, i.uv);
			
			
				//fixed3 specularReflection = _SpecColor.rgb * pow( max(0.0,dot( reflect(-i.lightDirection, i.normalD), -i.i)), (1-c2.b) * _Shininess ); //Marcus: i.i needs better name
				fixed3 specularReflection =  max(0.0,_SpecColor.rgb * pow( max(0.0,dot( reflect(-i.lightDirection, i.normalD), -i.i)), c2.g * _Shininess ));
				//fixed3 specularReflection =  _SpecColor.rgb * pow( max(0.0,dot( reflect(-i.lightDirection, i.normalD), -i.i)), max(0.0, c2.b * _Shininess ));
				//fixed3 specularReflection =  _SpecColor.rgb * pow( max(0.0,dot( reflect(-i.lightDirection, i.normalD), -i.i)), c2.b * _Shininess );
				//Marcus: It is faster to do a*b + a*c then to do a*(b+c)
				
			
			
			
			
				
				//c.rgb = c.rgb + (i.spec.rgb * c2.g);
				c.rgb = c.rgb + ((specularReflection + i.spec.rgb) * c2.g); //Marcus: same idea as above
				
				
				 #ifdef LIGHTMAP_ON

                   c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));

                #endif
				
				
				// AO
				fixed4 f = lerp(c,_RimColor, i.reflectionFactor);
				f = f * c2.r;
				
				
				
				
				// Emission
				//f = f + (_EmissionColor * c2.b)*_EmissionColor.a*10;
				//f.rgb = (1 + (c2.b * _EmissionIntensity )) * c3.rgb + f.rgb;
				f.rgb = (1 - c2.b + (_EmissionIntensity * c2.b)) * c3.rgb + f.rgb;
				//f.rgb = specularReflection;
				
				return f;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
