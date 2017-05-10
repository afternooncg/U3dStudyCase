// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "CC2/CC2_SimpleRimLight" {
	Properties {
		_RimColor ("Color", Color) = (1,0,0)
		_RimEdge("RimEdge", Range(0.5,2)) = 1.5
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		
         Pass {
			
		    CGPROGRAM

		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"
			#pragma debug 
			
		    struct v2f 
		    {
		        float4 pos : SV_POSITION;
		        float2 uv : TEXCOORD0;
		        float rl : COLOR0;
		    };
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _RimColor;
			float _RimEdge;
			
		    v2f vert (appdata_base v)
		    {
		        v2f o;
		        o.pos = UnityObjectToClipPos (v.vertex);
		        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
		        
		        
		        float3 camDir = normalize(WorldSpaceViewDir(v.vertex));
		        float3 normalDir = mul(float4(v.normal,0),unity_WorldToObject);
		      
		        float c = dot (camDir , normalDir);
		        o.rl = c;
		        
		        
		        return o;
		    }

		    half4 frag (v2f i) : COLOR
		    {
		    	float3 texcol = tex2D (_MainTex, i.uv);
		    	
		    	float d = clamp(i.rl*_RimEdge,0,1);
				
		    	return half4 (texcol * d + _RimColor * (1-d), 1);    
            	
		    }
		    ENDCG

            }
      }
      Fallback "VertexLit"
}
