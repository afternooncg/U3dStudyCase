// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/ShaderFunLerp"
{
	
        Properties
        {
                _background("背景色",Color) = (0,0,0,0)
                _col1("颜色1",Color)=(0,0,0,0)
                _col2("颜色2",Color)=(0,0,0,0)
        }
        SubShader
        {
                // No culling or depth
                Cull Off ZWrite Off ZTest Always
 
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
                float4 _col1;
                float4 _col2;
 
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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.y = 1 - o.uv.y;
                return o;
        }
        // Functions
        fixed4 frag(v2f i) : SV_Target
        {
 
                fixed3 pixel = _background.xyz;
 
                fixed3 ret;
 
                if (i.uv.x < 1.0 / 5.0) { // part1
                        float x0 = 0.2;
                        float x1 = 0.7;
                        float m = 0.1;
                        float val = x0 * (1.0 - m) + x1*m;
                        ret = fixed3(val, val, val);
                }
                else if (i.uv.x < 2.0 / 5.0) { // part2
                        float x0 = 0.2;
                        float x1 = 0.7;
                        float m = i.uv.y;
                        float val = x0*(1.0 - m) + x1*m;
                        ret = fixed3(val, val, val);
                }
                else if (i.uv.x < 3.0 / 5.0) { // part3
                        float x0 = 0.2;
                        float x1 = 0.7;
                        float m = i.uv.y;
                        //lerp(a,b,f)返回(1-f)*a+b*f
                        float val = lerp(x0, x1, m);
                        ret = fixed3(val, val, val);
 
                }
                else if (i.uv.x < 4.0 / 5.0) { // part4
                        float m = i.uv.y;
                        ret = lerp(_col1, _col2, m);
                }
                else if (i.uv.x < 5.0 / 5.0) {
                        float m = smoothstep(0.5, 0.6, i.uv.y);
                        ret = lerp(_col1, _col2, m);
                }
                pixel = ret;
                return fixed4(pixel, 1.0);
        }
 
 
        ENDCG
                }
        }
}
