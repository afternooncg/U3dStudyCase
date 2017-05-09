// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CC2/Paladin_Weapon_Add_Unit" {
    Properties {
        _diffuse_tex ("diffuse_tex", 2D) = "white" {}
	    _diffuse_add_col("diffuse_add_col", Color) = (1,1,1,1)
        _diffuse_value ("diffuse_value", Float ) = 2
        _emission_tex ("emission_tex", 2D) = "white" {}
		_emission_add_col("emission_add_col", Color) = (1,0.5,0,1)
        _add_tex ("add_tex", 2D) = "white" {}
         _add_value ("add_value", Float ) = 2
        _rim_value ("rim_value", Float ) = 1
        _rim_col ("rim_col", Color) = (0.5,0.5,0.5,0.5)
        _alpha ("alpha", Range(0, 1)) = 1
      
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"  //ͶӰ�Ǻ���
            "Queue"="Transparent"  
            "RenderType"="Transparent"   "ForceNoShadowCasting"="True"  //��Ҫ��Ӱ
        }
		LOD 200
        Pass {
              Tags {
                "LightMode"="ForwardBase"
            }
			 Blend SrcAlpha OneMinusSrcAlpha
			 ZWrite Off   //˫��ص�
             
			
            CGPROGRAM  
            #pragma vertex vert      //��֪�����������Ƭ�����غ�������
            #pragma fragment frag    
           #define UNITY_PASS_FORWARDBASE     //��������ǰͨ��
            #include "UnityCG.cginc"    //����unity�����ļ����������õĺ������꣬�ṹ��
           //  #pragma multi_compile_fog     //��ɫ������ָ���Ч
           #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2   //�޳���Ⱦ
            #pragma target 3.0   //����shanderģ��Ϊ3.0    Direct3D 9  ֧��512����ͼ+512������ ��2.0 ֧��32����ͼ+64������ ��4.0 5.0  ֻ֧��DirectX 11
           // float  32λ�������ݣ�half   16λ��������   fixed   12λ ��������  
            uniform half _diffuse_value;
            uniform half4 _emission_add_col;
            uniform sampler2D _emission_tex; 
			uniform half4 _emission_tex_ST;
            uniform sampler2D _add_tex; 
            uniform half4 _add_tex_ST;
            uniform half _add_value;
            uniform sampler2D _diffuse_tex; 
			uniform float4 _diffuse_tex_ST;
            uniform half4 _diffuse_add_col;
            uniform half _rim_value;
            uniform half4 _rim_col;
            uniform half _alpha;
			struct a2v {      //�ṹ������  �����ݴ�Ӧ�ý׶δ���������ɫ��    application  to   vertex
                half4 vertex : POSITION; //ģ�Ϳռ�Ķ����������vertex����
				half3 normal : NORMAL;     //��ģ�Ϳռ�ķ������normal����   
				half2 texcoord : TEXCOORD0;    //ģ�͵������������texcoord����
            };
            struct v2f {       //�ṹ�����   �����Ƭ������֮��Ĵ���
				half4 vertex : SV_POSITION;       //����������ռ��λ����Ϣ
				half2 uv0 : TEXCOORD0;          //����ռ��������uv0��Ϣ
				half4 posWorld : TEXCOORD1;      //����ռ��е�λ��
				half3 normalDir : TEXCOORD2;      //����ռ��з��߷�����Ϣ
                  };
            v2f vert (a2v v) {   //��������ṹ
                v2f o;    //��ʼ��һ������ṹ��
                o.uv0 = v.texcoord;  
                o.normalDir = UnityObjectToWorldNormal(v.normal);   //���߷������������ת������������
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);   
				o.vertex = UnityObjectToClipPos(v.vertex );  // �������λ�ã�����λ�ã�Ϊģ����ͼͶӰ������Զ���λ�ã����ǽ���ά����ͶӰ����ά����
                return o;   
            }//������ɫ�����������ռ�ķ��߷��򣬶���λ�ã��任����������꣬�ٴ��ݸ�Ƭ����ɫ��
                fixed4 frag(v2f i) : COLOR {  
                i.normalDir = normalize(i.normalDir);
			    half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);  //���������ͼ�����һ�����������������������ͬ����
				half3 normalDirection = i.normalDir;
////// Emissive:     max(0.0,dot( normalDirection, viewDirection ))//��ȡ��ǰ��Ĺ���ǿ�ȣ���������ֵ
				float4 _emissiontex_var = tex2D(_emission_tex, TRANSFORM_TEX(i.uv0, _emission_tex));
				float4 _add_tex_var = tex2D(_add_tex, TRANSFORM_TEX(i.uv0, _add_tex));
				float4 _diffusetex_var = tex2D(_diffuse_tex, TRANSFORM_TEX(i.uv0, _diffuse_tex));
				float3 emissive = ((pow(1.0 - max(0, dot(normalDirection, viewDirection)), _rim_value)*_rim_col.rgb) + (_emissiontex_var.rgb*_emission_add_col.rgb*_add_tex_var.rgb*_add_value) + (_diffusetex_var.rgb*_diffuse_add_col.rgb*_diffuse_value));
				float3 finalColor = emissive ;
				return fixed4(finalColor, _alpha);
			}
				ENDCG
		 }
		 }
			 FallBack "Mobile/Diffuse"
				
}
