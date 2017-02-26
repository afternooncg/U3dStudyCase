Shader "CC2/Environment/RealisticSnow" {
	Properties{
		//岩石贴图  
		_MainTex("Base (RGB)", 2D) = "white" {}
	//法线贴图  
	_Bump("Bump", 2D) = "bump" {}
	//表示覆盖在岩石上雪的数量，范围从0~1  
	_Snow("Snow Level", Range(0,1)) = 0
		//积雪颜色  默认白色  
		_SnowColor("Snow Color", Color) = (1.0,1.0,1.0,1.0)
		//积雪方向  
		_SnowDirection("Snow Direction", Vector) = (0,1,0)
		//积雪厚度  
		_SnowDepth("Snow Depth", Range(0,0.2)) = 0.1
		//湿润度，如果该值越大，则混色中积雪颜色所占比例越低，这表明积雪越湿润，则雪的颜色越少，都化成水了  
		_Wetness("Wetness", Range(0, 0.5)) = 0.3
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
        #pragma surface surf Lambert vertex:vert  

		//获取属性的具体值  
		sampler2D _MainTex;
	sampler2D _Bump;
	float _Snow;
	float4 _SnowColor;
	float4 _SnowDirection;
	float _SnowDepth;
	float _Wetness;

	//输入结构体  
	struct Input {
		//获取岩石贴图的uv坐标  
		float2 uv_MainTex;
		//获取法线贴图的uv坐标  
		float2 uv_Bump;
		//如果SurfaceOutput中设定了Normal值的话，通过worldNormal可以获取当前点在世界中的法线值  
		float3 worldNormal; INTERNAL_DATA
	};

	//顶点着色程序入口  

	void vert(inout appdata_full v)
	{
		//将_SnowDirection转化到模型局部坐标系下  
		float4 sn = mul(UNITY_MATRIX_IT_MV, _SnowDirection);

		if (dot(v.normal, sn.xyz) >= lerp(1,-1, (_Snow * 2) / 3))
		{
			v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
		}
	}

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));

		//WorldNormalVector通过输入的点及这个点的法线值，来计算它在世界坐标中的方向  
		//然后用输入点在世界坐标中的方向和 积雪的方向 做点积运算  并减去 _Snow插值  
		half difference = dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) - lerp(1,-1,_Snow);

		//saturate(x)函数  如果 x 小于 0 ，返回 0 ；如果 x 大于 1 ，返回 1 ；否则，返回 x  
		difference = saturate(difference / _Wetness);

		//对光源的反射率。  
		o.Albedo = difference*_SnowColor.rgb + (1 - difference) *c;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}


