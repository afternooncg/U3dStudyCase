Shader "CC2/Paladin_Weapon_Add_SingleLight" {
    Properties {
        _diffuse_tex ("diffuse_tex", 2D) = "white" {}
	    _diffuse_add_col("diffuse_add_col", Color) = (1,1,1,1)
        _diffuse_value ("diffuse_value", Float ) = 2
        _emission_tex ("emission_tex", 2D) = "white" {}
		_emission_add_color("emission_add_color", Color) = (1,0.3931034,0,1)
        _add_tex ("add_tex", 2D) = "white" {}
         _add_value ("add_value", Float ) = 2
        _rim_v ("rim_value", Float ) = 1
        _rim_color ("rim_color", Color) = (0.5,0.5,0.5,0.5)
        _alpha ("alpha", Range(0, 1)) = 1
      
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"  //ͶӰ�Ǻ���
            "Queue"="Transparent"
            "RenderType"="Transparent"   "ForceNoShadowCasting"="True"  //��Ҫ��Ӱ
        }
			
        Pass {
            //���ͨ��ֻ�Ǽ���������͵�ͨ���ڲ�ͬ��Ⱦ·���µı���
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM  
            #pragma vertex vert      //��֪�����������Ƭ�����غ�������
            #pragma fragment frag    
           #define UNITY_PASS_FORWARDBASE     //��������ǰͨ��
            #include "UnityCG.cginc"    //����unity�����ļ����������õĺ������꣬�ṹ��
           //  #pragma multi_compile_fog     //��ɫ������ָ���Ч
           #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2   //�޳���Ⱦ
            #pragma target 3.0   //����shanderģ��Ϊ3.0    Direct3D 9  ֧��512����ͼ+512������ ��2.0 ֧��32����ͼ+64������ ��4.0 5.0  ֻ֧��DirectX 11
            uniform float4 _LightColor0;  //���嵱ǰҪ��Ⱦ���ߵ���ɫ      float  32λ�������ݣ�half   16λ��������   fixed   12λ ��������  
            uniform half _diffuse_value;
            uniform half4 _emission_add_color;
            uniform sampler2D _emission_tex; 
			uniform half4 _emission_tex_ST;
            uniform sampler2D _add_tex; 
            uniform half4 _add_tex_ST;
            uniform half _add_value;
            uniform sampler2D _diffuse_tex; uniform float4 _diffuse_tex_ST;
            uniform half4 _diffuse_add_col;
            uniform half _rim_v;
            uniform half4 _rim_color;
            uniform half _alpha;
            struct a2v {      //�ṹ������  �����ݴ�Ӧ�ý׶δ���������ɫ��    application  to   vertex
                float4 vertex : POSITION; //ģ�Ϳռ�Ķ����������vertex����
                float3 normal : NORMAL;     //��ģ�Ϳռ�ķ������normal����   
                float2 texcoord : TEXCOORD0;    //ģ�͵������������texcoord����
            };
            struct v2f {       //�ṹ�����   �����Ƭ������֮��Ĵ���
                float4 vertex : SV_POSITION;       //����������ռ��λ����Ϣ
                float2 uv0 : TEXCOORD0;          //����ռ��������uv0��Ϣ
                float4 posWorld : TEXCOORD1;      //����ռ��е�λ��
                float3 normalDir : TEXCOORD2;      //����ռ��з��߷�����Ϣ
               // UNITY_FOG_COORDS(3)
            };
            v2f vert (a2v v) {   //��������ṹ
                v2f o;    //��ʼ��һ������ṹ��
                o.uv0 = v.texcoord;  
                o.normalDir = UnityObjectToWorldNormal(v.normal);   //���߷������������ת������������
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);   
                float3 lightColor = _LightColor0.rgb;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex );  // �������λ�ã�����λ�ã�Ϊģ����ͼͶӰ������Զ���λ�ã����ǽ���ά����ͶӰ����ά����
              //  UNITY_TRANSFER_FOG(o,o.pos);
                return o;   
            }//������ɫ�����������ռ�ķ��߷��򣬶���λ�ã��任����������꣬�ٴ��ݸ�Ƭ����ɫ��
            fixed4 frag(v2f i) : COLOR {  
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);  //���������ͼ�����һ�����������������������ͬ����
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);   
				////// Lighting:                
				float3 lightColor = _LightColor0.rgb;

               // float attenuation = 1;
               // float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));//��ȡ��ǰ��Ĺ���ǿ�ȣ���������ֵ
				float3 directDiffuse = max(0.0, NdotL)*lightColor;
                float3 indirectDiffuse = float3(0,0,0);
              //  indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _diffuse_tex_var = tex2D(_diffuse_tex,TRANSFORM_TEX(i.uv0, _diffuse_tex));  //��UnityCG.cginc���ú����ת��
                float3 diffuseColor = (_diffuse_tex_var.rgb*_diffuse_add_col.rgb*_diffuse_value);
              	float3 diffuse =directDiffuse * diffuseColor;   
				//float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _emission_tex_var = tex2D(_emission_tex,TRANSFORM_TEX(i.uv0, _emission_tex));
                float4 _add_tex_var = tex2D(_add_tex,TRANSFORM_TEX(i.uv0, _add_tex));
                float3 emissive = ((pow(1.0-max(0.0,dot(normalDirection, viewDirection)),_rim_v)*_rim_color.rgb)+(_emission_tex_var.rgb*_emission_add_color.rgb*_add_tex_var.rgb*_add_value));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,_alpha);
                return finalRGBA;
            }
            ENDCG
        }
