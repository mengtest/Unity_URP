#ifndef OUTLINE
#define OUTLINE

#include "Common.hlsl"
#include "Property.hlsl"



struct Attributes
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct Varyings
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normalDir : TEXCOORD1;
	float3 tangentDir : TEXCOORD2;
	float3 bitangentDir : TEXCOORD3;
};

float4 GetOutlineNormal(float3 vertex, float3 normal, float3 bakedNormalDir, half outlineWidth)
{
	float4 a = float4(vertex + normal * outlineWidth, 1);
	float4 b = float4(vertex + bakedNormalDir * outlineWidth, 1);
	float4 result = lerp(a, b, _Is_BakedNormal);
	return result;
}

float4 GetOutlinePosition(float4 vertex, float3 normal, half outlineWidth)
{
	outlineWidth = outlineWidth * 2;
	float signVar = (dot(normalize(vertex.xyz), normalize(normal)) < 0) ? -1 : 1;
	float4 result = float4(vertex.xyz + signVar * normalize(vertex.xyz) * outlineWidth, 1);
	return result;
}

Varyings vert(Attributes input)
{
	Varyings o = (Varyings) 0;
	o.uv = input.uv;
	float4 objPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
	float4 samplerTex = tex2Dlod(_Outline_Sampler, float4(TRANSFORM_TEX(o.uv, _Outline_Sampler), 0.0, 0));
	
	o.normalDir = UnityObjectToWorldNormal(input.normal);
	o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(input.tangent.xyz, 0.0)).xyz);
	o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * input.tangent.w);
	float3x3 tangentTransform = float3x3(o.tangentDir, o.bitangentDir, o.normalDir);
	
	float4 bakedNormal = (tex2Dlod(_BakedNormal, float4(TRANSFORM_TEX(o.uv, _BakedNormal), 0.0, 0)) * 2 - 1);
	float3 bakedNormalDir = normalize(mul(bakedNormal.rgb, tangentTransform));
	
	half near = _ProjectionParams.y;
	half far = _ProjectionParams.z;
	float outlineStep = smoothstep(far, near, distance(objPos.rgb, _WorldSpaceCameraPos));
	
	float baseWidth = _Outline_Width * 0.001;
	float outlineWidth = (baseWidth * outlineStep * samplerTex.rgb).r;
	
#if defined(UNITY_REVERSED_Z)
	_Offset_Z = _Offset_Z * -0.01;
#else
	float3 worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
	float3 eyeDir = UnityWorldSpaceViewDir(worldPos);
	half diff = clamp(max(0, dot(normalize(input.normal), eyeDir)), 0, 1);
	_Offset_Z = _Offset_Z * 0.01;
#endif
	
	
#ifdef _OUTLINE_NML
	float4 result = GetOutlineNormal(input.vertex.xyz, input.normal, bakedNormalDir, outlineWidth);
	o.pos = UnityObjectToClipPos(result);
	
#elif _OUTLINE_POS
	float4 result = GetOutlinePosition(input.vertex, input.normal, outlineWidth);
	o.pos = UnityObjectToClipPos(result);
#endif
	
	float4 viewDirectionVP = mul(UNITY_MATRIX_VP, float4(_WorldSpaceCameraPos.xyz, 1));
	o.pos.z = o.pos.z + _Offset_Z * viewDirectionVP.z;
	return o;
}


float4 frag(Varyings i) : SV_Target
{
	half3 ambientSkyColor = (unity_AmbientSky.rgb > 0.05) ? unity_AmbientSky.rgb * _Unlit_Intensity : half3(0.05, 0.05, 0.05) * _Unlit_Intensity;
	//float3 lightColor = (_LightColor0.rgb > 0.05) ? _LightColor0.rgb : ambientSkyColor.rgb;
	float3 lightColor = ambientSkyColor.rgb;
	float lightColorIntensity = (0.299 * lightColor.r + 0.587 * lightColor.g + 0.114 * lightColor.b);
	lightColor = (lightColorIntensity < 1) ? lightColor : lightColor / lightColorIntensity;
	lightColor = lerp(half3(1.0, 1.0, 1.0), lightColor, _Is_LightColor_Outline);
	
	float4 mainTex = tex2D(_MainTex, i.uv);
	float4 outlineTex = tex2D(_OutlineTex, i.uv);
	float3 blendA = (_Outline_Color.rgb * lightColor);
	float3 blendB = (_Outline_Color.rgb * lightColor * (_MainColor.rgb * mainTex.rgb));
	float3 lightBlendColor = lerp(blendA, blendB, _Is_BlendBaseColor);

		
#ifdef _IS_OUTLINE_CLIPPING_YES

	float4 clippingMask = tex2D(_ClippingMask, i.uv);
	float mainTexAlpha = mainTex.a;
	float baseMapClippingMaskAlpha = lerp(clippingMask.r, mainTexAlpha, _IsBaseMapAlphaAsClippingMask);
	float inverseClipping = lerp(baseMapClippingMaskAlpha, (1.0 - baseMapClippingMaskAlpha), _Inverse_Clipping);
	float clippingValue = saturate((inverseClipping + _ClippingLevel));
	clip(clippingValue - 0.5);
	float4 blendBaseColor = float4(lightBlendColor, clippingValue);
	float4 outlineColor = float4((outlineTex.rgb * _Outline_Color.rgb * lightColor), clippingValue);
	float4 finalColor = lerp(blendBaseColor, outlineColor, _Is_OutlineTex);
	return finalColor;
	
#else
	// _IS_OUTLINE_CLIPPING_NO
	float3 blendBaseColor = (lightBlendColor);
	float3 outlineColor = (outlineTex.rgb * _Outline_Color.rgb * lightColor);
	float3 finalColor = lerp(blendBaseColor, outlineColor, _Is_OutlineTex);	
	return float4(finalColor, 1.0);
	
#endif
}

#endif