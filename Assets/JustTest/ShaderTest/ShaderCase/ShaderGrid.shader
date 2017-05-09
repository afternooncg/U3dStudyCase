// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Styudy/ShaderGrid"
{
	
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//o.uv = 1-v.uv;
				return o;
			}
			
			fixed4 frag1 (v2f i) : SV_Target
			{
				fixed4 col = fixed4(1.0,1.0,1.0,1.0);
				// just invert the colors
				//col.xyz = i.uv.y; 

				if(i.uv.y < 0.5)
					col.xyz = fixed4(0.5,0.5,0.5,1.0);

				return col;
			}


			fixed4 frag(v2f i) : SV_Target
        {
                fixed3 backgroundColor = fixed3(1.0, 1.0, 1.0);
				fixed3 axesColor = fixed3(0.0, 0.0, 1.0);
				fixed3 gridColor = fixed3(0.5, 0.5, 0.5);
 
        fixed3 pixel = backgroundColor;
 
        const float tickWidth = 0.2;
        for (float lc = 0.0; lc< 1.0; lc += tickWidth) {
                if (abs(i.uv.x - lc) < 0.002) pixel = gridColor;
                if (abs(i.uv.y - lc) < 0.002) pixel = gridColor;
        }
 
        // »­×ø±êÖá
        if (abs(i.uv.x)<0.005) pixel = axesColor;
       if (abs(i.uv.y)<0.006) pixel = axesColor;
 
        return fixed4(pixel, 1.0);
        }
			ENDCG
		}
	}
}
