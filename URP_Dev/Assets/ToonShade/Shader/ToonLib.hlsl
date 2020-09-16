#ifndef TOON_LIB
#define TOON_LIB

#include "ToonDifinition.hlsl"

#if (SHADER_LIBRARY_VERSION_MAJOR ==7 && SHADER_LIBRARY_VERSION_MINOR >= 3) || (SHADER_LIBRARY_VERSION_MAJOR >= 8)

	#ifdef _ADDITIONAL_LIGHTS
		#ifndef  REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
			#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
		#endif
	#endif
#else
	#ifdef _MAIN_LIGHT_SHADOWS
		#ifndef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
		#endif
	#endif

	#ifdef _ADDITIONAL_LIGHTS
		#ifndef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
			#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
		#endif
	#endif
#endif

fixed _Use_BaseAs1st;
fixed _Use_1stAs2nd;
fixed _Is_LightColor_Base;
fixed _Is_LightColor_1st_Shade;
fixed _Is_LightColor_2nd_Shade;
fixed _Is_LightColor_HighColor;
fixed _Is_LightColor_RimLight;
fixed _Is_LightColor_Ap_RimLight;
fixed _Is_LightColor_MatCap;
#ifdef _ANGELRING_ON
	fixed _Is_LightColor_AR;
#endif

sampler2D _MainTex; float4 _MainTex_ST;

#if TOON_URP
	//
#else
float4 _BaseColor;
#endif

float4 _Color;
sampler2D _1st_ShadeMap; float4 _1st_ShadeMap_ST;
sampler2D _2nd_ShadeMap; float4 _2nd_ShadeMap_ST;
sampler2D _NormalMap; float4 _NormalMap_ST;
sampler2D _ShadingGradeMap; float4 _ShadingGradeMap_ST;
float4 _1st_ShadeColor;
float4 _2nd_ShadeColor;
float _Tweak_SystemShadowsLevel;
float _Tweak_ShadingGradeMapLevel;
float _1st_ShadeColor_Step, _1st_ShadeColor_Feather;
float _2nd_ShadeColor_Step, _2nd_ShadeColor_Feather;
float _BlurLevelSGM;
fixed _Is_NormalMapToBase;
fixed _Set_SystemShadowsToBase;

float4 _HighColor;
sampler2D _HighColor_Tex; float4 _HighColor_Tex_ST;
float _HighColor_Power;
float _TweakHighColorOnShadow;
sampler2D _Set_HighColorMask; float4 _Set_HighColorMask_ST;
float _Tweak_HighColorMaskLevel;
fixed _Is_NormalMapToHighColor;
fixed _Is_SpecularToHighColor;
fixed _Is_BlendAddToHiColor;
fixed _Is_UseTweakHighColorOnShadow;

fixed _RimLight;
float4 _RimLightColor;
float _RimLight_Power;
float _RimLight_InsideMask;
float _Tweak_LightDirection_MaskLevel;
float4 _Ap_RimLightColor;
float _Ap_RimLight_Power;
sampler2D _Set_RimLightMask; float4 _Set_RimLightMask_ST;
float _Tweak_RimLightMaskLevel;
fixed _Is_NormalMapToRimLight;
fixed _Is_RimLight_FeatherOff;
fixed _Is_LightDirection_MaskOn;
fixed _Is_Antipodean_RimLight;
fixed _Is_ApRimLight_FeatherOff;

fixed _MatCap;
sampler2D _MatCap_Sampler; float4 _MatCap_Sampler_ST;
float _BlurLevelMatcap;
float4 _MatCapColor;
float _Tweak_MatCapUV;
float _Rotate_MatCapUV;
sampler2D _NormalMapForMatCap; float4 _NormalMapForMatCap_ST;
float _Rotate_NormalMapForMatCapUV;
float _TweakMatCapOnShadow;
sampler2D _Set_MatcapMask; float4 _Set_MatcapMask_ST;
float _Tweak_MatcapMaskLevel;

fixed _Is_BlendAddToMatCap;
fixed _Is_NormalMapForMatCap;
fixed _Is_UseTweakMatCapOnShadow;
fixed _Is_Ortho;
fixed _Is_InverseMatcapMask;
fixed _Is_CameraRolling;

