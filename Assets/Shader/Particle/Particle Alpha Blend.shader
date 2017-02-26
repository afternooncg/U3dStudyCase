Shader "CC2/Particles/Alpha Blended" {
Properties {
	_TintColor("Tint Color", Color) = (0,0,0,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	}

Category{
	Tags { "Queue" = "Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off
	//ColorMask RGB
	ZWrite Off
		
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	
	SubShader {
		Pass {
	            SetTexture[_MainTex]{
	                    constantColor[_TintColor]
	                   combine constant * primary
}
                SetTexture[_MainTex]{
	            combine texture * previous DOUBLE
			}
		}
	}
}
}
