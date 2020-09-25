#ifndef DEPTH_ONLY
#define DEPTH_ONLY

#include "Property.hlsl"


struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float2 texcoord : TEXCOORD0;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
};


float4 GetShadowPositionHClip(Attributes input)
{
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
    Light light = GetMainLight();
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, light.direction));
#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif
    return positionCS;
}

Varyings vertDepth(Attributes input)
{
    Varyings output = (Varyings)0;
    output.positionCS = GetShadowPositionHClip(input);
    return output;
}
            
half4 fragDepth(Varyings input) : SV_TARGET
{
    return 1;
}




#endif