#if TOON_URP
	//
#else
float _BumpScale;
#endif
float _BumpScaleMatcap;

fixed _Emissive;
sampler2D _Emissive_Tex; float4 _Emissive_Tex_ST;
float4 _Emissive_Color;
float _Rotate_EmissiveUV;
float _Base_Speed;
float _Scroll_EmissiveU;
float _Scroll_EmissiveV;
float4 _ColorShift;
float4 _ViewShift;
float _ColorShift_Speed;
float3 emissive;
fixed _Is_ViewCoord_Scroll;
fixed _Is_PingPong_Base;
fixed _Is_ColorShift;
fixed _Is_ViewShift;

// Environment
float _Unlit_Intensity;
float _StepOffset;

// ForwardDelta
fixed _Is_Filter_HiCutPointLightColor;

#if false	
fixed _Is_Filter_LightColor;
fixed _Is_BLD;
float _Offset_X_Axis_BLD;
float _Offset_Y_Axis_BLD;
fixed _Inverse_Z_Axis_BLD;
#endif

#ifdef _IS_TRANSCLIPPING_OFF
#elif _IS_TRANSCLIPPING_ON
	sampler2D _ClippingMask; float4 _ClippingMask_ST;
	fixed _IsBaseMapAlphaAsClippingMask;
	float _ClippingLevel;
	fixed _Inverse_Clipping;
	float _Tweak_transparency;
#endif

float _GI_Intensity;
#ifdef _ANGELRING_OFF
	//
#elif _ANGELRING_ON
	fixed _AngelRing;
	sampler2D _AngelRing_Sampler; float4 _AngelRing_Sampler_ST;
	float4 _AngelRing_Color;
	float _AR_OffsetU;
	float _AR_OffsetV;
	fixed _Is_AngelRingAlphaOn;
#endif

#define UNITY_PROJ_COORD(a) a
#define UNITY_SAMPLE_SCREEN_SHADOW(tex, uv) tex2Dproj( tex, UNITY_PROJ_COORD(uv) ).r
#define TEXTURE2D_SAMPLER2D(textureName, samplerName) Texture2D textureName; SamplerState samplerName 
		TEXTURE2D_SAMPLER2D(_RaytracedHardShadow, sampler_RaytracedHardShadow);

// #define PI 3.141592654
//float4 _RaytracedHardShadow_TexelSize;


float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
{
	float RotateUV_ang = _radian;
	float RotateUV_cos = cos(_time * RotateUV_ang);
	float RotateUV_sin = sin(_time * RotateUV_ang);
	return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
}

fixed3 DecodeLightProbe(fixed3 N)
{
	return ShadeSH9(float4(N, 1));
}

inline void InitializeStandardLitSurfaceDataToonShade(float2 uv, out SurfaceData outSurfaceData)
{
	half4 albedoAlpha = half4(1.0, 1.0, 1.0, 1.0);
	outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
	half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
#if _SPECULAR_SETUP
	outSurfaceData.metallic = 1.0h;
	outSurfaceData.specular = specGloss.rgb;
#else
	outSurfaceData.metallic = specGloss.r;
	outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);
#endif
	outSurfaceData.smoothness = specGloss.a;
	outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	outSurfaceData.occlusion = SampleOcclusion(uv);
	outSurfaceData.emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
}

half3 GlobalIlluminationToonShade(BRDFData brdfData, half3 bakedGI, half occlusion, half3 normalWS, half3 viewDirectionWS)
{
	half3 reflectVector = reflect(-viewDirectionWS, normalWS);
	half fresnelTerm = Pow4(1.0 - saturate(dot(normalWS, viewDirectionWS)));
	half3 indirectDiffuse = bakedGI * occlusion;
	half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, occlusion);
	return EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
}

struct ToonLight
{
	float3 direction;
	float3 color;
	float distanceAttenuation;
	float shadowAttenuation;
	int type;
};

#define INIT_TOONLIGHT(toonlight) \
	toonlight.direction = 0; \
	toonlight.color = 0; \
	toonlight.distanceAttenuation = 0; \
	toonlight.shadowAttenuation = 0; \
	toonlight.type = 0

