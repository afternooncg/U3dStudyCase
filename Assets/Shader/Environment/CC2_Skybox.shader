Shader "CC2/Environment/Skybox_opaque_no fog" {

Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Color", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="Geometry+10" "IgnoreProjector"="True" "RenderType"="Opaque"}
	LOD 100
	
	ZWrite Off
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Lighting Off
		Fog {Mode Off}
		SetTexture [_MainTex] { combine texture } 
	}
	
	

}
}
