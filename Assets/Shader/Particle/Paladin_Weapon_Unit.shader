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
            "IgnoreProjector"="True"  //投影仪忽略
            "Queue"="Transparent"  
            "RenderType"="Transparent"   "ForceNoShadowCasting"="True"  //不要阴影
        }
		LOD 200
        Pass {
              Tags {
                "LightMode"="ForwardBase"
            }
			 Blend SrcAlpha OneMinusSrcAlpha
			 ZWrite Off   //双面关掉
             
			
            CGPROGRAM  
            #pragma vertex vert      //告知编译器顶点和片段像素函数名称
            #pragma fragment frag    
           #define UNITY_PASS_FORWARDBASE     //基本的向前通道
            #include "UnityCG.cginc"    //包含unity内置文件，包含常用的函数，宏，结构体
           //  #pragma multi_compile_fog     //着色器编译指令：雾效
           #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2   //剔除渲染
            #pragma target 3.0   //定义shander模型为3.0    Direct3D 9  支持512张贴图+512个计算 ，2.0 支持32张贴图+64个计算 ，4.0 5.0  只支持DirectX 11
           // float  32位浮点数据，half   16位浮点数据   fixed   12位 定点数据  
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
			struct a2v {      //结构体输入  把数据从应用阶段传到顶点着色器    application  to   vertex
                half4 vertex : POSITION; //模型空间的顶点坐标填充vertex变量
				half3 normal : NORMAL;     //用模型空间的法线填充normal变量   
				half2 texcoord : TEXCOORD0;    //模型的纹理坐标填充texcoord变量
            };
            struct v2f {       //结构体输出   顶点和片段像素之间的传递
				half4 vertex : SV_POSITION;       //顶点在世界空间的位置信息
				half2 uv0 : TEXCOORD0;          //世界空间中纹理的uv0信息
				half4 posWorld : TEXCOORD1;      //世界空间中的位置
				half3 normalDir : TEXCOORD2;      //世界空间中法线方向信息
                  };
            v2f vert (a2v v) {   //声明输出结构
                v2f o;    //初始化一个输入结构体
                o.uv0 = v.texcoord;  
                o.normalDir = UnityObjectToWorldNormal(v.normal);   //法线方向从物体坐标转换到世界坐标
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);   
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex );  // 输出顶点位置（像素位置）为模型视图投影矩阵乘以顶点位置，就是将三维坐标投影到二维窗口
                return o;   
            }//顶点着色器计算出世界空间的法线方向，顶点位置，变换后的纹理坐标，再传递给片段着色器
                fixed4 frag(v2f i) : COLOR {  
                i.normalDir = normalize(i.normalDir);
			    half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);  //定义输出视图方向归一化输入输出向量，并保持相同方向
				half3 normalDirection = i.normalDir;
////// Emissive:     max(0.0,dot( normalDirection, viewDirection ))//获取当前点的光照强度，并输出最大值
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
