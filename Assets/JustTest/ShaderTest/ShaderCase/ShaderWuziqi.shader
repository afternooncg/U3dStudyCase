// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/Wuziqi"
{
        Properties
        {
                _backgroundColor("面板背景色",Color) = (1.0,1.0,1.0,1.0)
                _axesColor("坐标轴的颜色",Color) = (0.0,0.0,1.0)
                _gridColor("网格的颜色",Color) = (0.5,0.5,0.5)
                _tickWidth("网格的间距",Range(0.1,1)) = 0.1
                _gridWidth("网格的宽度",Range(0.0001,0.01)) = 0.008
                _axesXWidth("x轴的宽度",Range(0.0001,0.01)) = 0.006
                _axesYWidth("y轴的宽度",Range(0.0001,0.01)) = 0.007
                _MouseBtnPosX("鼠标点击的X方向位置",float) = 0.0
                _MouseBtnPosY("鼠标点击的Y方向位置",float) = 0.0
                _MouseBtnRadius("鼠标点中位置圆盘的半径",float) = 0.001
                _MouseDownColor("鼠标点中的颜色",Color) = (1.00, 0.329, 0.298,1.0)
        }
        SubShader
        {
                //去掉遮挡和深度缓冲
				/*
                Cull Off
                ZWrite Off
                //开启深度测试
                ZTest Always
				*/
 
                CGINCLUDE
                //添加一个计算方法
                float mod(float a,float b)
                {
                        //floor(x)方法是Cg语言内置的方法，返回小于x的最大的整数
                        return a - b*floor(a / b);
                }
 
                //添加第二个计算方法，根据半径，原点和颜色来绘制圆盘        
                fixed3 disk(fixed2 r,fixed2 center,fixed radius,fixed3 color,fixed3 pixel)
                {
                        fixed3 col = pixel;
                        if (length(r - center) < radius)
                        {
                                col = color;
                        }
                        return col;
                }
                ENDCG
 
                Pass
                {
                        CGPROGRAM
                        //敲代码的时候要注意：“CGPROGRAM”和“#pragma...”中的拼写不同,真不知道“pragma”是什么单词
                        #pragma vertex vert
                        #pragma fragment frag
 
                        #include "UnityCG.cginc"
 
                        uniform float4 _backgroundColor;
                        uniform float4 _axesColor;
                        uniform float4 _gridColor;
                        uniform float _tickWidth;
                        uniform float _gridWidth;
                        uniform float _axesXWidth;
                        uniform float _axesYWidth;
 
                        uniform float _MouseBtnPosX;
                        uniform float _MouseBtnPosY;
                        uniform float _MouseBtnRadius;
                        uniform float4 _MouseDownColor;
 
 
                        struct appdata
                        {
                                float4 vertex:POSITION;
                                float2 uv:TEXCOORD0;
                        };
                        struct v2f
                        {
                                float2 uv:TEXCOORD0;
                                float4 vertex:SV_POSITION;
                        };
                        v2f vert(appdata v)
                        {
                                v2f o;
                                o.vertex = UnityObjectToClipPos(v.vertex);
                                o.uv = v.uv;
                                return o;
                        }
 
                        fixed4 frag(v2f i) :SV_Target
                        {
                                //将坐标的中心从左下角移动到网格的中心
                                float2 r = 2.0*(i.uv - 0.5);
                                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
                                //r.x *= aspectRatio;
                                fixed3 backgroundColor = _backgroundColor.xyz;
                                fixed3 axesColor = _axesColor.xyz;
                                fixed3 gridColor = _gridColor.xyz;
 
                                fixed3 pixel = backgroundColor;
 
                                //定义网格的的间距
                                const float tickWidth = _tickWidth;
                                if (mod(r.x, tickWidth) < _gridWidth)
                                {
                                        pixel = gridColor;
                                }
                                if (mod(r.y, tickWidth) < _gridWidth)
                                {
                                        pixel = gridColor;
                                }
 
                                //画两个坐标轴
                                if (abs(r.x) < _axesXWidth)
                                {
                                        pixel = axesColor;
                                }
                                if (abs(r.y) < _axesYWidth)
                                {
                                        pixel = axesColor;
                                }
 
 
                                //画一个点
                                pixel = disk(r, fixed2(_MouseBtnPosX, _MouseBtnPosY), _MouseBtnRadius, _MouseDownColor, pixel);
                                return fixed4(pixel, 1.0);
                        }
                        ENDCG
                }
 
        }
 
}