//        Pass {
//            Name "FORWARD_DELTA"
//            Tags {
//                "LightMode"="ForwardAdd"
//            }
//            Blend One One
//            ZWrite Off
//            
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #define UNITY_PASS_FORWARDADD
//            #include "UnityCG.cginc"
//            #include "AutoLight.cginc"
//            #pragma multi_compile_fwdadd
//            #pragma multi_compile_fog
//            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
//            #pragma target 3.0
//            uniform float4 _LightColor0;
//            uniform float _diffuse_value;
//            uniform float4 _emission_add_color;
//            uniform sampler2D _emission_tex; uniform float4 _emission_tex_ST;
//            uniform sampler2D _add_tex; uniform float4 _add_tex_ST;
//            uniform float _add_value;
//            uniform sampler2D _diffuse_tex; uniform float4 _diffuse_tex_ST;
//            uniform float4 _diffuse_add_col;
//            uniform float _rim_v;
//            uniform float4 _rim_color;
//            uniform float _alpha;
//            struct VertexInput {
//                float4 vertex : POSITION;
//                float3 normal : NORMAL;
//                float2 texcoord0 : TEXCOORD0;
//            };
//            struct VertexOutput {
//                float4 pos : SV_POSITION;
//                float2 uv0 : TEXCOORD0;
//                float4 posWorld : TEXCOORD1;
//                float3 normalDir : TEXCOORD2;
//                LIGHTING_COORDS(3,4)
//                UNITY_FOG_COORDS(5)
//            };
//            VertexOutput vert (VertexInput v) {
//                VertexOutput o = (VertexOutput)0;
//                o.uv0 = v.texcoord0;
//                o.normalDir = UnityObjectToWorldNormal(v.normal);
//                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
//                float3 lightColor = _LightColor0.rgb;
//                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
//                UNITY_TRANSFER_FOG(o,o.pos);
//                TRANSFER_VERTEX_TO_FRAGMENT(o)
//                return o;
//            }
//            float4 frag(VertexOutput i) : COLOR {
//                i.normalDir = normalize(i.normalDir);
//                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
//                float3 normalDirection = i.normalDir;
//                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
//                float3 lightColor = _LightColor0.rgb;
//////// Lighting:
//                float attenuation = LIGHT_ATTENUATION(i);
//                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Diffuse:
//                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
//                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
//                float4 _diffuse_tex_var = tex2D(_diffuse_tex,TRANSFORM_TEX(i.uv0, _diffuse_tex));
//                float3 diffuseColor = (_diffuse_tex_var.rgb*_diffuse_add_col.rgb*_diffuse_value);
//                float3 diffuse = directDiffuse * diffuseColor;
///// Final Color:
//                float3 finalColor = diffuse;
//                fixed4 finalRGBA = fixed4(finalColor * _alpha,0);
//                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
//                return finalRGBA;
//            }
            //ENDCG
        //}
    }
    FallBack "Diffuse"
  //  CustomEditor "ShaderForgeMaterialInspector"
}
