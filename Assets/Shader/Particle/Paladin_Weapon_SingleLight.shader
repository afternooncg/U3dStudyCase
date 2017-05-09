// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
            "IgnoreProjector"="True"  //投影仪忽略
            "Queue"="Transparent"
            "RenderType"="Transparent"   "ForceNoShadowCasting"="True"  //不要阴影
        }
			
        Pass {
            //这个通道只是检查这种类型的通道在不同渲染路径下的表现
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM  
            #pragma vertex vert      //告知编译器顶点和片段像素函数名称
            #pragma fragment frag    
           #define UNITY_PASS_FORWARDBASE     //基本的向前通道
            #include "UnityCG.cginc"    //包含unity内置文件，包含常用的函数，宏，结构体
           //  #pragma multi_compile_fog     //着色器编译指令：雾效
           #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2   //剔除渲染
            #pragma target 3.0   //定义shander模型为3.0    Direct3D 9  支持512张贴图+512个计算 ，2.0 支持32张贴图+64个计算 ，4.0 5.0  只支持DirectX 11
            uniform float4 _LightColor0;  //定义当前要渲染光线的颜色      float  32位浮点数据，half   16位浮点数据   fixed   12位 定点数据  
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
            struct a2v {      //结构体输入  把数据从应用阶段传到顶点着色器    application  to   vertex
                float4 vertex : POSITION; //模型空间的顶点坐标填充vertex变量
                float3 normal : NORMAL;     //用模型空间的法线填充normal变量   
                float2 texcoord : TEXCOORD0;    //模型的纹理坐标填充texcoord变量
            };
            struct v2f {       //结构体输出   顶点和片段像素之间的传递
                float4 vertex : SV_POSITION;       //顶点在世界空间的位置信息
                float2 uv0 : TEXCOORD0;          //世界空间中纹理的uv0信息
                float4 posWorld : TEXCOORD1;      //世界空间中的位置
                float3 normalDir : TEXCOORD2;      //世界空间中法线方向信息
               // UNITY_FOG_COORDS(3)
            };
            v2f vert (a2v v) {   //声明输出结构
                v2f o;    //初始化一个输入结构体
                o.uv0 = v.texcoord;  
                o.normalDir = UnityObjectToWorldNormal(v.normal);   //法线方向从物体坐标转换到世界坐标
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);   
                float3 lightColor = _LightColor0.rgb;
                o.vertex = UnityObjectToClipPos(v.vertex );  // 输出顶点位置（像素位置）为模型视图投影矩阵乘以顶点位置，就是将三维坐标投影到二维窗口
              //  UNITY_TRANSFER_FOG(o,o.pos);
                return o;   
            }//顶点着色器计算出世界空间的法线方向，顶点位置，变换后的纹理坐标，再传递给片段着色器
            fixed4 frag(v2f i) : COLOR {  
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);  //定义输出视图方向归一化输入输出向量，并保持相同方向
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);   
				////// Lighting:                
				float3 lightColor = _LightColor0.rgb;

               // float attenuation = 1;
               // float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));//获取当前点的光照强度，并输出最大值
				float3 directDiffuse = max(0.0, NdotL)*lightColor;
                float3 indirectDiffuse = float3(0,0,0);
              //  indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _diffuse_tex_var = tex2D(_diffuse_tex,TRANSFORM_TEX(i.uv0, _diffuse_tex));  //在UnityCG.cginc内置宏计算转换
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
