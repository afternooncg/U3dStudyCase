// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:Legacy Shaders/Diffuse,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33745,y:32376,varname:node_4013,prsc:2|diff-3025-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6378,x:30306,y:32553,ptovrint:False,ptlb:SplatTexture,ptin:_SplatTexture,varname:node_6378,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6445,x:30675,y:32560,varname:node_6445,prsc:2,ntxv:0,isnm:False|TEX-6378-TEX;n:type:ShaderForge.SFN_Color,id:8046,x:31756,y:31044,ptovrint:False,ptlb:RedAddColor,ptin:_RedAddColor,varname:node_8046,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4,c2:0,c3:0,c4:0.4;n:type:ShaderForge.SFN_Color,id:2706,x:31716,y:31610,ptovrint:False,ptlb:GreenAddColor,ptin:_GreenAddColor,varname:node_2706,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.4,c3:0,c4:0.4;n:type:ShaderForge.SFN_Subtract,id:7491,x:31949,y:31992,varname:node_7491,prsc:2|A-7262-RGB,B-8042-OUT;n:type:ShaderForge.SFN_Multiply,id:8091,x:32331,y:31538,varname:node_8091,prsc:2|A-8046-RGB,B-6047-OUT;n:type:ShaderForge.SFN_Multiply,id:5521,x:32508,y:31959,varname:node_5521,prsc:2|A-2706-RGB,B-8876-OUT;n:type:ShaderForge.SFN_Lerp,id:1912,x:32955,y:32020,varname:node_1912,prsc:2|A-8091-OUT,B-5521-OUT,T-5856-OUT;n:type:ShaderForge.SFN_Multiply,id:4600,x:32203,y:32049,varname:node_4600,prsc:2|A-7491-OUT,B-9254-OUT;n:type:ShaderForge.SFN_Multiply,id:3752,x:32463,y:32171,varname:node_3752,prsc:2|A-4600-OUT,B-6445-G;n:type:ShaderForge.SFN_Clamp01,id:5856,x:32708,y:32171,varname:node_5856,prsc:2|IN-3752-OUT;n:type:ShaderForge.SFN_Color,id:9145,x:31777,y:32342,ptovrint:False,ptlb:BlueAddColor,ptin:_BlueAddColor,varname:node_9145,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0.45,c4:0.4;n:type:ShaderForge.SFN_Multiply,id:5994,x:32421,y:32442,varname:node_5994,prsc:2|A-9145-RGB,B-6180-OUT;n:type:ShaderForge.SFN_Subtract,id:1948,x:31880,y:32827,varname:node_1948,prsc:2|A-283-RGB,B-7426-OUT;n:type:ShaderForge.SFN_Multiply,id:3141,x:32124,y:32838,varname:node_3141,prsc:2|A-1948-OUT,B-3673-OUT;n:type:ShaderForge.SFN_Multiply,id:6841,x:32356,y:32853,varname:node_6841,prsc:2|A-3141-OUT,B-6445-B;n:type:ShaderForge.SFN_Clamp01,id:6703,x:32572,y:32853,varname:node_6703,prsc:2|IN-6841-OUT;n:type:ShaderForge.SFN_Color,id:2388,x:31370,y:33486,ptovrint:False,ptlb:AlphaAddlColor,ptin:_AlphaAddlColor,varname:node_2388,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4,c2:0.4,c3:0.4,c4:0.4;n:type:ShaderForge.SFN_Multiply,id:3978,x:31975,y:33494,varname:node_3978,prsc:2|A-2388-RGB,B-602-OUT;n:type:ShaderForge.SFN_Multiply,id:3992,x:32214,y:33642,varname:node_3992,prsc:2|A-3978-OUT,B-2484-OUT;n:type:ShaderForge.SFN_Add,id:2484,x:31933,y:33879,varname:node_2484,prsc:2|A-2388-A,B-9406-OUT;n:type:ShaderForge.SFN_Subtract,id:6090,x:31727,y:34087,varname:node_6090,prsc:2|A-7968-RGB,B-8869-OUT;n:type:ShaderForge.SFN_Multiply,id:7681,x:31970,y:34087,varname:node_7681,prsc:2|A-6090-OUT,B-7902-OUT;n:type:ShaderForge.SFN_Multiply,id:3677,x:32329,y:33900,varname:node_3677,prsc:2|A-7681-OUT,B-6445-A;n:type:ShaderForge.SFN_Clamp01,id:1791,x:32559,y:33917,varname:node_1791,prsc:2|IN-3677-OUT;n:type:ShaderForge.SFN_Lerp,id:897,x:33133,y:32237,varname:node_897,prsc:2|A-1912-OUT,B-5994-OUT,T-6703-OUT;n:type:ShaderForge.SFN_Lerp,id:3025,x:33352,y:32509,varname:node_3025,prsc:2|A-897-OUT,B-3992-OUT,T-1791-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8042,x:31713,y:32160,ptovrint:False,ptlb:GreenChannelBias,ptin:_GreenChannelBias,varname:node_8042,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_ValueProperty,id:9254,x:31941,y:32318,ptovrint:False,ptlb:GreenChannelContrast,ptin:_GreenChannelContrast,varname:node_9254,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_ValueProperty,id:7426,x:31496,y:32906,ptovrint:False,ptlb:BlueChannelBias,ptin:_BlueChannelBias,varname:node_7426,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_ValueProperty,id:3673,x:31880,y:33016,ptovrint:False,ptlb:BlueChannelContrast,ptin:_BlueChannelContrast,varname:node_3673,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8869,x:31459,y:34099,ptovrint:False,ptlb:AlphaChannelBias,ptin:_AlphaChannelBias,varname:node_8869,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_ValueProperty,id:7902,x:31435,y:34236,ptovrint:False,ptlb:AlphaChannelContrast,ptin:_AlphaChannelContrast,varname:node_7902,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Tex2d,id:7968,x:31201,y:33916,ptovrint:False,ptlb:Alpha_Tex,ptin:_Alpha_Tex,varname:node_7968,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7327,x:30665,y:31145,ptovrint:False,ptlb:R_Tex,ptin:_R_Tex,varname:node_7327,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d4b367a8c0892844a98ada4aa8dd8b04,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7262,x:30701,y:31839,ptovrint:False,ptlb:G_Tex,ptin:_G_Tex,varname:node_7262,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:283,x:30705,y:32146,ptovrint:False,ptlb:B_Tex,ptin:_B_Tex,varname:node_283,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Desaturate,id:6047,x:31756,y:31243,varname:node_6047,prsc:2|COL-7327-RGB,DES-7594-OUT;n:type:ShaderForge.SFN_Slider,id:7594,x:31230,y:31409,ptovrint:False,ptlb:R_Desaturate,ptin:_R_Desaturate,varname:node_7594,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Desaturate,id:8876,x:31669,y:31856,varname:node_8876,prsc:2|COL-7262-RGB,DES-7481-OUT;n:type:ShaderForge.SFN_Slider,id:7481,x:31314,y:32066,ptovrint:False,ptlb:G_Desaturate,ptin:_G_Desaturate,varname:node_7481,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1456311,max:1;n:type:ShaderForge.SFN_Desaturate,id:6180,x:31892,y:32541,varname:node_6180,prsc:2|COL-283-RGB,DES-5075-OUT;n:type:ShaderForge.SFN_Slider,id:5075,x:31554,y:32619,ptovrint:False,ptlb:B_Desaturate,ptin:_B_Desaturate,varname:node_5075,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:602,x:31785,y:33600,varname:node_602,prsc:2,v1:2;n:type:ShaderForge.SFN_Slider,id:5857,x:31108,y:33766,ptovrint:False,ptlb:A_Desaturate,ptin:_A_Desaturate,varname:node_5857,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5901974,max:1;n:type:ShaderForge.SFN_Desaturate,id:9406,x:31596,y:33884,varname:node_9406,prsc:2|COL-7968-RGB,DES-5857-OUT;proporder:6378-7327-7594-8046-7262-7481-2706-8042-9254-283-5075-9145-7426-3673-7968-5857-2388-8869-7902;pass:END;sub:END;*/

