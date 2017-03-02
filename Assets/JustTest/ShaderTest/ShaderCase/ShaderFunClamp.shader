Shader "Study/ShaderFunClamp"
{

        Properties
        {
                _background("背景色",Color) = (0,0,0,0)
        }
                SubShader
        {
                // No culling or depth
                Cull Off ZWrite Off ZTest Always
 
                // 追加
                CGINCLUDE
                //定义宏
                #define PI 3.14159
                ENDCG
 
 
        Pass
        {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
 
                #include "UnityCG.cginc"
 
                float4 _background;
 
        struct appdata
        {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
        };
 
        struct v2f
        {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
        };
 
        v2f vert(appdata v)
        {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                o.uv.y = 1 - o.uv.y;
                return o;
        }
 
        // Functions
        fixed4 frag1(v2f i) : SV_Target
        {
                float2 r = 2.0*(i.uv - 0.5);
                //_ScreenParams是Unity内置的变量
                float aspectRatio = _ScreenParams.x / _ScreenParams.y;
                r.x *= aspectRatio;
 
                fixed3 pixel = _background.xyz;
                float edge, variable, ret;
 
                //第一部分
                if (i.uv.x < 0.25) { // part1
                        ret = i.uv.y;
                }
                else if (i.uv.x < 0.5) { // part2
                        float minVal = 0.6;
                        float maxVal = 0.8;
                        variable = i.uv.y;
                        if (variable < minVal) {
                                ret = minVal;
                        }
                        if (variable > minVal && variable < maxVal) {
                                ret = variable;
                        }
                        if (variable > maxVal) {
                                ret = maxVal;
                        }
                }
                else if (i.uv.x < 0.75) { // part3
                        float minVal = 0.6;
                        float maxVal = 0.8;
                        variable = i.uv.y;
                        //clam(x,a,b)：x如果小于a返回a，如果大于b返回b，在a~b范围内返回x
                        ret = clamp(variable, minVal, maxVal);
                }
                else { // part4
                        float y = cos(5.0 * 2.0 * PI *i.uv.y);  //把0-1映射到0-360度转 弧度  5.0表示生成几个循环
                      //  y = (y + 1.0)*0.5; // map [-1,1] to [0,1]

						
                        ret = clamp(y, -0.2, 0.8);
						if(ret == y)
							ret = 1.0;
                }
 
                pixel = fixed3(ret, ret, ret);
                return fixed4(pixel, 1.0);
 
        }
 

 fixed4 frag(v2f i) : SV_Target
        {
 
                fixed3 pixel = _background.xyz;
 
                float edge, variable, ret;
 
                if (i.uv.x < 1.0 / 5.0) { // part1
                        edge = 0.5;
                        ret = step(edge, i.uv.y);
                }
                else if (i.uv.x < 2.0 / 5.0) { // part2
                        float edge0 = 0.45;
                        float edge1 = 0.55;
                        float t = (i.uv.y - edge0) / (edge1 - edge0);    //都是过渡算法，这个过渡范围会大些
                        float t1 = clamp(t, 0.0, 1.0);
                        ret = t1;
                }
                else if (i.uv.x < 3.0 / 5.0) { // part3
                        float edge0 = 0.45;
                        float edge1 = 0.55;
                        float t = clamp((i.uv.y - edge0) / (edge1 - edge0), 0.0, 1.0);
                        float t1 = 3.0*t*t - 2.0*t*t*t;
                        ret = t1;
                }
                else if (i.uv.x < 4.0 / 5.0) { // part4
 
                        //smoothstep(min,max,x)：x=-2*((x-min)/(max-min))^3+3*((x-min)/(max-min))^2，当x=min时返回0，当x=max时返回1
                        ret = smoothstep(0.2, 0.8, i.uv.y);
                }
                else if (i.uv.x < 5.0 / 5.0) {
                        float edge0 = 0.45;
                        float edge1 = 0.55;
                        float t = clamp((i.uv.y - edge0) / (edge1 - edge0), 0.0, 1.0);
                        float t1 = t*t*t*(t*(t*6.0 - 15.0) + 10.0);
                        ret = t1;
                }
 
 
                pixel = fixed3(ret, ret, ret);
                return fixed4(pixel, 1.0);
        }



 
                ENDCG
        }
        }

}
