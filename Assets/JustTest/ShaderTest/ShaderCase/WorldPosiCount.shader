// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Study/worldSpaceColor"
{
    Properties{
        _Point("原点", Vector) = (0., 0., 0., 1.0)
        _DistanceNear("阈值距离", Float) = 5.0
        _ColorNear("离原点小于阈值距离的点的颜色", Color) = (0.0, 1.0, 0.0, 1.0)
        _ColorFar("离原点大于阈值距离的点的颜色", Color) = (0.3, 0.3, 0.3, 1.0)
    }
 
	SubShader{
        Pass{
				CGPROGRAM
 
				#pragma vertex vert  
				#pragma fragment frag 
 
				#include "UnityCG.cginc" 
 
				//使用关键字uniform再定义属性中的变量
				uniform float4 _Point;
				uniform float _DistanceNear;
				uniform float4 _ColorNear;
				uniform float4 _ColorFar;
 
				struct vertexInput {
					float4 vertex : POSITION;
				};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float4 position_in_world_space : TEXCOORD0;
				};
 
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
 
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
 
				//_Object2World是Unity内置的四乘四矩阵，使用了#include "UnityCG.cginc" 命令就可以直接使用，不用再使用uniform关键字进行定义
				output.position_in_world_space =   mul(unity_ObjectToWorld, input.vertex);
				return output;
			}
 
			float4 frag(vertexOutput input) : COLOR
			{
				//计算原点和这个物体的片段点之间的距离
				float dist = distance(input.position_in_world_space,  _Point);
				if (dist < _DistanceNear)
				{
					return _ColorNear;
				}
				else
				{
					return _ColorFar;
				}
			}
 
			ENDCG
		 }
    }
}