Shader "CC2/Terrain" {
    Properties {
        _SplatTexture ("SplatTexture", 2D) = "white" {}
        _R_Tex ("R_Tex", 2D) = "white" {}
        _R_Desaturate ("R_Desaturate", Range(0, 1)) = 1
        _RedAddColor ("RedAddColor", Color) = (0.4,0,0,0.4)
        _G_Tex ("G_Tex", 2D) = "white" {}
        _G_Desaturate ("G_Desaturate", Range(0, 1)) = 0.1456311
        _GreenAddColor ("GreenAddColor", Color) = (0,0.4,0,0.4)
        _GreenChannelBias ("GreenChannelBias", Float ) = 0.3
        _GreenChannelContrast ("GreenChannelContrast", Float ) = 5
        _B_Tex ("B_Tex", 2D) = "white" {}
        _B_Desaturate ("B_Desaturate", Range(0, 1)) = 0
        _BlueAddColor ("BlueAddColor", Color) = (0,0,0.45,0.4)
        _BlueChannelBias ("BlueChannelBias", Float ) = 0.3
        _BlueChannelContrast ("BlueChannelContrast", Float ) = 1
        _Alpha_Tex ("Alpha_Tex", 2D) = "white" {}
        _A_Desaturate ("A_Desaturate", Range(0, 1)) = 0.5901974
        _AlphaAddlColor ("AlphaAddlColor", Color) = (0.4,0.4,0.4,0.4)
        _AlphaChannelBias ("AlphaChannelBias", Float ) = 0.4
        _AlphaChannelContrast ("AlphaChannelContrast", Float ) = 3
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _SplatTexture; uniform float4 _SplatTexture_ST;
            uniform float4 _RedAddColor;
            uniform float4 _GreenAddColor;
            uniform float4 _BlueAddColor;
            uniform float4 _AlphaAddlColor;
            uniform float _GreenChannelBias;
            uniform float _GreenChannelContrast;
            uniform float _BlueChannelBias;
            uniform float _BlueChannelContrast;
            uniform float _AlphaChannelBias;
            uniform float _AlphaChannelContrast;
            uniform sampler2D _Alpha_Tex; uniform float4 _Alpha_Tex_ST;
            uniform sampler2D _R_Tex; uniform float4 _R_Tex_ST;
            uniform sampler2D _G_Tex; uniform float4 _G_Tex_ST;
            uniform sampler2D _B_Tex; uniform float4 _B_Tex_ST;
            uniform float _R_Desaturate;
            uniform float _G_Desaturate;
            uniform float _B_Desaturate;
            uniform float _A_Desaturate;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _R_Tex_var = tex2D(_R_Tex,TRANSFORM_TEX(i.uv0, _R_Tex));
                float4 _G_Tex_var = tex2D(_G_Tex,TRANSFORM_TEX(i.uv0, _G_Tex));
                float4 node_6445 = tex2D(_SplatTexture,TRANSFORM_TEX(i.uv0, _SplatTexture));
                float4 _B_Tex_var = tex2D(_B_Tex,TRANSFORM_TEX(i.uv0, _B_Tex));
                float4 _Alpha_Tex_var = tex2D(_Alpha_Tex,TRANSFORM_TEX(i.uv0, _Alpha_Tex));
                float3 diffuseColor = lerp(lerp(lerp((_RedAddColor.rgb*lerp(_R_Tex_var.rgb,dot(_R_Tex_var.rgb,float3(0.3,0.59,0.11)),_R_Desaturate)),(_GreenAddColor.rgb*lerp(_G_Tex_var.rgb,dot(_G_Tex_var.rgb,float3(0.3,0.59,0.11)),_G_Desaturate)),saturate((((_G_Tex_var.rgb-_GreenChannelBias)*_GreenChannelContrast)*node_6445.g))),(_BlueAddColor.rgb*lerp(_B_Tex_var.rgb,dot(_B_Tex_var.rgb,float3(0.3,0.59,0.11)),_B_Desaturate)),saturate((((_B_Tex_var.rgb-_BlueChannelBias)*_BlueChannelContrast)*node_6445.b))),((_AlphaAddlColor.rgb*2.0)*(_AlphaAddlColor.a+lerp(_Alpha_Tex_var.rgb,dot(_Alpha_Tex_var.rgb,float3(0.3,0.59,0.11)),_A_Desaturate))),saturate((((_Alpha_Tex_var.rgb-_AlphaChannelBias)*_AlphaChannelContrast)*node_6445.a)));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Legacy Shaders/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
