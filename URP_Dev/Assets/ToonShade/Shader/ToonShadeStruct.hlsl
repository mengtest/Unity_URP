#ifndef TOONSHADE_STRUCT
#define TOONSHADE_STRUCT

struct VertexInput
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 texcoord0 : TEXCOORD0;
#ifdef _ANGELRING_ON
	float2 texcoord1 : TEXCOORD1;
	float2 lightmapUV : TEXCOORD2;	
#else
	float2 lightmapUV : TEXCOORD1;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutput
{
	// half4 fogFactorAndVertexLight : TEXCOORD7; 
	// x: fogFactor, yzw: vertex light
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float4 posWorld : TEXCOORD2;
	float3 normalDir : TEXCOORD3;
	float3 tangentDir : TEXCOORD4;
	float3 bitangentDir : TEXCOORD5;
	float mirrorFlag : TEXCOORD6;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 7);
half4 fogFactorAndVertexLight : TEXCOORD8;

#ifndef _MAIN_LIGHT_SHADOWS
float4 positionCS : TEXCOORD9;
int mainLightID : TEXCOORD10;
#else
	float4 shadowCoord : TEXCOORD9;
	float4 positionCS : TEXCOORD10;
	int mainLightID : TEXCOORD11;
#endif
UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};


#endif