half MainLightRealtimeShadowToonShade(float4 shadowCoord, float4 positionCS)
{
#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
	return 1.0h;
#endif
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half4 shadowParams = GetMainLightShadowParams();
#if defined(_RAYTRACINGSHADOW_ON)
	float4 screenPos =  ComputeScreenPos(positionCS/ positionCS.w);
	return SAMPLE_TEXTURE2D(_RaytracedHardShadow, sampler_RaytracedHardShadow, screenPos);
#endif 
	return SampleShadowmap(
		TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), 
		shadowCoord, 
		shadowSamplingData, 
		shadowParams, 
		false);
}

half AdditionalLightRealtimeShadowToonShade(int lightIndex, float3 positionWS, float4 positionCS)
{
#if defined(_RAYTRACINGSHADOW_ON)
	float4 screenPos = ComputeScreenPos(positionCS / positionCS.w);
	return SAMPLE_TEXTURE2D(_RaytracedHardShadow, sampler_RaytracedHardShadow, screenPos);
#endif

#if !defined(ADDITIONAL_LIGHT_CALCULATE_SHADOWS)
	return 1.0h;
#endif
	ShadowSamplingData shadowSamplingData = GetAdditionalLightShadowSamplingData();

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
	lightIndex = _AdditionalShadowsIndices[lightIndex];

	UNITY_BRANCH
	if (lightIndex < 0)
		return 1.0;

	float4 shadowCoord = mul(_AdditionalShadowsBuffer[lightIndex].worldToShadowMatrix, float4(positionWS, 1.0));
#else
	float4 shadowCoord = mul(_AdditionalLightsWorldToShadow[lightIndex], float4(positionWS, 1.0));
#endif

	half4 shadowParams = GetAdditionalLightShadowParams(lightIndex);
	return SampleShadowmap(
		TEXTURE2D_ARGS(_AdditionalLightsShadowmapTexture, sampler_AdditionalLightsShadowmapTexture), 
		shadowCoord, 
		shadowSamplingData, 
		shadowParams, 
		true);
}

ToonLight GetMainToonLight()
{
	ToonLight light;
	light.direction = _MainLightPosition.xyz;
	light.distanceAttenuation = unity_LightData.z;
#if defined(LIGHTMAP_ON) || defined(_MIXED_LIGHTING_SUBTRACTIVE)
	light.distanceAttenuation *= unity_ProbesOcclusion.x;
#endif
	light.shadowAttenuation = 1.0;
	light.color = _MainLightColor.rgb;
	light.type = _MainLightPosition.w;
	return light;
}

ToonLight GetMainToonLight(float4 shadowCoord, float4 positionCS)
{
	ToonLight light = GetMainToonLight();
	light.shadowAttenuation = MainLightRealtimeShadowToonShade(shadowCoord, positionCS);
	return light;
}

ToonLight GetAdditionalPerObjectToonLight(int perObjectLightIndex, float3 positionWS, float4 positionCS)
{
#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
	float4 lightPositionWS = _AdditionalLightsBuffer[perObjectLightIndex].position;
	half3 color = _AdditionalLightsBuffer[perObjectLightIndex].color.rgb;
	half4 distanceAndSpotAttenuation = _AdditionalLightsBuffer[perObjectLightIndex].attenuation;
	half4 spotDirection = _AdditionalLightsBuffer[perObjectLightIndex].spotDirection;
	half4 lightOcclusionProbeInfo = _AdditionalLightsBuffer[perObjectLightIndex].occlusionProbeChannels;
#else
	float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
	half3 color = _AdditionalLightsColor[perObjectLightIndex].rgb;
	half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
	half4 spotDirection = _AdditionalLightsSpotDir[perObjectLightIndex];
	half4 lightOcclusionProbeInfo = _AdditionalLightsOcclusionProbes[perObjectLightIndex];
#endif
	
	float3 lightVector = lightPositionWS.xyz - positionWS * lightPositionWS.w;
	float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);
	half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
	half attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy) * 
		AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw);

	ToonLight light;
	light.direction = lightDirection;
	light.distanceAttenuation = attenuation;
	light.shadowAttenuation = AdditionalLightRealtimeShadowToonShade(perObjectLightIndex, positionWS, positionCS);
	light.color = color;
	light.type = lightPositionWS.w;

