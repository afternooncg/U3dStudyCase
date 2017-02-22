Shader "CC2/Environment/Skybox_transparent_no fog" {

Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Color", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="Transparent-20" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Lighting Off
		Fog {Mode Off}
		SetTexture [_MainTex] { combine texture } 
	}
	
	

}
}
