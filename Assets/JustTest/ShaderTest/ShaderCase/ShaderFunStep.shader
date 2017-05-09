// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Study/ShaderFunStep"
{
        Properties
        {
                _background("背景色",Color)=(0,0,0,0)
        }
        SubShader
        {
                // No culling or depth
                Cull Off ZWrite Off ZTest Always
 
 
 
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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.y = 1 - o.uv.y;
                return o;
        }
 
        // Functions
        fixed4 frag(v2f i) : SV_Target
        {
                float2 r = 2.0*(i.uv - 0.5);
                //_ScreenParams是Unity内置的变量
                float aspectRatio = 1.0;//_ScreenParams.x / _ScreenParams.y;
                r.x *= aspectRatio;
 
                fixed3 pixel = _background.xyz;
                float edge, variable, ret;
 
                //将屏幕划分成五个部分
 
                //第一部分
                if (r.x < -0.6*aspectRatio)
                {
                        variable = r.y;
                        edge = 0.2;
                        if (variable > edge)
                        {
                                ret = 1.0;
                        }
                        else
                        {
                                ret = 0;
                        }
                }
                else if (r.x < -0.2*aspectRatio)
                {
                        variable = r.y;
                        edge = -0.2;
                        //step(a,x):如果x<a结果返回0，反之返回1
                        ret = step(edge, variable);
                }
                else if (r.x < 0.2*aspectRatio)
                {
                        ret = 1.0 - step(0.5, r.y);
                }
                else if (r.x < 0.6*aspectRatio)
                {
                        ret = 0.3 + 0.5*step(-0.4, r.y);
                }
                else
                {
                        ret = step(-0.3, r.y) * (1.0 - step(0.2, r.y));
                }
                pixel = fixed3(ret, ret, ret);
                return fixed4(pixel, 1.0);
 
        }
 
 
                ENDCG
        }
        }
}