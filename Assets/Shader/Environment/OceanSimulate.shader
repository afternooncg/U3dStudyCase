//_WaterTexure.r is caustic pattern. _WaterTexure.g is wave motion.
Shader "Custom/OceanSimulate" 
{
    Properties 
    {
        _WaterTexure ("Water Texture", 2D) = "white" {}
        _WaveTexure ("Wave Texture", 2D) = "white" {}
        _WaterTransparency("Water Transparency", Float) = 0.5
        _MainColor ("Main Color", Color) = (0.5, 0.5, 0.5, 1)
        _SecondColor ("Second Color", Color) = (0.5, 0.5, 0.5, 1)
        _WaveTiling("Wave Tiling", Float) = 3.0
        _WaveSize ("Wave Size", Float) = 10.0
        _ReflectionIntensity ("Reflection Intensity", Float) = 1.0
        _Speed ("Wave Speed", Float) = 1.0
       // _Direction ("Wave Direction", Float) = 1.0
        
        
    }
    SubShader 
    {
    Tags { "Queue" = "transparent-200"  }
        Blend SrcAlpha OneMinusSrcAlpha
        Zwrite Off
        Pass
        {
            CGPROGRAM
            //#pragma only_renderers d3d9
            #pragma target 2.0
           // #pragma exclude_renderers d3d11 gles3 d3d11_9x  xbox360 ps3 flash 
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            
            //uniforms
            uniform sampler2D _WaterTexure; //r is caustic pattern. g is wave motion.
            uniform half4 _WaterTexure_ST;
            uniform sampler2D _WaveTexure; 
            uniform half4 _WaveTexure_ST;
            uniform fixed4 _MainColor;
            uniform fixed4 _SecondColor;
            uniform fixed _ReflectionIntensity;
            uniform fixed _WaveTiling;
            uniform fixed _WaveSize;
            uniform float _Speed;
            uniform float _WaterTransparency;
           // uniform float _Direction;
            
            
            struct vertexInput
            {
                fixed4 vertex : POSITION; 
                fixed4 texcoord : TEXCOORD0; 
                float4 texcoord2 : TEXCOORD1; 
                fixed4 color : COLOR;
                fixed3 normal : NORMAL; //surface normal vector
                
            };
            
            struct fragmentInput
            {
                fixed4 pos : SV_POSITION;
                //use UV to pass the wiggle motion to the frag
                float2 uv : TEXCOORD0;
                
                half4 vertcolor : COLOR0;
                
                //uv3 is for wiggle itself
                float2 uv3 : TEXCOORD1;
                
                float2 uv4 : TEXCOORD2;
                float2 uv5 : TEXCOORD3;
               UNITY_FOG_COORDS(8)
            };
            
            fragmentInput vert( vertexInput i )
            {
                fragmentInput o;
                o.uv = TRANSFORM_TEX(i.texcoord, _WaterTexure);
                o.uv3 = i.texcoord * _WaveTiling;
                
               // i.vertex.y =  sin(50 * (o.uv3.x + o.uv3.y + _Direction) + _Time.y * 50 * _Speed) * _WaveSize;
                i.vertex.y =  sin(50 * (o.uv3.x + o.uv3.y ) + _Time.y * 50 * _Speed) * _WaveSize;
               
                o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
                
               
				
                
                o.vertcolor = i.color;
                
                
                o.uv3.x += _Time.y * _Speed;
                //o.uv3.y += _Time.y * _Direction;
                
                o.uv4 = TRANSFORM_TEX(i.texcoord, _WaveTexure);
                o.uv4.x = o.uv4.x + frac(_Time.x * 0.75f); 
                o.uv4.y = o.uv4.y - frac(_Time.x * -0.75f); 
				o.uv5 = i.texcoord2;
				UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            
            half4 frag( fragmentInput i ):COLOR
            {
            	//wave motion
                half4 wiggle = tex2D(_WaterTexure, i.uv3); 
                
                //Wiggle Size
                i.uv.x -= wiggle.g ;
                i.uv.y += wiggle.g ;
                
                
                half4 tex = tex2D(_WaterTexure, i.uv4 + i.uv) ;                
                //get the pure highlight texture.
                half theHighLight = tex.b;
                
                //for the water patterns
                half4 tex2 = tex2D(_WaterTexure, i.uv4 - i.uv) ;
                
                
                const half ambientLight = 0.2;
                //mix wiggle and water Patterns, add a ambientlight
                tex.rgb = ambientLight + (tex2.r * wiggle.g) ;
                
                
                
                
                // this is a hack, make the base water texture has stronger Contrast
                tex.rgb = tex.rgb * tex.b * wiggle.g  * _ReflectionIntensity;
                
                
                
               
                //tint the water
                tex *= _MainColor;
                tex += _SecondColor * i.vertcolor.g;
                
                
                //overall water transparency
                tex.a = _WaterTransparency;
                
                
                //Make wiggle more extreme for the highlight
                wiggle.g = pow((wiggle.g + 0.15f),10) - 0.15f; 
                wiggle.g = max(0,wiggle.g);
                                
                //add highlight
                theHighLight = theHighLight * wiggle.g;
                
                //less highlight in the Polluted water
                theHighLight *= 1 - (i.vertcolor.g * 2.2);
                
                tex = tex + theHighLight;
                
                
                //soft the water edges before add the wave
             	tex.a *= i.vertcolor.a;
               
               
               
                // UV anim for the wave
                i.uv5.y += _Time.x * 2 ;
                //add wave ========================================
                half4 WaveTex = tex2D(_WaveTexure, i.uv5 + i.uv3) ;
                //fade in and fade out wave
                WaveTex.a *= i.vertcolor.r;
                //only keep the wave part for the rgb
                WaveTex.rgb = WaveTex.rgb * WaveTex.a;
                
                
                float4 finelTex = tex + WaveTex;
                
                UNITY_APPLY_FOG(i.fogCoord, finelTex);
				//UNITY_OPAQUE_ALPHA(finelTex.a);
                
               return finelTex ;
            }
            
            ENDCG
        }
    } 
    //FallBack "Diffuse"
}