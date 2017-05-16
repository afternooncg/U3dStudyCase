
Shader "Study/ShaderStroke"

{

Properties{

_Color("Color", Color) = (1, 1, 1, 0.5)

// user-specified RGBA color including opacity

}

SubShader{

Tags{ "Queue" = "Transparent" }

// draw after all opaque geometry has been drawn

Pass{

ZWrite Off // don't occlude other objects

Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending

//==float4 result = fragment_output.aaaa * fragment_output + (float4(1.0, 1.0, 1.0, 1.0) - fragment_output.aaaa) * pixel_color;



CGPROGRAM



#pragma vertex vert 

#pragma fragment frag 



#include "UnityCG.cginc"



uniform float4 _Color; // define shader property for shaders



struct vertexInput {

float4 vertex : POSITION;

float3 normal : NORMAL;

};

struct vertexOutput {

float4 pos : SV_POSITION;

float3 normal : TEXCOORD;

float3 viewDir : TEXCOORD1;

};



vertexOutput vert(vertexInput input)

{

vertexOutput output;


float4x4 modelMatrix = unity_ObjectToWorld;//模型矩阵

float4x4 modelMatrixInverse = unity_WorldToObject;//模型的逆矩阵



output.normal = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

output.viewDir = normalize(_WorldSpaceCameraPos- mul(modelMatrix, input.vertex).xyz);


output.pos = UnityObjectToClipPos(input.vertex);

return output;

}



float4 frag(vertexOutput input) : COLOR

{

//为什么在顶点着色器程序中已经将这两个向量归一化了，在此为什么还要归一化？

//1>首先，在顶点程序中归一化是因为要在任何它们之间的方向上进行或多或少的插值

//2>在此处又进行一次插值是因为，上面的插值过程会将归一化的值扭曲

float3 normalDirection = normalize(input.normal);

float3 viewDirection = normalize(input.viewDir);



//float newOpacity = min(1.0, _Color.a/abs(dot(viewDirection, normalDirection)));
float newOpacity = min(1.0, 1.0-dot(viewDirection, normalDirection));

return float4(_Color.rgb, newOpacity);

}

ENDCG

}

}

}