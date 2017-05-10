// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CC2/Dissolve" {
    Properties {
        _Diffuse ("Base(RGB)", 2D) = "white" {}
        _Noise ("Burn Map", 2D) = "white" {}
        _Amount ("Amount", Range(-1,1)) = 0.5
        _ColorTex ("ColorTex", 2D) = "white" {}
		_ColorAmount("Edge Width", Range(0.5, 2.0)) = 1.0
    }
    SubShader {
        Tags {
            "Queue"="Geometry+300"
            "RenderType"="Opaque"
        }
        Pass {
            //Name "FORWARD"
			Cull Off Lighting Off ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
             #pragma target 3.0
            uniform float4 _LightColor0;
		    uniform fixed _ColorAmount;
            uniform sampler2D _Diffuse;
			uniform float4 _Diffuse_ST;
            uniform sampler2D _Noise; 
			uniform float4 _Noise_ST;
            uniform fixed _Amount;
            uniform sampler2D _ColorTex;
			uniform float4 _ColorTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float burn = (_Noise_var.rgb+_Amount).r;
                clip(burn - 0.5);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse =  _Diffuse_var.rgb*1.2;
////// Emissive:
                float2 t = float2(saturate(((1.0 - burn)*9.0+-4.0)),0.0);
                float4 _Color = tex2D(_ColorTex,TRANSFORM_TEX(t, _ColorTex));
				//ฑ฿ิต
				float EdgeFactor = 1-saturate(burn / _ColorAmount);
				_Color = _Color*EdgeFactor*2.5;
                float3 emissive = _Color.rgb;
/// Final Color:
                float3 finalColor = diffuse + emissive;
               return fixed4(finalColor, 1);
            }
            ENDCG
        }

    }
    FallBack "Diffuse"
   
}