#if defined(LIGHTMAP_ON) || defined(_MIXED_LIGHTING_SUBTRACTIVE)
	int probeChannel = lightOcclusionProbeInfo.x;
	half lightProbeContribution = lightOcclusionProbeInfo.y;
	half probeOcclusionValue = unity_ProbesOcclusion[probeChannel];
	light.distanceAttenuation *= max(probeOcclusionValue, lightProbeContribution);
#endif
	return light;
}

ToonLight GetAdditionalToonLight(uint i, float3 positionWS, float4 positionCS)
{
	int perObjectLightIndex = GetPerObjectLightIndex(i);
	return GetAdditionalPerObjectToonLight(perObjectLightIndex, positionWS, positionCS);
}

half3 GetLightColor(ToonLight light)
{
	return light.color * light.distanceAttenuation;
}

int DetermineToonShadeMainLightIndex(float3 posW, float4 shadowCoord, float4 positionCS)
{
	ToonLight mainLight;
	INIT_TOONLIGHT(mainLight);

	int mainLightIndex = -2;
	ToonLight nextLight = GetMainToonLight(shadowCoord, positionCS);
	if (nextLight.distanceAttenuation > mainLight.distanceAttenuation && nextLight.type == 0)
	{
		mainLight = nextLight;
		mainLightIndex = -1;
	}
	int lightCount = GetAdditionalLightsCount();
	for (int ii = 0; ii < lightCount; ++ii)
	{
		nextLight = GetAdditionalToonLight(ii, posW, positionCS);
		if (nextLight.distanceAttenuation > mainLight.distanceAttenuation && nextLight.type == 0)
		{
			mainLight = nextLight;
			mainLightIndex = ii;
		}
	}
	return mainLightIndex;
}

ToonLight GetMainToonShadeLightByID(int index, float3 posW, float4 shadowCoord, float4 positionCS)
{
	ToonLight mainLight;
	INIT_TOONLIGHT(mainLight);
	if (index == -2)
	{
		return mainLight;
	}
	if (index == -1)
	{
		return GetMainToonLight(shadowCoord, positionCS);
	}
	return GetAdditionalToonLight(index, posW, positionCS);
}

float3 GetLightDirection(float3 direction)
{
	float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);

#if false	
	float3 customLightDirection = normalize(
		mul(unity_ObjectToWorld,
			float4((
				(float3(1.0, 0.0, 0.0) * _Offset_X_Axis_BLD * 10) +
				(float3(0.0, 1.0, 0.0) * _Offset_Y_Axis_BLD * 10) +
				(float3(0.0, 0.0, -1.0) * lerp(-1.0, 1.0, _Inverse_Z_Axis_BLD))
			), 0)).xyz);
	
	float3 lightDirection = normalize(lerp(defaultLightDirection, direction.xyz, any(direction.xyz)));
	lightDirection = lerp(lightDirection, customLightDirection, _Is_BLD);
#endif
	
	float3 lightDirection = normalize(lerp(defaultLightDirection, direction.xyz, any(direction.xyz)));
	return lightDirection;
}


struct VertexInput
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 texcoord0 : TEXCOORD0;
#ifdef _ANGELRING_ON
	float2 texcoord1 : TEXCOORD1;
	float2 lightmapUV : TEXCOORD2;	
#elif _ANGELRING_OFF
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
	
#ifdef _ANGELRING_ON
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
	
#elif _ANGELRING_OFF
	float4 posWorld : TEXCOORD1;
	float3 normalDir : TEXCOORD2;
	float3 tangentDir : TEXCOORD3;
	float3 bitangentDir : TEXCOORD4;
	float mirrorFlag : TEXCOORD5;
	DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 6);
	half4 fogFactorAndVertexLight : TEXCOORD7;

	#ifndef _MAIN_LIGHT_SHADOWS
		float4 positionCS : TEXCOORD8;
		int mainLightID : TEXCOORD9;
	#else
		float4 shadowCoord : TEXCOORD8;
		float4 positionCS : TEXCOORD9;
		int mainLightID : TEXCOORD10;
	#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
#endif

};

#endif
